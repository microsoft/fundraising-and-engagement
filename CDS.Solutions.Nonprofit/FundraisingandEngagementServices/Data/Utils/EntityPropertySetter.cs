using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using Microsoft.Xrm.Sdk;

namespace Data.Utils
{
    static class EntityPropertySetter
    {
        public static void SetValueWithConversion(object targetObject, PropertyInfo property, object value)
        {
            if (targetObject == null)
            {
                throw new ArgumentException("Target object cannot be null");
            }

            if (value == null)
            {
                setNullValue(targetObject, property);
                return;
            }

            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (!SetValueWithConversionForType(targetObject, property, targetType, value))
            {
                throw new NotSupportedException($"Value {value} of type {value.GetType().Name} cannot be set as value of property {property.Name} of type {property.PropertyType.Name}");
            }
        }

        private static void setNullValue(object targetObject, PropertyInfo property)
        {
            property.SetValue(targetObject, null); // works also for value types - will set default value - see https://stackoverflow.com/a/10288816
        }

        private static bool SetValueWithConversionForType(object targetObject, PropertyInfo property, Type propertyType, object value)
        {
            // This is an ugly special case, but this is what Dataverse plugins were doing behind the scenes before writing to Azure
            if (targetObject is IContactPaymentEntity contactPaymentEntity && property.Name == nameof(IContactPaymentEntity.CustomerId) && value is EntityReference customerIdReference)
            {
                contactPaymentEntity.CustomerId = customerIdReference.Id;
                contactPaymentEntity.CustomerIdType = CustomerIdType.LogicalNameToType(customerIdReference.LogicalName);
                return true;
            }

            if (propertyType == value.GetType())
            {
                property.SetValue(targetObject, value);
                return true;
            }

            if (value is OptionSetValue optionSetValue)
            {
                return SetValueWithConversionForType(targetObject, property, propertyType, optionSetValue.Value);
            }

            if (value is Money moneyValue)
            {
                return SetValueWithConversionForType(targetObject, property, propertyType, moneyValue.Value);
            }

            if (value is EntityReference reference)
            {
                property.SetValue(targetObject, reference.Id);
                return true;
            }

            if (value is IConvertible && propertyType.IsEnum)
            {
                var convertedValue = Enum.ToObject(propertyType, value);
                property.SetValue(targetObject, convertedValue);
                return true;
            }

            if (value is IConvertible && typeof(IConvertible).IsAssignableFrom(propertyType))
            {
                var convertValue = Convert.ChangeType(value, propertyType);
                property.SetValue(targetObject, convertValue);
                return true;
            }

            if (value is string v && propertyType == typeof(Guid))
            {
                if (Guid.TryParse(v, out var guid))
                {
                    property.SetValue(targetObject, guid);
                    return true;
                }
            }

            return false;
        }
    }
}