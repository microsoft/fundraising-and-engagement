using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;
using FundraisingandEngagement.Models.Enums;

namespace FundraisingandEngagement.Models.Entities
{
    [EntityLogicalName("msnfp_transaction")]
    public partial class Transaction : PaymentEntity, IContactPaymentEntity, ITransactionResultEntity
    {
        public Transaction()
        {
            Refund = new HashSet<Refund>();
            Response = new HashSet<Response>();
        }

        [EntityNameMap("msnfp_transactionid", PushToDataverse = true)]
        public Guid TransactionId { get; set; }

        [EntityReferenceMap("msnfp_CustomerId", PushToDataverse = true)]
        public Guid? CustomerId { get; set; }

        public int? CustomerIdType { get; set; }

        [EntityLogicalName("msnfp_designation")]
        [EntityReferenceMap("msnfp_DesignationId", PushToDataverse = true)]
        public Guid? DesignationId { get; set; }

        [EntityLogicalName("campaign")]
        [EntityReferenceMap("msnfp_OriginatingCampaignId", PushToDataverse = true)]
        public Guid? OriginatingCampaignId { get; set; }

        [EntityLogicalName("contact")]
        [EntityReferenceMap("msnfp_RelatedConstituentId", PushToDataverse = true)]
        public Guid? ConstituentId { get; set; }

        [EntityLogicalName("msnfp_appeal")]
        [EntityReferenceMap("msnfp_AppealId", PushToDataverse = true)]
        public Guid? AppealId { get; set; }

        [EntityLogicalName("msnfp_event")]
        [EntityReferenceMap("msnfp_EventId", PushToDataverse = true)]
        public Guid? EventId { get; set; }

        [EntityLogicalName("msnfp_eventpackage")]
        [EntityReferenceMap("msnfp_EventPackageId", PushToDataverse = true)]
        public Guid? EventPackageId { get; set; }

        [EntityLogicalName("msnfp_giftaidreturn")]
        [EntityReferenceMap("msnfp_Ga_ReturnId", PushToDataverse = true)]
        public Guid? GaReturnId { get; set; }

        [EntityLogicalName("msnfp_GiftBatch")]
        [EntityReferenceMap("msnfp_GiftBatchId", PushToDataverse = true)]
        public Guid? GiftBatchId { get; set; }

        [EntityLogicalName("msnfp_MembershipCategory")]
        [EntityReferenceMap("msnfp_MembershipCategoryId", PushToDataverse = true)]
        public Guid? MembershipId { get; set; }

        [EntityLogicalName("msnfp_Membership")]
        [EntityReferenceMap("msnfp_MembershipInstanceId", PushToDataverse = true)]
        public Guid? MembershipInstanceId { get; set; }

        [EntityLogicalName("msnfp_package")]
        [EntityReferenceMap("msnfp_PackageId", PushToDataverse = true)]
        public Guid? PackageId { get; set; }

        [ForeignKey(nameof(TaxReceipt))]
        [EntityLogicalName("msnfp_receipt")]
        [EntityReferenceMap("msnfp_TaxReceiptId", PushToDataverse = true)]
        public Guid? TaxReceiptId { get; set; }

        [EntityLogicalName("msnfp_DonorCommitment")]
        [EntityReferenceMap("msnfp_DonorCommitmentId", PushToDataverse = true)]
        public Guid? DonorCommitmentId { get; set; }

        [EntityLogicalName("msnfp_TransactionBatch")]
        [EntityReferenceMap("msnfp_TransactionBatchId", PushToDataverse = true)]
        public Guid? TransactionBatchId { get; set; }

        [EntityLogicalName("msnfp_TributeOrMemory")]
        [EntityReferenceMap("msnfp_TributeId", PushToDataverse = true)]
        public Guid? TributeId { get; set; }

        [EntityLogicalName("msnfp_Configuration")]
        [EntityReferenceMap("msnfp_ConfigurationId", PushToDataverse = true)]
        public Guid? ConfigurationId { get; set; }

        [EntityLogicalName("msnfp_paymentschedule")]
        [EntityReferenceMap("msnfp_Transaction_PaymentScheduleId", PushToDataverse = true)]
        public Guid? TransactionPaymentScheduleId { get; set; }

        [EntityLogicalName("msnfp_paymentmethod")]
        [EntityReferenceMap("msnfp_Transaction_PaymentMethodId", PushToDataverse = true)]
        public Guid? TransactionPaymentMethodId { get; set; }

