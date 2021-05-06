using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FundraisingandEngagement.Models.Attributes;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.Utils
{
    public static class DataverseMappingMetadataExtensions
    {
        public static string EntityLogicalName(this Type entityModelType)
        {
            return entityModelType.GetCustomAttribute<EntityLogicalName>()?.LogicalName?.ToLower();
        }

        public static string PropertyLogicalName(this PropertyInfo entityModelProperty)
        {
            return entityModelProperty.GetEntityNameMap()
                   ?? entityModelProperty.GetEntityOptionSetName()
                   ?? entityModelProperty.GetEntityReferenceMap();
        }

        private static string GetEntityNameMap(this PropertyInfo propIn)
        {
            var entityName = propIn.GetCustomAttribute<EntityNameMap>();
            return entityName?.EntityName?.ToLower();
        }

        private static string GetEntityOptionSetName(this PropertyInfo propIn)
        {
            var optionSetName = propIn.GetCustomAttribute<EntityOptionSetMap>();
            return optionSetName?.EntityOptionSet?.ToLower();
        }

        private static string GetEntityReferenceMap(this PropertyInfo propIn)
        {
            var entityRef = propIn.GetCustomAttribute<EntityReferenceMap>();
            return entityRef?.EntityReference?.ToLower();
        }
    }
}