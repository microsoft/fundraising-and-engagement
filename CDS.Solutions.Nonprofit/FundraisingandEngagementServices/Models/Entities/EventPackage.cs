using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
    [EntityLogicalName("msnfp_EventPackage")]
    public partial class EventPackage : ContactPaymentEntity, IIdentifierEntity
    {
        public EventPackage()
        {
            PaymentSchedule = new HashSet<PaymentSchedule>();
            Product = new HashSet<Product>();
            Registration = new HashSet<Registration>();
            Sponsorship = new HashSet<Sponsorship>();
            Ticket = new HashSet<Ticket>();
        }

        [EntityNameMap("msnfp_EventPackageid")]
        public Guid EventPackageId { get; set; }

        [EntityReferenceMap("msnfp_CampaignId")]
        [EntityLogicalName("msnfp_Campaign")]
        public Guid? CampaignId { get; set; }

        [EntityReferenceMap("msnfp_PackageId")]
        [EntityLogicalName("msnfp_Package")]
        public Guid? PackageId { get; set; }

        [EntityReferenceMap("msnfp_Appealid")]
        [EntityLogicalName("msnfp_Appeal")]
        public Guid? Appealid { get; set; }

        [EntityReferenceMap("msnfp_EventId")]
        [EntityLogicalName("msnfp_Event")]
        public Guid? EventId { get; set; }

        [EntityReferenceMap("msnfp_ConfigurationId")]
        [EntityLogicalName("msnfp_Configuration")]
        public Guid? ConfigurationId { get; set; }

        [EntityReferenceMap("msnfp_ConstituentId")]
        [EntityLogicalName("contact")]
        public Guid? ConstituentId { get; set; }

        [EntityReferenceMap("msnfp_PaymentmethodId")]
        [EntityLogicalName("msnfp_Paymentmethod")]
        public Guid? PaymentmethodId { get; set; }

        [EntityReferenceMap("transactioncurrencyid")]
        [EntityLogicalName("transactioncurrency")]
        public Guid? TransactionCurrencyId { get; set; }

        [EntityNameMap("msnfp_Amount_Receipted")]
        [Column(TypeName = "money")]
        public decimal? AmountReceipted { get; set; }

        [EntityNameMap("msnfp_Amount_NonReceiptable")]
        [Column(TypeName = "money")]
        public decimal? AmountNonReceiptable { get; set; }

        [EntityNameMap("msnfp_Amount_Tax")]
        [Column(TypeName = "money")]
        public decimal? AmountTax { get; set; }

        [EntityNameMap("msnfp_Amount")]
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        [EntityNameMap("msnfp_Ref_Amount_Receipted")]
        [Column(TypeName = "money")]
        public decimal? RefAmountReceipted { get; set; }

        [EntityNameMap("msnfp_Ref_Amount_Nonreceiptable")]
        [Column(TypeName = "money")]
        public decimal? RefAmountNonreceiptable { get; set; }

        [EntityNameMap("msnfp_Ref_Amount_Tax")]
        [Column(TypeName = "money")]
        public decimal? RefAmountTax { get; set; }

        [EntityNameMap("msnfp_Ref_Amount")]
        [Column(TypeName = "money")]
        public decimal? RefAmount { get; set; }

        [EntityNameMap("msnfp_FirstName")]
        public string FirstName { get; set; }

        [EntityNameMap("msnfp_LastName")]
        public string LastName { get; set; }

        [EntityNameMap("msnfp_Emailaddress1")]
        public string Emailaddress1 { get; set; }

        [EntityNameMap("msnfp_Telephone1")]
        public string Telephone1 { get; set; }

        [EntityNameMap("msnfp_Telephone2")]
        public string Telephone2 { get; set; }

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

        [EntityNameMap("msnfp_ChequeNumber")]
        public string ChequeNumber { get; set; }

        [EntityNameMap("msnfp_ChequeWireDate", Format = "yyyy-MM-dd")]
        public DateTime? ChequeWireDate { get; set; }



        [EntityNameMap("msnfp_Date", Format = "yyyy-MM-dd")]
        public DateTime? Date { get; set; }

        [EntityNameMap("msnfp_DateRefunded", Format = "yyyy-MM-dd")]
        public DateTime? DateRefunded { get; set; }

        [EntityOptionSetMap("msnfp_DataEntrySource")]
        public int? DataEntrySource { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        [EntityOptionSetMap("msnfp_CcBrandCode")]
        public int? CcBrandCode { get; set; }

        [EntityNameMap("msnfp_OrganizationName")]
        public string OrganizationName { get; set; }

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

        [EntityNameMap("msnfp_ThirdPartyReceipt")]
        public string ThirdPartyReceipt { get; set; }

        [EntityOptionSetMap("msnfp_Sum_Donations")]
        public int? SumDonations { get; set; }

        [EntityOptionSetMap("msnfp_Sum_Products")]
        public int? SumProducts { get; set; }

        [EntityOptionSetMap("msnfp_Sum_Sponsorships")]
        public int? SumSponsorships { get; set; }

        [EntityOptionSetMap("msnfp_Sum_Tickets")]
        public int? SumTickets { get; set; }

        [EntityOptionSetMap("msnfp_Sum_Registrations")]
        public int? SumRegistrations { get; set; }

        [EntityNameMap("msnfp_Val_Donations")]
        [Column(TypeName = "money")]
        public decimal? ValDonations { get; set; }

        [EntityNameMap("msnfp_Val_Products")]
        [Column(TypeName = "money")]
        public decimal? ValProducts { get; set; }

        [EntityNameMap("msnfp_Val_Sponsorships")]
        [Column(TypeName = "money")]
        public decimal? ValSponsorships { get; set; }

        [EntityNameMap("msnfp_Val_Tickets")]
        [Column(TypeName = "money")]
        public decimal? ValTickets { get; set; }

        public virtual TransactionCurrency TransactionCurrency { get; set; }

        public virtual Configuration Configuration { get; set; }

        public virtual Event Event { get; set; }

        public virtual PaymentMethod Paymentmethod { get; set; }

        public virtual ICollection<PaymentSchedule> PaymentSchedule { get; set; }

        public virtual ICollection<Product> Product { get; set; }

        public virtual ICollection<Registration> Registration { get; set; }

        public virtual ICollection<Sponsorship> Sponsorship { get; set; }

        public virtual ICollection<Ticket> Ticket { get; set; }

        public virtual ICollection<Account> Account { get; set; }

        public virtual ICollection<Contact> Contact { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
