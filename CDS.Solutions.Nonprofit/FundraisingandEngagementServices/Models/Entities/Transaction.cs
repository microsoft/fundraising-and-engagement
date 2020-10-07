using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;
using FundraisingandEngagement.Models.Enums;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_transaction")]
    public partial class Transaction : ContactPaymentEntity, ITransactionResultEntity
    {
        public Transaction()
        {
            Refund = new HashSet<Refund>();
            Response = new HashSet<Response>();
            SyncLogs = new HashSet<SyncLog>();
        }

        [EntityNameMap("msnfp_transactionid")]
        public Guid TransactionId { get; set; }


        [EntityLogicalName("msnfp_designation")]
        [EntityReferenceMap("msnfp_DesignationId")]
        public Guid? DesignationId { get; set; }

        [EntityLogicalName("campaign")]
        [EntityReferenceMap("msnfp_OriginatingCampaignId")]
        public Guid? OriginatingCampaignId { get; set; }

        [EntityLogicalName("contact")]
        [EntityReferenceMap("msnfp_RelatedConstituentId")]
        public Guid? ConstituentId { get; set; }

        [EntityLogicalName("msnfp_appeal")]
        [EntityReferenceMap("msnfp_AppealId")]
        public Guid? AppealId { get; set; }

        [EntityLogicalName("msnfp_event")]
        [EntityReferenceMap("msnfp_EventId")]
        public Guid? EventId { get; set; }

        [EntityLogicalName("msnfp_eventpackage")]
        [EntityReferenceMap("msnfp_EventPackageId")]
        public Guid? EventPackageId { get; set; }

        [EntityLogicalName("msnfp_giftaidreturn")]
        [EntityReferenceMap("msnfp_Ga_ReturnId")]
        public Guid? GaReturnId { get; set; }

        [EntityLogicalName("msnfp_GiftBatch")]
        [EntityReferenceMap("msnfp_GiftBatchId")]
        public Guid? GiftBatchId { get; set; }

        [EntityLogicalName("msnfp_MembershipCategory")]
        [EntityReferenceMap("msnfp_MembershipCategoryId")]
        public Guid? MembershipId { get; set; }

        [EntityLogicalName("msnfp_Membership")]
        [EntityReferenceMap("msnfp_MembershipInstanceId")]
        public Guid? MembershipInstanceId { get; set; }

        [EntityLogicalName("msnfp_package")]
        [EntityReferenceMap("msnfp_PackageId")]
        public Guid? PackageId { get; set; }

        [ForeignKey(nameof(TaxReceipt))]
        [EntityLogicalName("msnfp_receipt")]
        [EntityReferenceMap("msnfp_TaxReceiptId")]
        public Guid? TaxReceiptId { get; set; }

        [EntityLogicalName("msnfp_DonorCommitment")]
        [EntityReferenceMap("msnfp_DonorCommitmentId")]
        public Guid? DonorCommitmentId { get; set; }

        [EntityLogicalName("msnfp_TransactionBatch")]
        [EntityReferenceMap("msnfp_TransactionBatchId")]
        public Guid? TransactionBatchId { get; set; }

        [EntityLogicalName("msnfp_TributeOrMemory")]
        [EntityReferenceMap("msnfp_TributeId")]
        public Guid? TributeId { get; set; }

        [EntityLogicalName("msnfp_Configuration")]
        [EntityReferenceMap("msnfp_ConfigurationId")]
        public Guid? ConfigurationId { get; set; }

        [EntityLogicalName("msnfp_paymentschedule")]
        [EntityReferenceMap("msnfp_Transaction_PaymentScheduleId")]
        public Guid? TransactionPaymentScheduleId { get; set; }

        [EntityLogicalName("msnfp_paymentmethod")]
        [EntityReferenceMap("msnfp_Transaction_PaymentMethodId")]
        public Guid? TransactionPaymentMethodId { get; set; }

        [EntityLogicalName("msnfp_PaymentProcessor")]
        [EntityReferenceMap("msnfp_PaymentProcessorId")]
        public Guid? PaymentProcessorId { get; set; }

		[ForeignKey(nameof(TransactionCurrency))]
        [EntityLogicalName("transactioncurrency")]
        [EntityReferenceMap("transactioncurrencyid")]
        public Guid? TransactionCurrencyId { get; set; }

        public Guid? OwningBusinessUnitId { get; set; }




        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Amount_Receipted")]
        public decimal? AmountReceipted { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Amount_Membership")]
        public decimal? AmountMembership { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Amount_NonReceiptable")]
        public decimal? AmountNonReceiptable { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Amount_Tax")]
        public decimal? AmountTax { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_amount")]
        public decimal? Amount { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Ref_Amount_Receipted")]
        public decimal? RefAmountReceipted { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Ref_Amount_Membership")]
        public decimal? RefAmountMembership { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Ref_Amount_Nonreceiptable")]
        public decimal? RefAmountNonreceiptable { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Ref_Amount_Tax")]
        public decimal? RefAmountTax { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Ref_Amount")]
        public decimal? RefAmount { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Amount_Transfer")]
        public decimal? AmountTransfer { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Ga_Amount_Claimed")]
        public decimal? GaAmountClaimed { get; set; }


        [EntityOptionSetMap("msnfp_anonymous")]
        public int? Anonymous { get; set; }


        [EntityNameMap("msnfp_Appraiser")]
        public string Appraiser { get; set; }
        [EntityNameMap("msnfp_Billing_City")]
        public string BillingCity { get; set; }
        [EntityNameMap("msnfp_Billing_Country")]
        public string BillingCountry { get; set; }
        [EntityNameMap("msnfp_Billing_Line1")]
        public string BillingLine1 { get; set; }
        [EntityNameMap("msnfp_Billing_Line2")]
        public string BillingLine2 { get; set; }
        [EntityNameMap("msnfp_Billing_Line3")]
        public string BillingLine3 { get; set; }
        [EntityNameMap("msnfp_Billing_Postalcode")]
        public string BillingPostalCode { get; set; }
        [EntityNameMap("msnfp_Billing_StateorProvince")]
        public string BillingStateorProvince { get; set; }

        [EntityOptionSetMap("msnfp_CcBrandCode")]
        public int? CcBrandCode { get; set; }
        [EntityNameMap("msnfp_ChargeonCreate")]
        public bool? ChargeonCreate { get; set; }
        [EntityNameMap("msnfp_ChequeNumber")]
        public string ChequeNumber { get; set; }
        [EntityNameMap("msnfp_ChequeWireDate")]
        public DateTime? ChequeWireDate { get; set; }


        [EntityNameMap("msnfp_CurrentRetry")]
        public int? CurrentRetry { get; set; }


        [EntityNameMap("msnfp_bookdate")]
        public DateTime? BookDate { get; set; }
        [EntityNameMap("msnfp_DateRefunded")]
        public DateTime? DateRefunded { get; set; }
        [EntityNameMap("msnfp_Ga_DeliveryCode")]
        public int? GaDeliveryCode { get; set; }
        [EntityNameMap("msnfp_receiveddate", Format = "yyyy-MM-dd")]
        public DateTime? ReceivedDate { get; set; }
        [EntityNameMap("msnfp_Emailaddress1")]
        public string Emailaddress1 { get; set; }

        [EntityNameMap("msnfp_Firstname")]
        public string FirstName { get; set; }

        [EntityNameMap("msnfp_Ga_ApplicableCode")]
        public int? GaApplicableCode { get; set; }

        [EntityNameMap("msnfp_TransactionDescription")]
        public string TransactionDescription { get; set; }
        [EntityOptionSetMap("msnfp_dataentrysource")]
        public int? DataEntrySource { get; set; }



        [EntityOptionSetMap("msnfp_PaymentTypeCode")]
        public PaymentTypeCode? PaymentTypeCode { get; set; }




        [EntityNameMap("msnfp_LastFailedRetry", Format = "yyyy-MM-dd")]
        public DateTime? LastFailedRetry { get; set; }
        [EntityNameMap("msnfp_LastName")]
        public string LastName { get; set; }

        [EntityNameMap("msnfp_MobilePhone")]
        public string MobilePhone { get; set; }
        [EntityNameMap("msnfp_NextFailedRetry", Format = "yyyy-MM-dd")]
        public DateTime? NextFailedRetry { get; set; }
        [EntityNameMap("msnfp_OrganizationName")]
        public string OrganizationName { get; set; }

        [EntityOptionSetMap("msnfp_ReceiptPreferenceCode")]
        public int? ReceiptPreferenceCode { get; set; }
        [EntityNameMap("msnfp_ReturnedDate")]
        public DateTime? ReturnedDate { get; set; }
        [EntityNameMap("msnfp_Telephone1")]
        public string Telephone1 { get; set; }
        [EntityNameMap("msnfp_Telephone2")]
        public string Telephone2 { get; set; }
        [EntityNameMap("msnfp_ThirdPartyReceipt")]
        public string ThirdPartyReceipt { get; set; }
        [EntityNameMap("msnfp_name")]
        public string Name { get; set; }

        [EntityNameMap("msnfp_dataentryreference")]
        public string DataEntryReference { get; set; }
        [EntityNameMap("msnfp_InvoiceIdentifier")]
        public string InvoiceIdentifier { get; set; }
        [EntityNameMap("msnfp_TransactionFraudCode")]
        public string TransactionFraudCode { get; set; }
        [EntityNameMap("msnfp_TransactionIdentifier")]
        public string TransactionIdentifier { get; set; }
        [EntityNameMap("msnfp_TransactionNumber")]
        public string TransactionNumber { get; set; }
        [EntityNameMap("msnfp_TransactionResult")]
        public string TransactionResult { get; set; }
        [EntityNameMap("msnfp_TributeName")]
        public string TributeName { get; set; }
        [EntityOptionSetMap("msnfp_TributeCode")]
        public int? TributeCode { get; set; }
        [EntityNameMap("msnfp_TributeAcknowledgement")]
        public string TributeAcknowledgement { get; set; }

        [EntityNameMap("msnfp_TributeMessage")]
        public string TributeMessage { get; set; }
        [EntityNameMap("msnfp_ValidationDate", Format = "yyyy-MM-dd")]
        public DateTime? ValidationDate { get; set; }
        [EntityNameMap("msnfp_ValidationPerformed")]
        public bool? ValidationPerformed { get; set; }

        [EntityOptionSetMap("msnfp_typecode")]
        public TransactionTypeCode? TypeCode { get; set; }

        [EntityNameMap("msnfp_depositdate", Format = "yyyy-MM-dd")]
        public DateTime? DepositDate { get; set; }

        public virtual Receipt TaxReceipt { get; set; }

        public virtual TransactionCurrency TransactionCurrency { get; set; }

        public virtual PaymentProcessor PaymentProcessor { get; set; }

        public virtual Configuration Configuration { get; set; }

        public virtual Event Event { get; set; }

        public virtual PaymentMethod TransactionPaymentMethod { get; set; }

        public virtual PaymentSchedule TransactionPaymentSchedule { get; set; }

        public virtual MembershipCategory MembershipCategory { get; set; }

        public virtual Membership Membership { get; set; }

        public virtual ICollection<Refund> Refund { get; set; }

        public virtual ICollection<Response> Response { get; set; }

        public virtual ICollection<SyncLog> SyncLogs { get; set; }

        public virtual ICollection<Account> Account { get; set; }

        public virtual ICollection<Contact> Contact { get; set; }

        [EntityNameMap("msnfp_EmployerMatches")]
        public bool? EmployerMatches { get; set; }
    }
}
