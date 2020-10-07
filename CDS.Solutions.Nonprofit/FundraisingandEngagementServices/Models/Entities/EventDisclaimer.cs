using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_EventDisclaimer")]
    public partial class EventDisclaimer : PaymentEntity, IIdentifierEntity
    {
        [EntityNameMap("msnfp_EventDisclaimerid")]
        public Guid EventDisclaimerId { get; set; }

        [EntityReferenceMap("msnfp_EventId")]
        [EntityLogicalName("msnfp_Event")]
        public Guid? EventId { get; set; }

        [EntityNameMap("msnfp_Description")]
        public string Description { get; set; }

        [EntityNameMap("msnfp_Name")]
        public string Name { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        public virtual Event Event { get; set; }
    }
}
