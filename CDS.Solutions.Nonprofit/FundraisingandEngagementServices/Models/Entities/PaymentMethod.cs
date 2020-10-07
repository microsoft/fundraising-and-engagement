using System;
using System.Collections.Generic;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_paymentmethod")]
    public partial class PaymentMethod : ContactPaymentEntity, IIdentifierEntity
    {
        public PaymentMethod()
        {
            EventPackages = new HashSet<EventPackage>();
            PaymentSchedules = new HashSet<PaymentSchedule>();
            Transactions = new HashSet<Transaction>();
        }

        [EntityNameMap("msnfp_paymentmethodId")]
        public Guid PaymentMethodId { get; set; }

        [EntityReferenceMap("msnfp_PaymentProcessorId")]
        [EntityLogicalName("msnfp_PaymentProcessor")]
        public Guid? PaymentProcessorId { get; set; }

        [EntityNameMap("msnfp_TransactionFraudCode")]
        public string TransactionFraudCode { get; set; }

        public string TransactionIdentifier { get; set; }

        public string TransactionResult { get; set; }

        [EntityNameMap("msnfp_Emailaddress1")]
        public string Emailaddress1 { get; set; }

        [EntityNameMap("msnfp_Telephone1")]
        public string Telephone1 { get; set; }

        public string BillingStateorProvince { get; set; }

        [EntityNameMap("msnfp_Billing_Line1")]
        public string BillingLine1 { get; set; }

        [EntityNameMap("msnfp_Billing_Line2")]
        public string BillingLine2 { get; set; }

        [EntityNameMap("msnfp_Billing_Line3")]
        public string BillingLine3 { get; set; }

        [EntityNameMap("msnfp_Billing_City")]
        public string BillingCity { get; set; }

        [EntityNameMap("msnfp_Billing_Country")]
        public string BillingCountry { get; set; }

        [EntityNameMap("msnfp_Billing_Postalcode")]
        public string BillingPostalCode { get; set; }

        [EntityNameMap("msnfp_CcLast4")]
        public string CcLast4 { get; set; }

        [EntityNameMap("msnfp_expirydate")]
        public DateTime? CcExpDate { get; set; }

        [EntityNameMap("msnfp_ccexpmmyy")]
        public string CcExpMmYy { get; set; }

        [EntityOptionSetMap("msnfp_CcBrandCode")]
        public int? CcBrandCode { get; set; }



        [EntityNameMap("msnfp_BankName")]
        public string BankName { get; set; }

        [EntityNameMap("msnfp_BankActNumber")]
        public string BankActNumber { get; set; }

        [EntityNameMap("msnfp_BankActRtNumber")]
        public string BankActRtNumber { get; set; }

        [EntityOptionSetMap("msnfp_BankTypeCode")]
        public int? BankTypeCode { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

		[EntityNameMap("msnfp_isreusable")]
		public bool? IsReusable { get; set; }

		[EntityNameMap("msnfp_FirstName")]
        public string FirstName { get; set; }

        [EntityNameMap("msnfp_LastName")]
        public string LastName { get; set; }

        [EntityNameMap("msnfp_NameOnFile")]
        public string NameOnFile { get; set; }

        [EntityNameMap("msnfp_name")]
        public string Name { get; set; }

        [EntityNameMap("msnfp_StripeCustomerId")]
        public string StripeCustomerId { get; set; }

        // StripeCardId MonerisAuthToken IatsCustomerCode AdyenShopperReference WorldPayToken
        [EntityNameMap("msnfp_authtoken")]
        public string AuthToken { get; set; }

		[EntityNameMap("msnfp_abafinancialinstitutionname")]
		public string AbaFinancialInstitutionName { get; set; }

        [EntityOptionSetMap("msnfp_type")]
        public int? Type { get; set; }

        [EntityNameMap("msnfp_nameonbankaccount")]
		public string NameAsItAppearsOnTheAccount { get; set; }

		public virtual PaymentProcessor PaymentProcessor { get; set; }

        public virtual ICollection<EventPackage> EventPackages { get; set; }

        public virtual ICollection<PaymentSchedule> PaymentSchedules { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
