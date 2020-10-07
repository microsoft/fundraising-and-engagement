using System;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityNameMap("msnfp_refund")]
    public partial class Refund : ContactPaymentEntity, IIdentifierEntity
    {
        [EntityLogicalName("msnfp_refundId")]
        public Guid RefundId { get; set; }




        [EntityReferenceMap("msnfp_TransactionId")]
        [EntityLogicalName("msnfp_Transaction")]
        public Guid? TransactionId { get; set; }

        [EntityReferenceMap("TransactionCurrencyId")]
        [EntityLogicalName("TransactionCurrency")]
        public Guid? TransactionCurrencyId { get; set; }



        [EntityNameMap("msnfp_Amount_Receipted")]
        [Column(TypeName = "money")]
        public decimal? AmountReceipted { get; set; }

        [EntityNameMap("msnfp_Amount_Membership")]
        [Column(TypeName = "money")]
        public decimal? AmountMembership { get; set; }

        [EntityNameMap("msnfp_Ref_Amount_Membership")]
        [Column(TypeName = "money")]
        public decimal? RefAmountMembership { get; set; }

        [EntityNameMap("msnfp_Amount_NonReceiptable")]
        [Column(TypeName = "money")]
        public decimal? AmountNonReceiptable { get; set; }

        [EntityNameMap("msnfp_Ref_Amount_Nonreceiptable")]
        [Column(TypeName = "money")]
        public decimal? RefAmountNonreceiptable { get; set; }

        [EntityNameMap("msnfp_Ref_Amount_Receipted")]
        [Column(TypeName = "money")]
        public decimal? RefAmountReceipted { get; set; }

        [EntityNameMap("msnfp_Amount_Tax")]
        [Column(TypeName = "money")]
        public decimal? AmountTax { get; set; }

        [EntityNameMap("msnfp_Ref_Amount_Tax")]
        [Column(TypeName = "money")]
        public decimal? RefAmountTax { get; set; }

        [EntityNameMap("msnfp_ChequeNumber")]
        public string ChequeNumber { get; set; }

        [EntityNameMap("msnfp_BookDate")]
        public DateTime? BookDate { get; set; }

        [EntityNameMap("msnfp_ReceivedDate")]
        public DateTime? ReceivedDate { get; set; }

        [EntityOptionSetMap("msnfp_RefundTypeCode")]
        public int? RefundTypeCode { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        [EntityNameMap("msnfp_Amount")]
        public decimal? Amount { get; set; }

        [EntityNameMap("msnfp_Ref_Amount")]
        [Column(TypeName = "money")]
        public decimal? RefAmount { get; set; }

        [EntityNameMap("msnfp_TransactionIdentifier")]
        public string TransactionIdentifier { get; set; }

        [EntityNameMap("msnfp_TransactionResult")]
        public string TransactionResult { get; set; }

        public virtual TransactionCurrency TransactionCurrency { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
