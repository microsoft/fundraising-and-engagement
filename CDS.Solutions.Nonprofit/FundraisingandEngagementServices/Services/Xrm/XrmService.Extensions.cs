using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FundraisingandEngagement.Models;
using FundraisingandEngagement.Models.Attributes;
using FundraisingandEngagement.Models.Entities;
using Xrm.Crm.WebApi.Models;

namespace FundraisingandEngagement.Services.Xrm
{
	public static class XrmServiceExtensions
	{
		public static async Task<T> GetAsync<T>(this IXrmService xrmService, Guid id, params string[] properties)
			where T : class, new()
		{
			var type = typeof(T);
			var attribute = type.GetCustomAttribute<EntityLogicalName>();
			if (attribute == null)
				throw new XrmException($"{type.Name} type is missing {nameof(EntityLogicalName)} attribute.");

			var crmEntity = await xrmService.GetAsync(attribute.LogicalName, id, properties);
			var entityProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
				.Where(p => p.CanWrite && p.CanRead)
				.ToArray();

			return ConvertToEntity<T>(crmEntity, entityProperties);
		}

		public static async Task<IReadOnlyList<T>> GetListAsync<T>(this IXrmService xrmService, string entityCollection, params string[] properties)
			where T : class, new()
		{
			var crmEntities = await xrmService.GetListAsync(entityCollection, properties);

			var list = new List<T>();
			var entityProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
				.Where(p => p.CanWrite && p.CanRead)
				.ToArray();

			foreach (var crmEntity in crmEntities)
				list.Add(ConvertToEntity<T>(crmEntity, entityProperties));

			return list;
		}

		public static async Task<IReadOnlyList<T>> GetFilteredListAsync<T>(this IXrmService xrmService, string entityCollection, string filter, params string[] properties)
			where T : class, new()
		{
			var crmEntities = await xrmService.GetFilteredListAsync(entityCollection, filter, properties);

			var list = new List<T>();
			var entityProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
				.Where(p => p.CanWrite && p.CanRead)
				.ToArray();

			foreach (var crmEntity in crmEntities)
				list.Add(ConvertToEntity<T>(crmEntity, entityProperties));

			return list;
		}

		public static async Task UpdateAsync<T>(this IXrmService xrmService, T model)
		{
			var entity = ConvertToCrmEntity(model);
			await xrmService.UpdateAsync(entity);
		}

		public static async Task<Guid> CreateAsync<T>(this IXrmService xrmService, T model)
		{
			var entity = ConvertToCrmEntity(model);
			return await xrmService.CreateAsync(entity);
		}

		public static async Task DisassociateAsync<T>(this IXrmService xrmService, T model, List<string> navigationProperties)
		{
			var entity = ConvertToCrmEntity(model);

			foreach (var navigationProperty in navigationProperties)
			{
				await xrmService.DisassociateAsync(entity, navigationProperty);
			}
		}

		private static T ConvertToEntity<T>(Entity crmEntity, PropertyInfo[] properties) where T : class, new()
		{
			var entity = new T();

			foreach (var property in properties)
			{
				var entityMapName = property.GetEntityNameMap();
				if (!String.IsNullOrEmpty(entityMapName))
				{
					var enitityValue = crmEntity[entityMapName];
					if (enitityValue != null)
						property.SetValueConvertIfNeeded(entity, enitityValue);

					continue;
				}

				var optionSetName = property.GetEntityOptionSetName();
				if (!String.IsNullOrEmpty(optionSetName))
				{
					var optionSetValue = crmEntity[optionSetName];
					if (optionSetValue != null)
						property.SetValueConvertIfNeeded(entity, optionSetValue);

					continue;
				}

				var refName = property.GetEntityReferenceMap();
				var logicalName = property.GetReferenceLogicalName();
				if (!String.IsNullOrEmpty(refName) && !String.IsNullOrEmpty(logicalName))
				{
					var refId = crmEntity[refName];
					if (refId != null)
						property.SetValueConvertIfNeeded(entity, refId);
				}

				var attributeName = property.GetEntityFormattedAttributeMap();
				if (!String.IsNullOrEmpty(attributeName))
				{
					var value = crmEntity.FormattedValues.ContainsKey(attributeName) ? crmEntity.FormattedValues[attributeName] : String.Empty;
					if (!String.IsNullOrEmpty(value))
					{
						property.SetValueConvertIfNeeded(entity, value);
					}
				}
			}

			return entity;
		}


