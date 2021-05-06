using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
    [EntityLogicalName("msnfp_Ticket")]
    public partial class Ticket : PaymentEntity, IContactPaymentEntity, IIdentifierEntity
    {
        public Ticket()
        {
            Registration = new HashSet<Registration>();
        }

        [EntityNameMap("msnfp_Ticketid", PushToDataverse = true)]
        public Guid TicketId { get; set; }

        [EntityReferenceMap("msnfp_CustomerId")]
        public Guid? CustomerId { get; set; }

        public int? CustomerIdType { get; set; }

        [EntityReferenceMap("msnfp_EventId")]
        [EntityLogicalName("msnfp_Event")]
        public Guid? EventId { get; set; }

        [EntityReferenceMap("msnfp_EventPackageId")]
        [EntityLogicalName("msnfp_EventPackage")]
        public Guid? EventPackageId { get; set; }

        [EntityReferenceMap("msnfp_EventTicketId")]
        [EntityLogicalName("msnfp_EventTicket")]
        public Guid? EventTicketId { get; set; }

        [EntityReferenceMap("transactioncurrencyid")]
        [EntityLogicalName("transactioncurrency")]
        public Guid? TransactionCurrencyId { get; set; }

        [EntityNameMap("msnfp_Amount_Receipted", PushToDataverse = true)]
        [Column(TypeName = "money")]
        public decimal? AmountReceipted { get; set; }

        [EntityNameMap("msnfp_Amount_Nonreceiptable", PushToDataverse = true)]
        [Column(TypeName = "money")]
        public decimal? AmountNonreceiptable { get; set; }

        [EntityNameMap("msnfp_Amount_Tax")]
        [Column(TypeName = "money")]
        public decimal? AmountTax { get; set; }

        [EntityNameMap("msnfp_Amount")]
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        [EntityNameMap("msnfp_GroupNotes")]
        public string GroupNotes { get; set; }

        [EntityNameMap("msnfp_Date", Format = "yyyy-MM-dd")]
        public DateTime? Date { get; set; }

        [EntityNameMap("msnfp_name")]
        public string Name { get; set; }

        [EntityOptionSetMap("msnfp_RegistrationsPerTicket")]
        public int? RegistrationsPerTicket { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        public virtual TransactionCurrency TransactionCurrency { get; set; }

        public virtual Event Event { get; set; }

        public virtual EventPackage EventPackage { get; set; }

        public virtual ICollection<Registration> Registration { get; set; }
    }

    public enum TicketStateCode
    {
        Active = 0,
        Inactive = 1
    }
}