using System;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_eventproduct")]
    public partial class EventProduct : PaymentEntity, IIdentifierEntity
    {
        [EntityNameMap("msnfp_eventproductid")]
        public Guid EventProductId { get; set; }

        [EntityReferenceMap("msnfp_eventid")]
        [EntityLogicalName("msnfp_event")]
        public Guid? EventId { get; set; }

        [EntityReferenceMap("transactioncurrencyid")]
        [EntityLogicalName("transactioncurrency")]
        public Guid? TransactionCurrencyId { get; set; }

		[EntityNameMap("msnfp_amount")]
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        [EntityNameMap("msnfp_description")]
        public string Description { get; set; }

        [EntityNameMap("msnfp_detailamount")]
        [Column(TypeName = "money")]
        public decimal? AmountReceipted { get; set; }

        [EntityNameMap("msnfp_Amount_NonReceiptable")]
        [Column(TypeName = "money")]
        public decimal? AmountNonReceiptable { get; set; }

        [EntityOptionSetMap("msnfp_maxproductsperpackage")]
        public int? MaxProducts { get; set; }

        [EntityNameMap("msnfp_name")]
        public string Name { get; set; }

        [EntityOptionSetMap("msnfp_productsavailable")]
        public int? ValAvailable { get; set; }

        [EntityOptionSetMap("msnfp_quantity")]
        public int? Quantity { get; set; }

        [EntityNameMap("msnfp_restrictperregistration")]
        public bool? RestrictPerRegistration { get; set; }

        [EntityOptionSetMap("msnfp_sum_productssold")]
        public int? ValSold { get; set; }

        [EntityNameMap("msnfp_Amount_Tax")]
        [Column(TypeName = "money")]
        public decimal? AmountTax { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        [EntityNameMap("msnfp_val_products")]
        [Column(TypeName = "money")]
        public decimal? SumSold { get; set; }

        public virtual TransactionCurrency TransactionCurrency { get; set; }

        public virtual Event Event { get; set; }
    }
}