        [EntityLogicalName("msnfp_PaymentProcessor")]
        [EntityReferenceMap("msnfp_PaymentProcessorId", PushToDataverse = true)]
        public Guid? PaymentProcessorId { get; set; }

        [ForeignKey(nameof(TransactionCurrency))]
        [EntityLogicalName("transactioncurrency")]
        [EntityReferenceMap("transactioncurrencyid", PushToDataverse = true)]
        public Guid? TransactionCurrencyId { get; set; }

        public Guid? OwningBusinessUnitId { get; set; }




        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Amount_Receipted", PushToDataverse = true)]
        public decimal? AmountReceipted { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Amount_Membership", PushToDataverse = true)]
        public decimal? AmountMembership { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Amount_NonReceiptable", PushToDataverse = true)]
        public decimal? AmountNonReceiptable { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Amount_Tax", PushToDataverse = true)]
        public decimal? AmountTax { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_amount", PushToDataverse = true)]
        public decimal? Amount { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Ref_Amount_Receipted", PushToDataverse = true)]
        public decimal? RefAmountReceipted { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Ref_Amount_Membership", PushToDataverse = true)]
        public decimal? RefAmountMembership { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Ref_Amount_Nonreceiptable", PushToDataverse = true)]
        public decimal? RefAmountNonreceiptable { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Ref_Amount_Tax", PushToDataverse = true)]
        public decimal? RefAmountTax { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Ref_Amount", PushToDataverse = true)]
        public decimal? RefAmount { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Amount_Transfer", PushToDataverse = true)]
        public decimal? AmountTransfer { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_Ga_Amount_Claimed", PushToDataverse = true)]
        public decimal? GaAmountClaimed { get; set; }


        [EntityOptionSetMap("msnfp_anonymous", PushToDataverse = true)]
        public int? Anonymous { get; set; }


        [EntityNameMap("msnfp_Appraiser", PushToDataverse = true)]
        public string Appraiser { get; set; }
        [EntityNameMap("msnfp_Billing_City", PushToDataverse = true)]
        public string BillingCity { get; set; }
        [EntityNameMap("msnfp_Billing_Country", PushToDataverse = true)]
        public string BillingCountry { get; set; }
        [EntityNameMap("msnfp_Billing_Line1", PushToDataverse = true)]
        public string BillingLine1 { get; set; }
        [EntityNameMap("msnfp_Billing_Line2", PushToDataverse = true)]
        public string BillingLine2 { get; set; }
        [EntityNameMap("msnfp_Billing_Line3", PushToDataverse = true)]
        public string BillingLine3 { get; set; }
        [EntityNameMap("msnfp_Billing_Postalcode", PushToDataverse = true)]
        public string BillingPostalCode { get; set; }
        [EntityNameMap("msnfp_Billing_StateorProvince", PushToDataverse = true)]
        public string BillingStateorProvince { get; set; }

        [EntityOptionSetMap("msnfp_CcBrandCode", PushToDataverse = true)]
        public int? CcBrandCode { get; set; }
        [EntityNameMap("msnfp_ChargeonCreate", PushToDataverse = true)]
        public bool? ChargeonCreate { get; set; }
        [EntityNameMap("msnfp_ChequeNumber", PushToDataverse = true)]
        public string ChequeNumber { get; set; }
        [EntityNameMap("msnfp_ChequeWireDate", PushToDataverse = true)]
        public DateTime? ChequeWireDate { get; set; }


        [EntityNameMap("msnfp_CurrentRetry", PushToDataverse = true)]
        public int? CurrentRetry { get; set; }


        [EntityNameMap("msnfp_bookdate", PushToDataverse = true)]
        public DateTime? BookDate { get; set; }
        [EntityNameMap("msnfp_DateRefunded", PushToDataverse = true)]
        public DateTime? DateRefunded { get; set; }
        [EntityNameMap("msnfp_Ga_DeliveryCode", PushToDataverse = true)]
        public int? GaDeliveryCode { get; set; }
        [EntityNameMap("msnfp_receiveddate", Format = "yyyy-MM-dd", PushToDataverse = true)]
        public DateTime? ReceivedDate { get; set; }
        [EntityNameMap("msnfp_Emailaddress1", PushToDataverse = true)]
        public string Emailaddress1 { get; set; }

        [EntityNameMap("msnfp_Firstname", PushToDataverse = true)]
        public string FirstName { get; set; }

        [EntityNameMap("msnfp_Ga_ApplicableCode", PushToDataverse = true)]
        public int? GaApplicableCode { get; set; }

        [EntityNameMap("msnfp_TransactionDescription", PushToDataverse = true)]
        public string TransactionDescription { get; set; }
        [EntityOptionSetMap("msnfp_dataentrysource", PushToDataverse = true)]
        public int? DataEntrySource { get; set; }



        [EntityOptionSetMap("msnfp_PaymentTypeCode", PushToDataverse = true)]
        public PaymentTypeCode? PaymentTypeCode { get; set; }




        [EntityNameMap("msnfp_LastFailedRetry", Format = "yyyy-MM-dd", PushToDataverse = true)]
        public DateTime? LastFailedRetry { get; set; }
        [EntityNameMap("msnfp_LastName", PushToDataverse = true)]
        public string LastName { get; set; }

        [EntityNameMap("msnfp_MobilePhone", PushToDataverse = true)]
        public string MobilePhone { get; set; }
        [EntityNameMap("msnfp_NextFailedRetry", Format = "yyyy-MM-dd", PushToDataverse = true)]
        public DateTime? NextFailedRetry { get; set; }
        [EntityNameMap("msnfp_OrganizationName", PushToDataverse = true)]
        public string OrganizationName { get; set; }

        [EntityOptionSetMap("msnfp_ReceiptPreferenceCode", PushToDataverse = true)]
        public int? ReceiptPreferenceCode { get; set; }
        [EntityNameMap("msnfp_ReturnedDate", PushToDataverse = true)]
        public DateTime? ReturnedDate { get; set; }
        [EntityNameMap("msnfp_Telephone1", PushToDataverse = true)]
        public string Telephone1 { get; set; }
        [EntityNameMap("msnfp_Telephone2", PushToDataverse = true)]
        public string Telephone2 { get; set; }
        [EntityNameMap("msnfp_ThirdPartyReceipt", PushToDataverse = true)]
        public string ThirdPartyReceipt { get; set; }
        [EntityNameMap("msnfp_name", PushToDataverse = true)]
        public string Name { get; set; }

        [EntityNameMap("msnfp_dataentryreference", PushToDataverse = true)]
        public string DataEntryReference { get; set; }
        [EntityNameMap("msnfp_InvoiceIdentifier", PushToDataverse = true)]
        public string InvoiceIdentifier { get; set; }
        [EntityNameMap("msnfp_TransactionFraudCode", PushToDataverse = true)]
        public string TransactionFraudCode { get; set; }
        [EntityNameMap("msnfp_TransactionIdentifier", PushToDataverse = true)]
        public string TransactionIdentifier { get; set; }
        [EntityNameMap("msnfp_TransactionNumber", PushToDataverse = true)]
        public string TransactionNumber { get; set; }
        [EntityNameMap("msnfp_TransactionResult", PushToDataverse = true)]
        public string TransactionResult { get; set; }
        [EntityNameMap("msnfp_TributeName", PushToDataverse = true)]
        public string TributeName { get; set; }
        [EntityOptionSetMap("msnfp_TributeCode", PushToDataverse = true)]
        public int? TributeCode { get; set; }
        [EntityNameMap("msnfp_TributeAcknowledgement", PushToDataverse = true)]
        public string TributeAcknowledgement { get; set; }

        [EntityNameMap("msnfp_TributeMessage", PushToDataverse = true)]
        public string TributeMessage { get; set; }
        [EntityNameMap("msnfp_ValidationDate", Format = "yyyy-MM-dd", PushToDataverse = true)]
        public DateTime? ValidationDate { get; set; }
        [EntityNameMap("msnfp_ValidationPerformed", PushToDataverse = true)]
        public bool? ValidationPerformed { get; set; }

        [EntityOptionSetMap("msnfp_typecode", PushToDataverse = true)]
        public TransactionTypeCode? TypeCode { get; set; }

        [EntityNameMap("msnfp_depositdate", Format = "yyyy-MM-dd", PushToDataverse = true)]
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

        public virtual ICollection<Account> Account { get; set; }

        public virtual ICollection<Contact> Contact { get; set; }

        [EntityNameMap("msnfp_EmployerMatches", PushToDataverse = true)]
        public bool? EmployerMatches { get; set; }
    }
}