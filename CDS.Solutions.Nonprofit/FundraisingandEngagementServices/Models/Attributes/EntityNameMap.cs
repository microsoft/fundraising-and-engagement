using System;

namespace FundraisingandEngagement.Models.Attributes
{
	[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
	public sealed class EntityNameMap : Attribute
	{
		public string EntityName { get; }

		public string Format { get; set; }

		public EntityNameMap(string entityName)
		{
			EntityName = entityName;
		}
	}
}
