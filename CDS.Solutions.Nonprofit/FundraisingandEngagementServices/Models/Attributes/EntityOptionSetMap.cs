using System;

namespace FundraisingandEngagement.Models.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class EntityOptionSetMap : Attribute
    {
        public string EntityOptionSet { get; }

        public bool PushToDataverse { get; set; } // must be false by default

        public EntityOptionSetMap(string entityName)
        {
            EntityOptionSet = entityName;
        }
    }
}