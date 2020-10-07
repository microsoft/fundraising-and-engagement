using System;
using System.Collections.Generic;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_registration")]
    public partial class Registration : ContactPaymentEntity, IIdentifierEntity
    {
        public Registration()
        {
            Response = new HashSet<Response>();
        }

        [EntityNameMap("msnfp_registrationid")]
        public Guid RegistrationId { get; set; }


        [EntityReferenceMap("msnfp_EventId")]
        [EntityLogicalName("msnfp_Event")]
        public Guid? EventId { get; set; }

        [EntityReferenceMap("msnfp_EventPackageId")]
        [EntityLogicalName("msnfp_EventPackage")]
        public Guid? EventPackageId { get; set; }

        [EntityReferenceMap("msnfp_TicketId")]
        [EntityLogicalName("msnfp_Ticket")]
        public Guid? TicketId { get; set; }

        [EntityReferenceMap("msnfp_EventTicketId")]
        [EntityLogicalName("msnfp_EventTicket")]
        public Guid? EventTicketId { get; set; }

        [EntityReferenceMap("TransactionCurrencyId")]
        [EntityLogicalName("TransactionCurrency")]
        public Guid? TransactionCurrencyId { get; set; }

        [EntityNameMap("msnfp_date")]
        public DateTime? Date { get; set; }

        public string Name { get; set; }

        [EntityNameMap("msnfp_GroupNotes")]
        public string GroupNotes { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        [EntityNameMap("msnfp_Firstname")]
        public string FirstName { get; set; }

        [EntityNameMap("msnfp_LastName")]
        public string LastName { get; set; }

        [EntityNameMap("msnfp_Emailaddress1")]
        public string Emailaddress1 { get; set; }

        [EntityNameMap("msnfp_Telephone1")]
        public string Telephone1 { get; set; }

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

        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Address_Line1 { get; set; }
        public string Address_Line2 { get; set; }
        public string Address_City { get; set; }
        public string Address_Province { get; set; }
        public string Address_PostalCode { get; set; }
        public string Address_Country { get; set; }
        public string Team { get; set; }

        [EntityReferenceMap("msnfp_EventTableId")]
        [EntityLogicalName("msnfp_EventTable")]
        public Guid? TableId { get; set; }

        public virtual TransactionCurrency TransactionCurrency { get; set; }
        public virtual Event Event { get; set; }
        public virtual EventPackage EventPackage { get; set; }
        public virtual EventTicket EventTicket { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual ICollection<Response> Response { get; set; }
    }
}
