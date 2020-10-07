using System;

namespace FundraisingandEngagement.Models.Attributes
{
	[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public sealed class EntityLogicalName : Attribute
    {
        public string LogicalName { get; }

        public EntityLogicalName(string logicalName)
        {
            LogicalName = logicalName;
        }
    }
}
