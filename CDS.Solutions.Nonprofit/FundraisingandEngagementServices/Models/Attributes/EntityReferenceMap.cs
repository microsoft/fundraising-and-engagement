using System;

namespace FundraisingandEngagement.Models.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class EntityReferenceMap : Attribute
    {
        public string EntityReference { get; }

        public bool PushToDataverse { get; set; } // must be false by default

        public EntityReferenceMap(string entityReference)
        {
            EntityReference = entityReference;
        }
    }
}