using System;

namespace FundraisingandEngagement.Models.Attributes
{
	[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
	public sealed class EntityOptionSetMap : Attribute
	{
		public string EntityOptionSet { get; }

		public EntityOptionSetMap(string entityName)
		{
			EntityOptionSet = entityName;
		}
	}
}
