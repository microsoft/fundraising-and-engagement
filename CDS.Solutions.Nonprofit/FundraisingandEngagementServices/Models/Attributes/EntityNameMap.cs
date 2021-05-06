using System;

namespace FundraisingandEngagement.Models.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class EntityNameMap : Attribute
    {
        public string EntityName { get; }

        public string Format { get; set; }

        public bool PushToDataverse { get; set; } // must be false by default

        public EntityNameMap(string entityName)
        {
            EntityName = entityName;
        }
    }
}