using System;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_EventDonation")]
    public partial class EventDonation : PaymentEntity, IIdentifierEntity
    {
        [EntityNameMap("msnfp_EventDonationid")]
        public Guid EventDonationId { get; set; }

        [EntityReferenceMap("msnfp_EventId")]
        [EntityLogicalName("msnfp_Event")]
        public Guid? EventId { get; set; }

        [EntityReferenceMap("TransactionCurrencyId")]
        [EntityLogicalName("TransactionCurrency")]
        public Guid? TransactionCurrencyId { get; set; }

        [EntityNameMap("msnfp_Amount")]
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        [EntityNameMap("msnfp_Description")]
        public string Description { get; set; }

        [EntityNameMap("msnfp_Name")]
        public string Name { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        public virtual TransactionCurrency TransactionCurrency { get; set; }

        public virtual Event Event { get; set; }
    }
}
