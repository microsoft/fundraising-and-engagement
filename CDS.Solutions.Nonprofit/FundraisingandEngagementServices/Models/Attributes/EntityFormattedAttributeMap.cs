using System;

namespace FundraisingandEngagement.Models.Attributes
{
	[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
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
