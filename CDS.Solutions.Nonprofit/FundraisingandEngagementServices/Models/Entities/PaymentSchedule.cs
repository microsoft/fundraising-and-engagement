using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;
using FundraisingandEngagement.Models.Enums;

namespace FundraisingandEngagement.Models.Entities
{
    [EntityLogicalName("msnfp_PaymentSchedule")]
    public partial class PaymentSchedule : ContactPaymentEntity
    {
        public PaymentSchedule()
        {
            Receipt = new HashSet<Receipt>();
            Response = new HashSet<Response>();
            Transaction = new HashSet<Transaction>();
        }

        [EntityNameMap("msnfp_PaymentScheduleid")]
        public Guid PaymentScheduleId { get; set; }

        [EntityReferenceMap("msnfp_PaymentMethodId")]
        [EntityLogicalName("msnfp_paymentmethod")]
        public Guid? PaymentMethodId { get; set; }

        [EntityReferenceMap("msnfp_AppealId")]
        [EntityLogicalName("msnfp_Appeal")]
        public Guid? AppealId { get; set; }

        [EntityLogicalName("campaign")]
        [EntityReferenceMap("msnfp_OriginatingCampaignId")]
        public Guid? OriginatingCampaignId { get; set; }

        [EntityReferenceMap("msnfp_ConfigurationId")]
        [EntityLogicalName("msnfp_Configuration")]
        public Guid? ConfigurationId { get; set; }

        [EntityReferenceMap("msnfp_ConstituentId")]
        [EntityLogicalName("contact")]
        public Guid? ConstituentId { get; set; }

        [EntityLogicalName("msnfp_designation")]
        [EntityReferenceMap("msnfp_DesignationId")]
        public Guid? DesignationId { get; set; }

        [EntityReferenceMap("msnfp_EventId")]
        [EntityLogicalName("msnfp_Event")]
        public Guid? EventId { get; set; }

        [EntityReferenceMap("msnfp_EventPackageId")]
        [EntityLogicalName("msnfp_EventPackage")]
        public Guid? EventPackageId { get; set; }

        [EntityReferenceMap("msnfp_GiftBatchId")]
        [EntityLogicalName("msnfp_GiftBatch")]
        public Guid? GiftBatchId { get; set; }

        [EntityLogicalName("msnfp_MembershipCategory")]
        [EntityReferenceMap("msnfp_MembershipCategoryId")]
        public Guid? MembershipCategoryId { get; set; }

        [EntityLogicalName("msnfp_Membership")]
        [EntityReferenceMap("msnfp_MembershipInstanceId")]
        public Guid? MembershipId { get; set; }

        [EntityReferenceMap("msnfp_PackageId")]
        [EntityLogicalName("msnfp_Package")]
        public Guid? PackageId { get; set; }

        [EntityLogicalName("msnfp_receipt")]
        [EntityReferenceMap("msnfp_TaxReceiptId")]
        public Guid? TaxReceiptId { get; set; }

        [EntityReferenceMap("msnfp_TributeId")]
        [EntityLogicalName("msnfp_Tribute")]
        public Guid? TributeId { get; set; }

        [EntityReferenceMap("msnfp_TransactionBatchId")]
        [EntityLogicalName("msnfp_TransactionBatch")]
        public Guid? TransactionBatchId { get; set; }

        [EntityReferenceMap("msnfp_PaymentProcessorId")]
        [EntityLogicalName("msnfp_PaymentProcessor")]
        public Guid? PaymentProcessorId { get; set; }

        [EntityReferenceMap("transactioncurrencyid")]
        [EntityLogicalName("transactioncurrency")]
        public Guid? TransactionCurrencyId { get; set; }

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
        [EntityNameMap("msnfp_recurringamount")]
        public decimal? RecurringAmount { get; set; }

        [EntityNameMap("msnfp_firstpaymentdate")]
        public DateTime? FirstPaymentDate { get; set; }

        [EntityNameMap("msnfp_FrequencyInterval")]
        public int? FrequencyInterval { get; set; }

        [EntityOptionSetMap("msnfp_frequencystartcode")]
        public FrequencyStart? FrequencyStartCode { get; set; }

        [EntityNameMap("msnfp_nextpaymentdate", Format = "yyyy-MM-dd")]
        public DateTime? NextPaymentDate { get; set; }

        [EntityOptionSetMap("msnfp_frequency")]
        public FrequencyType? Frequency { get; set; }

