using System;

namespace FundraisingandEngagement.Models.Attributes
{
	[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
	public sealed class EntityReferenceMap : Attribute
	{
		public string EntityReference { get; }

		public EntityReferenceMap(string entityReference)
		{
			EntityReference = entityReference;
		}
	}
}
