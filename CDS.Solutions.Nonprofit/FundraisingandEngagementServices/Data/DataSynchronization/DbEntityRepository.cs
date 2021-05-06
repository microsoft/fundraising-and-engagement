using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Data.DataSynchronization;
using Data.Utils;
using FundraisingandEngagement.DataverseSynchronization;
using FundraisingandEngagement.Models.Attributes;
using FundraisingandEngagement.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;

namespace FundraisingandEngagement.Data.DataSynchronization
{
    public class DbEntityRepository : IEntityRepository
    {
        internal const int MaxDeleteBatchSize = 100;

        private readonly PaymentContext _dbContext;
        private readonly ILogger _logger;
        private readonly IDictionary<string, EntityMetadata> _entityMetadataByEntityName;
        private readonly MethodInfo _contextSetMethod;

        public DbEntityRepository(PaymentContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;

            _contextSetMethod = dbContext.GetType().GetMethod(nameof(DbContext.Set), new Type[0]);
            if (_contextSetMethod == null)
            {
                throw new InvalidOperationException($"{nameof(PaymentContext)} doesn't have a method Set()");
            }

            _entityMetadataByEntityName = getEntityMetadataByEntityName(dbContext);
        }

        private static Dictionary<string, EntityMetadata> getEntityMetadataByEntityName(PaymentContext dbContext)
        {
            return dbContext.GetType().GetProperties()
                .Where(property => property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(property =>
                {
                    Type entityType = property.PropertyType.GetGenericArguments()[0];
                    PropertyInfo primaryKeyProperty = dbContext.Model.FindEntityType(entityType)?.FindPrimaryKey()?.Properties[0]?.PropertyInfo;
                    return new EntityMetadata(entityType, primaryKeyProperty);
                })
                .Where(entity => entity.EntityLogicalName != null && entity.PrimaryKeyProperty != null)
                .ToDictionary(it => it.EntityLogicalName, it => it);
        }

        public void Upsert(IEnumerable<Entity> changedEntities)
        {
            // Note this is not 100% reliable under high concurrency - primary key conflicts can occur
            // See https://michaeljswart.com/2017/07/sql-server-upsert-patterns-and-antipatterns/
            using var transaction = this._dbContext.Database.BeginTransaction();
            var now = DateTime.Now;
            foreach (var changedEntity in changedEntities)
            {
                var entityMetadata = getEntityMetadata(changedEntity.LogicalName);
                var primaryKey = changedEntity.Id;

                if (primaryKey == null)
                {
                    throw new ArgumentException($"Given entity of type {changedEntity.LogicalName} does not have Id");
                }

                // ReSharper disable once ConvertToUsingDeclaration
                var dbEntity = this._dbContext.Find(entityMetadata.EntityType, primaryKey);
                if (dbEntity == null)
                {
                    dbEntity = Activator.CreateInstance(entityMetadata.EntityType);
                    entityMetadata.PrimaryKeyProperty.SetValue(dbEntity, primaryKey);
                    copyAttributes(changedEntity, dbEntity, entityMetadata, now);
                    this._dbContext.Add(dbEntity);
                }
                else
                {
                    copyAttributes(changedEntity, dbEntity, entityMetadata, now);
                    this._dbContext.Update(dbEntity);
                }
            }

            this._dbContext.SaveChanges();
            transaction.Commit();
        }

        private static void copyAttributes(Entity from, object to, EntityMetadata entityMetadata, DateTime syncDate)
        {
            if (to is PaymentEntity paymentEntity)
            {
                paymentEntity.Deleted = false;
                paymentEntity.DeletedDate = null;
                paymentEntity.SyncDate = syncDate;
            }

            foreach (var (propertyName, propertyInfo) in entityMetadata.AttributePropertiesByName)
            {
                if (propertyInfo == entityMetadata.PrimaryKeyProperty)
                {
                    continue; // handled elsewhere
                }

                if (shouldSkipProperty(from, to, propertyName))
                {
                    continue;
                }

                var value = from.Attributes.ContainsKey(propertyName) ? from.Attributes[propertyName] : null;
                try
                {
                    EntityPropertySetter.SetValueWithConversion(to, propertyInfo, value);
                }
                catch (Exception e)
                {
                    throw new EntityRepositoryException($"Error mapping value of {propertyName} of {from.LogicalName}", e);
                }
            }
        }

        private static bool shouldSkipProperty(Entity from, object to, string propertyName)
        {
            if (to is PaymentSchedule paymentSchedule)
            {
                // Ugly special case, but this is the original logic from API Update handler:
                // preserve later LastPaymentDate or NextPaymentDate if present
                // This should ideally be removed in favor of having a single source of truth for every field, e.g. by saving changes directly to Dataverse instead of going through SQL
                var newValue = from.Attributes.ContainsKey(propertyName) ? from.Attributes[propertyName] : null;
                switch (propertyName)
                {
                    case "msnfp_lastpaymentdate":
                        return paymentSchedule.LastPaymentDate.HasValue && isLaterThanNullable(paymentSchedule.LastPaymentDate.Value, newValue);
                    case "msnfp_nextpaymentdate":
                        return paymentSchedule.NextPaymentDate.HasValue && isLaterThanNullable(paymentSchedule.NextPaymentDate.Value, newValue);
                }
            }
            return false;
        }

        public void Delete(string entityLogicalName, Guid entityId)
        {
            var entityMetadata = getEntityMetadata(entityLogicalName);
            // ReSharper disable once ConvertToUsingDeclaration
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var dbEntity = _dbContext.Find(entityMetadata.EntityType, entityId);
                if (dbEntity != null)
                {
                    _dbContext.Remove(dbEntity);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public void RetainOnly(string entityLogicalName, IEnumerable<Guid> entityIdsToRetain)
        {
            var entityMetadata = getEntityMetadata(entityLogicalName);

            dynamic dbSet = _contextSetMethod.MakeGenericMethod(entityMetadata.EntityType).Invoke(_dbContext, null);
            var parameterExpression = Expression.Parameter(entityMetadata.EntityType, "entity");
            dynamic selectIdExpression = Expression.Lambda( // expression that gets the primary key property from an entity object, e.g. (entity => entity.DesignationId)
                Expression.Property(
                    parameterExpression,
                    entityMetadata.PrimaryKeyProperty
                ),
                parameterExpression
            );

            var allIds = ((IQueryable<Guid>)Queryable.Select(dbSet, selectIdExpression)).ToList();
            var idsToRemove = allIds.Except(entityIdsToRetain).ToList();

            _logger.LogInformation("Deleting {toDelete} entities of type {name}, retaining {toRetain}", idsToRemove.Count, entityLogicalName, allIds.Count - idsToRemove.Count);
            // ReSharper disable once PossibleMultipleEnumeration
            for (int batchIndex = 0; batchIndex < idsToRemove.Count; batchIndex += MaxDeleteBatchSize)
            {
                var batch = idsToRemove.Skip(batchIndex).Take(MaxDeleteBatchSize);
                foreach (var entityId in batch)
                {
                    var entityInstance = Activator.CreateInstance(entityMetadata.EntityType);
                    entityMetadata.PrimaryKeyProperty.SetValue(entityInstance!, entityId);
                    _dbContext.Attach(entityInstance);
                    _dbContext.Remove(entityInstance);
                }

                _dbContext.SaveChanges();
            }
        }

        private static bool isLaterThanNullable(DateTime date1, object date2)
        {
            return !(date2 is DateTime date2Typed) || date1 > date2Typed;
        }

        private EntityMetadata getEntityMetadata(string entityLogicalName)
        {
            if (!this._entityMetadataByEntityName.ContainsKey(entityLogicalName))
            {
                throw new ArgumentException("Unknown entity '" + entityLogicalName + "'");
            }

            return this._entityMetadataByEntityName[entityLogicalName];
        }

        private readonly struct EntityMetadata
        {
            public Type EntityType { get; }
            public string EntityLogicalName { get; }
            public PropertyInfo PrimaryKeyProperty { get; }
            public Dictionary<string, PropertyInfo> AttributePropertiesByName { get; }

            public EntityMetadata(Type entityType, PropertyInfo primaryKeyProperty)
            {
                EntityType = entityType;
                EntityLogicalName = entityType.EntityLogicalName();
                PrimaryKeyProperty = primaryKeyProperty;
                AttributePropertiesByName = getAttributePropertiesByName(entityType);
            }

            private static Dictionary<string, PropertyInfo> getAttributePropertiesByName(Type entityType)
            {
                return entityType.GetProperties()
                    .Select(prop => new { Property = prop, Name = prop.PropertyLogicalName() })
                    .Where(it => it.Name != null)
                    .ToDictionary(it => it.Name, it => it.Property);
            }
        }
    }
}