        [EntityOptionSetMap("msnfp_cancelationcode")]
        public int? CancelationCode { get; set; }

        [EntityNameMap("msnfp_cancellationnote")]
        public string CancellationNote { get; set; }

        [EntityNameMap("msnfp_cancelledon")]
        public DateTime? CancelledOn { get; set; }

        [EntityNameMap("msnfp_endondate")]
        public DateTime? EndonDate { get; set; }

        [EntityNameMap("msnfp_lastpaymentdate")]
        public DateTime? LastPaymentDate { get; set; }

        [EntityOptionSetMap("msnfp_scheduletypecode")]
        public int? ScheduleTypeCode { get; set; }

        [EntityOptionSetMap("msnfp_Anonymity")]
        public int? Anonymity { get; set; }

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

        [EntityNameMap("msnfp_BookDate", Format = "yyyy-MM-dd")]
        public DateTime? BookDate { get; set; }

        [EntityOptionSetMap("msnfp_Ga_DeliveryCode")]
        public int? GaDeliveryCode { get; set; }

        [EntityNameMap("msnfp_DepositDate", Format = "yyyy-MM-dd")]
        public DateTime? DepositDate { get; set; }

        [EntityNameMap("msnfp_Emailaddress1")]
        public string EmailAddress1 { get; set; }

        [EntityNameMap("msnfp_Firstname")]
        public string FirstName { get; set; }

        [EntityNameMap("msnfp_LastName")]
        public string LastName { get; set; }

        [EntityNameMap("msnfp_TransactionDescription")]
        public string TransactionDescription { get; set; }

        [EntityOptionSetMap("msnfp_DataEntrySource")]
        public int? DataEntrySource { get; set; }

        [EntityOptionSetMap("msnfp_PaymentTypeCode")]
        public PaymentTypeCode? PaymentTypeCode { get; set; }

        [EntityNameMap("msnfp_MobilePhone")]
        public string MobilePhone { get; set; }

        [EntityNameMap("msnfp_OrganizationName")]
        public string OrganizationName { get; set; }

        [EntityOptionSetMap("msnfp_ReceiptPreferenceCode")]
        public int? ReceiptPreferenceCode { get; set; }

        [EntityNameMap("msnfp_Telephone1")]
        public string Telephone1 { get; set; }

        [EntityNameMap("msnfp_Telephone2")]
        public string Telephone2 { get; set; }

        [EntityNameMap("msnfp_name")]
        public string Name { get; set; }

        [EntityNameMap("msnfp_DataEntryReference")]
        public string DataEntryReference { get; set; }

        [EntityNameMap("msnfp_InvoiceIdentifier")]
        public string InvoiceIdentifier { get; set; }

        [EntityNameMap("msnfp_TransactionFraudCode")]
        public string TransactionFraudCode { get; set; }

        [EntityNameMap("msnfp_TransactionIdentifier")]
        public string TransactionIdentifier { get; set; }

        [EntityNameMap("msnfp_TransactionResult")]
        public string TransactionResult { get; set; }

        [EntityOptionSetMap("msnfp_TributeCode")]
        public int? TributeCode { get; set; }

        [EntityNameMap("msnfp_TributeAcknowledgement")]
        public string TributeAcknowledgement { get; set; }

        [EntityNameMap("msnfp_TributeMessage")]
        public string TributeMessage { get; set; }

        [EntityNameMap("msnfp_numberoffailures")]
        public int? NumberOfFailures { get; set; }

        [EntityNameMap("msnfp_numberofsuccesses")]
        public int? NumberOfSuccesses { get; set; }

        [EntityNameMap("msnfp_concurrentfailures")]
        public int? ConcurrentFailures { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_amountoffailures")]
        public decimal? AmountOfFailures { get; set; }

        [Column(TypeName = "money")]
        [EntityNameMap("msnfp_totalamount")]
        public decimal? TotalAmount { get; set; }

        public virtual TransactionCurrency TransactionCurrency { get; set; }

        public virtual PaymentProcessor PaymentProcessor { get; set; }

        public virtual Configuration Configuration { get; set; }

        public virtual Event Event { get; set; }

        public virtual EventPackage EventPackage { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual MembershipCategory MembershipCategory { get; set; }

        public virtual Membership Membership { get; set; }

        public virtual ICollection<Receipt> Receipt { get; set; }

        public virtual ICollection<Response> Response { get; set; }

        public virtual ICollection<Transaction> Transaction { get; set; }
    }

}
