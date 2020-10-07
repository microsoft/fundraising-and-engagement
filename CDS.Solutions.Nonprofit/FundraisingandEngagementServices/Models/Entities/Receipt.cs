using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_Receipt")]
	public partial class Receipt : ContactPaymentEntity, IIdentifierEntity
	{
		public Receipt()
		{
			InverseReplacesReceipt = new HashSet<Receipt>();
		}

		[EntityNameMap("msnfp_Receiptid")]
		public Guid ReceiptId { get; set; }

		[EntityReferenceMap("msnfp_ReceiptStackid")]
		[EntityLogicalName("msnfp_ReceiptStack")]
		public Guid? ReceiptStackId { get; set; }

		[EntityReferenceMap("msnfp_PaymentScheduleId")]
		[EntityLogicalName("msnfp_PaymentSchedule")]
		public Guid? PaymentScheduleId { get; set; }

		[EntityReferenceMap("msnfp_ReplacesReceiptId")]
		[EntityLogicalName("msnfp_ReplacesReceipt")]
		public Guid? ReplacesReceiptId { get; set; }

		[EntityReferenceMap("TransactionCurrencyId")]
		[EntityLogicalName("TransactionCurrency")]
		public Guid? TransactionCurrencyId { get; set; }

		[EntityNameMap("msnfp_ExpectedTaxCredit")]
		[Column(TypeName = "money")]
		public decimal? ExpectedTaxCredit { get; set; }

		[EntityNameMap("msnfp_GeneratedorPrinted")]
		public double? GeneratedorPrinted { get; set; }

		[EntityNameMap("msnfp_LastDonationDate")]
		public DateTime? LastDonationDate { get; set; }

		[EntityNameMap("msnfp_Amount_NonReceiptable")]
		[Column(TypeName = "money")]
		public decimal? AmountNonReceiptable { get; set; }

		//[EntityOptionSetMap("msnfp_TransactionCount")]
		public int? TransactionCount { get; set; }

		[EntityOptionSetMap("msnfp_PreferredLanguageCode")]
		public int? PreferredLanguageCode { get; set; }

		[EntityNameMap("msnfp_ReceiptNumber")]
		public string ReceiptNumber { get; set; }

		[EntityOptionSetMap("msnfp_ReceiptGeneration")]
		public int? ReceiptGeneration { get; set; }

		[EntityNameMap("msnfp_ReceiptIssueDate")]
		public DateTime? ReceiptIssueDate { get; set; }

		[EntityNameMap("msnfp_ReceiptStatus")]
		public string ReceiptStatus { get; set; }

		//[EntityOptionSetMap("msnfp_Amount_Receipted")]
		public decimal? AmountReceipted { get; set; }

		[EntityNameMap("msnfp_Identifier")]
		public string Identifier { get; set; }

		[EntityNameMap("msnfp_Amount")]
		[Column(TypeName = "money")]
		public decimal? Amount { get; set; }

		[EntityNameMap("msnfp_Printed")]
		public DateTime? Printed { get; set; }

		[EntityOptionSetMap("msnfp_deliveryCode")]
		public int? DeliveryCode { get; set; }

		[EntityOptionSetMap("msnfp_EmailDeliveryStatusCode")]
		public int? EmailDeliveryStatusCode { get; set; }

		public virtual TransactionCurrency TransactionCurrency { get; set; }

		public virtual PaymentSchedule PaymentSchedule { get; set; }

		public virtual ReceiptStack ReceiptStack { get; set; }

		public virtual Receipt ReplacesReceipt { get; set; }

		public virtual ICollection<Receipt> InverseReplacesReceipt { get; set; }
	}
}
