using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_Event")]
    public partial class Event : PaymentEntity, IIdentifierEntity
    {
        public Event()
        {
            EventDislaimer = new HashSet<EventDisclaimer>();
            EventDonation = new HashSet<EventDonation>();
            EventPackage = new HashSet<EventPackage>();
            EventProduct = new HashSet<EventProduct>();
            EventSponsor = new HashSet<EventSponsor>();
            EventSponsorship = new HashSet<EventSponsorship>();
            EventTicket = new HashSet<EventTicket>();
            PaymentSchedule = new HashSet<PaymentSchedule>();
            Product = new HashSet<Product>();
            Registration = new HashSet<Registration>();
            SponsorshipNavigation = new HashSet<Sponsorship>();
            Ticket = new HashSet<Ticket>();
            Transaction = new HashSet<Transaction>();
        }

        [EntityNameMap("msnfp_Eventid")]
        public Guid EventId { get; set; }


        [EntityLogicalName("msnfp_PaymentProcessor")]
        [EntityNameMap("msnfp_PaymentProcessorId")]
        public Guid? PaymentProcessorId { get; set; }

        [EntityReferenceMap("msnfp_CampaignId")]
        [EntityLogicalName("msnfp_Campaign")]
        public Guid? CampaignId { get; set; }

        [EntityReferenceMap("msnfp_AppealId")]
        [EntityLogicalName("msnfp_Appeal")]
        public Guid? AppealId { get; set; }

        [EntityReferenceMap("msnfp_PackageId")]
        [EntityLogicalName("msnfp_Package")]
        public Guid? PackageId { get; set; }

        [EntityReferenceMap("msnfp_DesignationId")]
        [EntityLogicalName("msnfp_Designation")]
        public Guid? DesignationId { get; set; }

        [EntityReferenceMap("msnfp_ConfigurationId")]
        [EntityLogicalName("msnfp_Configuration")]
        public Guid? ConfigurationId { get; set; }

        [EntityReferenceMap("msnfp_VenueId")]
        [EntityLogicalName("msnfp_Venue")]
        public Guid? VenueId { get; set; }

        [EntityReferenceMap("msnfp_TeamOwnerId")]
        [EntityLogicalName("msnfp_TeamOwner")]
        public Guid? TeamOwnerId { get; set; }

        [EntityReferenceMap("msnfp_TermsOfReferenceId")]
        [EntityLogicalName("msnfp_TermsOfReferenceId")]
        public Guid? TermsOfReferenceId { get; set; }

        [EntityReferenceMap("TransactionCurrencyId")]
        [EntityLogicalName("TransactionCurrency")]
        public Guid? TransactionCurrencyId { get; set; }


        [EntityNameMap("msnfp_Goal")]
        [Column(TypeName = "money")]
        public decimal? Goal { get; set; }

        [EntityOptionSetMap("msnfp_Capacity")]
        public int? Capacity { get; set; }

        [EntityNameMap("msnfp_Coordinator")]
        public string Coordinator { get; set; }

        [EntityNameMap("msnfp_TimeAndDate")]
        public string TimeAndDate { get; set; }

        [EntityNameMap("msnfp_Description")]
        public string Description { get; set; }

        [EntityNameMap("msnfp_Location")]
        public string Location { get; set; }

        [EntityNameMap("msnfp_Sponsorship")]
        public string Sponsorship { get; set; }

        [EntityNameMap("msnfp_Name")]
        public string Name { get; set; }

        [EntityNameMap("msnfp_Amount")]
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        [EntityOptionSetMap("msnfp_EventTypeCode")]
        public int? EventTypeCode { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        [EntityNameMap("msnfp_Map_Line1")]
        public string MapLine1 { get; set; }

        [EntityNameMap("msnfp_Map_Line2")]
        public string MapLine2 { get; set; }

        [EntityNameMap("msnfp_Map_Line3")]
        public string MapLine3 { get; set; }

        [EntityNameMap("msnfp_Map_City")]
        public string MapCity { get; set; }

        [EntityNameMap("msnfp_Map_StateOrProvince")]
        public string MapStateOrProvince { get; set; }

        [EntityNameMap("msnfp_Map_Postalcode")]
        public string MapPostalCode { get; set; }

        [EntityNameMap("msnfp_Map_Country")]
        public string MapCountry { get; set; }

        [EntityNameMap("msnfp_ProposedEnd")]
        public DateTime? ProposedEnd { get; set; }

        [EntityNameMap("msnfp_ProposedStart")]
        public DateTime? ProposedStart { get; set; }

        [EntityNameMap("msnfp_ShowMap")]
        public bool? ShowMap { get; set; }

        [EntityNameMap("msnfp_FreeEvent")]
        public bool? FreeEvent { get; set; }

        [EntityNameMap("msnfp_LargeImage")]
        public string LargeImage { get; set; }

        [EntityNameMap("msnfp_SmallImage")]
        public string SmallImage { get; set; }

        [EntityNameMap("msnfp_CostAmount")]
        [Column(TypeName = "money")]
        public decimal? CostAmount { get; set; }

        [EntityOptionSetMap("msnfp_CostPercentage")]
        public int? CostPercentage { get; set; }

        [EntityNameMap("msnfp_ExternalUrl")]
        public string ExternalUrl { get; set; }

        [EntityNameMap("msnfp_ForceRedirect")]
        public bool? ForceRedirect { get; set; }

        [EntityOptionSetMap("msnfp_ForceRedirectTiming")]
        public int? ForceRedirectTiming { get; set; }

        [EntityNameMap("msnfp_HomePageUrl")]
        public string HomePageUrl { get; set; }

        [EntityNameMap("msnfp_InvoiceMessage")]
        public string InvoiceMessage { get; set; }

        [EntityNameMap("msnfp_LabelLanguageCode")]
        public string LabelLanguageCode { get; set; }

        [EntityNameMap("msnfp_LastPublished")]
        public DateTime? LastPublished { get; set; }

        [EntityNameMap("msnfp_MadeVisible")]
        public DateTime? MadeVisible { get; set; }

        [EntityNameMap("msnfp_PaymentNotice")]
        public string PaymentNotice { get; set; }

        [EntityNameMap("msnfp_Removed")]
        public bool? Removed { get; set; }

        [EntityNameMap("msnfp_SelectCurrency")]
        public bool? SelectCurrency { get; set; }

        [EntityNameMap("msnfp_SetAcceptNotice")]
        public bool? SetAcceptNotice { get; set; }

        [EntityNameMap("msnfp_SetCoverCosts")]
        public bool? SetCoverCosts { get; set; }

        [EntityNameMap("msnfp_SetSignUp")]
        public bool? SetSignUp { get; set; }

        [EntityNameMap("msnfp_ShowApple")]
        public bool? ShowApple { get; set; }

        [EntityNameMap("msnfp_ShowCompany")]
        public bool? ShowCompany { get; set; }

        [EntityNameMap("msnfp_ShowCoverCosts")]
        public bool? ShowCoverCosts { get; set; }

        [EntityNameMap("msnfp_ShowGoogle")]
        public bool? ShowGoogle { get; set; }

        [EntityNameMap("msnfp_ShowInvoice")]
        public bool? ShowInvoice { get; set; }

        [EntityNameMap("msnfp_ShowPayPal")]
        public bool? ShowPayPal { get; set; }

        [EntityNameMap("msnfp_ShowCreditCard")]
        public bool? ShowCreditCard { get; set; }

        [EntityNameMap("msnfp_ThankYou")]
        public string ThankYou { get; set; }

        [EntityNameMap("msnfp_Visible")]
        public bool? Visible { get; set; }

        [EntityNameMap("msnfp_ShowGiftAid")]
        public bool? ShowGiftAid { get; set; }

        [EntityNameMap("msnfp_RemovedOn")]
        public DateTime? RemovedOn { get; set; }


        public virtual TransactionCurrency TransactionCurrency { get; set; }
        public virtual PaymentProcessor PaymentProcessor { get; set; }
        public virtual Configuration Configuration { get; set; }
        public virtual TermsOfReference TermsOfReference { get; set; }
        public virtual ICollection<EventDisclaimer> EventDislaimer { get; set; }
        public virtual ICollection<EventDonation> EventDonation { get; set; }
        public virtual ICollection<EventPackage> EventPackage { get; set; }
        public virtual ICollection<EventProduct> EventProduct { get; set; }
        public virtual ICollection<EventSponsor> EventSponsor { get; set; }
        public virtual ICollection<EventSponsorship> EventSponsorship { get; set; }
        public virtual ICollection<EventTicket> EventTicket { get; set; }
        public virtual ICollection<PaymentSchedule> PaymentSchedule { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<Registration> Registration { get; set; }
        public virtual ICollection<Sponsorship> SponsorshipNavigation { get; set; }
        public virtual ICollection<Ticket> Ticket { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
        public virtual Designation Designation { get; set; }
    }
}
