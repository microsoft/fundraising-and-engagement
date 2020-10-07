using FundraisingandEngagement.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_EventSponsorship")]
    public partial class EventSponsorship : PaymentEntity, IIdentifierEntity
    {

        [EntityNameMap("msnfp_eventsponsorshipId")]
        public Guid EventSponsorshipId { get; set; }


        [EntityReferenceMap("msnfp_EventId")]
        [EntityLogicalName("msnfp_Event")]
        public Guid? EventId { get; set; }

        [EntityReferenceMap("transactioncurrencyid")]
        [EntityLogicalName("transactioncurrency")]
        public Guid? TransactionCurrencyId { get; set; }

		[EntityNameMap("msnfp_advantage")]
        [Column(TypeName = "money")]
        public decimal? Advantage { get; set; }

        [EntityNameMap("msnfp_amount")]
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

		[EntityNameMap("msnfp_amount_nonreceiptable")]
		[Column(TypeName = "money")]
		public decimal? AmountNonReceiptable { get; set; }

		[EntityNameMap("msnfp_amount_receipted")]
		[Column(TypeName = "money")]
		public decimal? AmountReceipted { get; set; }

        [EntityNameMap("msnfp_date")]
        public DateTime? Date { get; set; }

        [EntityNameMap("msnfp_description")]
        public string Description { get; set; }

		// check
        [EntityNameMap("msnfp_Name")]
        public string Name { get; set; }

        [EntityNameMap("msnfp_order")]
        public int? Order { get; set; }

        [EntityNameMap("msnfp_quantity")]
        public int? Quantity { get; set; }

        [EntityNameMap("msnfp_fromamount")]
        [Column(TypeName = "money")]
        public decimal? FromAmount { get; set; }

		// check
        [EntityNameMap("msnfp_Val_Available")]
        public int? ValAvailable { get; set; }

        [EntityNameMap("msnfp_val_sold")]
        public decimal? ValSold { get; set; }

        [EntityNameMap("msnfp_identifier")]
        public string Identifier { get; set; }

        [EntityNameMap("msnfp_sum_sold")]
        public int? SumSold { get; set; }

        public virtual TransactionCurrency TransactionCurrency { get; set; }

        public virtual Event Event { get; set; }

        public virtual ICollection<Sponsorship> Sponsorship { get; set; }

	}
}
