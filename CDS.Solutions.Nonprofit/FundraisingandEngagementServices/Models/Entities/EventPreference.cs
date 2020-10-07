using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_EventPreference")]
    public partial class EventPreference : PaymentEntity, IIdentifierEntity
	{
        public Guid EventPreferenceId { get; set; }

        public string Identifier { get; set; } 

        public Guid? EventId { get; set; }

        public Guid? PreferenceId { get; set; } 

        public Guid? PreferenceCategoryId { get; set; }

        public virtual PreferenceCategory PreferenceCategory { get; set; }
    }
}