		private static Entity ConvertToCrmEntity<T>(T model)
		{
			var entity = new Entity();
			var type = typeof(T);
			var keyName = String.Concat(type.Name, "Id");

			var attribute = type.GetCustomAttribute<EntityLogicalName>();
			if (attribute == null)
				throw new XrmException($"'{type.Name}' Entity missing EntityLogicalName attribute.");

			entity.LogicalName = attribute.LogicalName;

			var properties = type
				.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
				.Where(p => p.CanWrite && p.CanRead)
				.ToList();

			foreach (var property in properties)
			{
				var propValue = property.GetValue(model, null);
				if (propValue == null)
					continue;

				if (String.Equals(property.Name, keyName) && propValue is Guid key)
					entity.Id = key;

				if (property.Name == "CustomerId")
				{
					var entityReferenceMap = property.GetEntityReferenceMap();

					if (model is ContactPaymentEntity customer && customer.CustomerId.HasValue && !String.IsNullOrEmpty(entityReferenceMap))
					{
						if (customer.CustomerIdType == 1)
						{
							entity[$"{entityReferenceMap}_account"] = new EntityReference("account", customer.CustomerId.Value);
						}
						else if (customer.CustomerIdType == 2)
						{
							entity[$"{entityReferenceMap}_contact"] = new EntityReference("contact", customer.CustomerId.Value);
						}
					}
				}
				else
				{
					var entityMap = property.GetCustomAttribute<EntityNameMap>();
					var entityMapName = entityMap?.EntityName?.ToLower();
					if (entityMap != null && !String.IsNullOrEmpty(entityMapName))
					{
						if (!String.IsNullOrEmpty(entityMap.Format) && propValue is IFormattable formattable)
							entity[entityMapName] = formattable.ToString(entityMap.Format, CultureInfo.CurrentCulture);
						else
							entity[entityMapName] = propValue;

						continue;
					}

					var optionSetName = property.GetEntityOptionSetName();
					if (!String.IsNullOrEmpty(optionSetName))
					{
						if (propValue is int intValue)
							entity[optionSetName] = intValue;
						else if (propValue is Enum enumValue)
							entity[optionSetName] = Convert.ToInt32(enumValue);

						continue;
					}

					var refName = property.GetEntityReferenceMap();
					var logicalName = property.GetReferenceLogicalName();
					if (!String.IsNullOrEmpty(refName) && !String.IsNullOrEmpty(logicalName) && propValue is Guid id)
					{
						entity[refName] = new EntityReference(logicalName, id); ;
					}
				}
			}

			return entity;
		}

		private static string GetEntityFormattedAttributeMap(this PropertyInfo propIn)
		{
			var formattedAttribute = propIn.GetCustomAttribute<EntityFormattedAttributeMap>();

			if (formattedAttribute != null)
				return formattedAttribute.EntityFormattedValue;

			return String.Empty;
		}

		private static string GetEntityNameMap(this PropertyInfo propIn)
		{
			var entityName = propIn.GetCustomAttribute<EntityNameMap>();

			if (entityName != null)
				return entityName.EntityName.ToLower();

			return String.Empty;
		}

		private static string GetEntityOptionSetName(this PropertyInfo propIn)
		{
			var optionSetName = propIn.GetCustomAttribute<EntityOptionSetMap>();

			if (optionSetName != null)
				return optionSetName.EntityOptionSet.ToLower();

			return String.Empty;
		}

		private static string GetEntityReferenceMap(this PropertyInfo propIn)
		{
			var entityRef = propIn.GetCustomAttribute<EntityReferenceMap>();

			if (entityRef != null)
				return entityRef.EntityReference;

			return String.Empty;
		}

		private static string GetReferenceLogicalName(this PropertyInfo propIn)
		{
			var logicalName = propIn.GetCustomAttribute<EntityLogicalName>();

			if (logicalName != null)
				return logicalName.LogicalName.ToLower();

			return String.Empty;
		}

		private static void SetValueConvertIfNeeded(this PropertyInfo property, object entity, object value)
		{
			if (entity == null || value == null)
				return;

			if (InternalSetValueConvertIfNeeded(property, property.PropertyType, entity, value))
				return;

			var nullableType = Nullable.GetUnderlyingType(property.PropertyType);
			if (nullableType != null)
			{
				var nullableValue = value;
				if (nullableType != value.GetType())
				{
					if (InternalSetValueConvertIfNeeded(property, nullableType, entity, value))
						return;
				}

				property.SetValue(entity, nullableValue);
				return;
			}

			throw new NotSupportedException($"Not supported value {value} of type {value.GetType().Name} to be set for property {property.Name} of type {property.PropertyType.Name}");
		}

		private static bool InternalSetValueConvertIfNeeded(PropertyInfo property, Type propertyType, object entity, object value)
		{
			if (propertyType == value.GetType())
			{
				property.SetValue(entity, value);
				return true;
			}

			if (value is IConvertible && propertyType.IsEnum)
			{
				var convertValue = Enum.ToObject(propertyType, value);
				property.SetValue(entity, convertValue);
				return true;
			}

			if (value is IConvertible && typeof(IConvertible).IsAssignableFrom(propertyType))
			{
				var convertValue = Convert.ChangeType(value, propertyType);
				property.SetValue(entity, convertValue);
				return true;
			}

			if (value is string v && propertyType == typeof(Guid))
			{
				if (Guid.TryParse(v, out var guid))
				{
					property.SetValue(entity, guid);
					return true;
				}
			}

			if (value is EntityReference er)
			{
				property.SetValue(entity, er.Id);
				return true;
			}

			return false;
		}
	}
}
