using System;

namespace FundraisingandEngagement.Models.Attributes
{
    public class EntityFormattedAttributeMap : Attribute
    {
        private string attribute;

        public EntityFormattedAttributeMap(string attribute)
        {
            this.attribute = attribute;
        }

        public virtual string EntityFormattedValue
        {
            get { return attribute; }
            set { attribute = value; }
        }
    }
}