using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.DataSynchronization;
using Data.Utils;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.DataverseSynchronization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;

namespace FundraisingandEngagement.Data.DataSynchronization
{
    public class DbEntitySynchronizer : IDbEntitySynchronizer
    {
        private static readonly TimeSpan DefaultCommandTimeout = TimeSpan.FromMinutes(15);

        private readonly IDataverseSynchronizer _dataverseSynchronizer;
        private readonly PaymentContext _dbContext;

        public static DbEntitySynchronizer CreateWithConfiguredContext(DbContextOptions<PaymentContext> dbContextOptions, IOrganizationService organizationService, ILogger logger,
            ChangeTrackingDataverseSynchronizer.FullSyncDelegate fullSyncDelegate)
        {
            PaymentContext dbContext = new PaymentContext(dbContextOptions);
            dbContext.Database.SetCommandTimeout(DefaultCommandTimeout);
            var dbEntityRepository = new DbEntityRepository(dbContext, logger);
            var dbDataVersionTokenRepository = new DbDataVersionTokenRepository(dbContext);
            var synchronizer = new ChangeTrackingDataverseSynchronizer(organizationService, dbEntityRepository, dbDataVersionTokenRepository, logger);
            if (fullSyncDelegate != null)
            {
                synchronizer.OnBeforeFullSync += fullSyncDelegate;
            }
            return new DbEntitySynchronizer(synchronizer, dbContext);
        }

        public DbEntitySynchronizer(IDataverseSynchronizer dataverseSynchronizer, PaymentContext dbContext)
        {
            this._dataverseSynchronizer = dataverseSynchronizer;
            this._dbContext = dbContext;
        }

        public void SynchronizeEntitiesToDbTransitively(List<Type> entitiesToSynchronize)
        {
            var model = this._dbContext.Model;
            var dbEntityTypes = entitiesToSynchronize.Select(clrType => model.FindEntityType(clrType));

            var reachableEntities = TransitiveClosure.ReachableNodes(
                dbEntityTypes,
                // we need both directions: referenced entities because of FK constraints inserts, referencing because of FK constraints for deletes
                entity => getReferencedEntities(entity).Concat(getReferencingEntities(entity)) // both directions are necessary, one for creates, the other for deletes
            );

            var topologicalSort = new TopologicalSort<IEntityType>(
                getReferencedEntities,
                entityType => entityType.GetTableName()
            );
            var orderedEntityNames = topologicalSort.ReverseTopologicalSort(reachableEntities)
                .Select(it => it.ClrType.EntityLogicalName())
                .Where(it => it != null);
            this._dataverseSynchronizer.Synchronize(orderedEntityNames);
        }

        private static IEnumerable<IEntityType> getReferencingEntities(IEntityType entityType)
        {
            return entityType.GetReferencingForeignKeys().Select(it => it.DeclaringEntityType);
        }

        private static IEnumerable<IEntityType> getReferencedEntities(IEntityType entityType)
        {
            return entityType.GetForeignKeys().Select(it => it.PrincipalEntityType);
        }
    }
}