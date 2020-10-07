using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_EventTicket")]
    public partial class EventTicket : PaymentEntity, IIdentifierEntity
    {
        public EventTicket()
        {
            Registration = new HashSet<Registration>();
        }

        [EntityNameMap("msnfp_EvenTicketid")]
        public Guid EvenTicketId { get; set; }

        [EntityReferenceMap("msnfp_EventId")]
        [EntityLogicalName("msnfp_Event")]
        public Guid? EventId { get; set; }

        [EntityReferenceMap("transactioncurrencyid")]
        [EntityLogicalName("transactioncurrency")]
        public Guid? TransactionCurrencyId { get; set; }

		[EntityNameMap("msnfp_Amount")]
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        [EntityNameMap("msnfp_Amount_Receipted")]
        [Column(TypeName = "money")]
        public decimal? AmountReceipted { get; set; }

        [EntityNameMap("msnfp_Amount_NonReceiptable")]
        [Column(TypeName = "money")]
        public decimal? AmountNonReceiptable { get; set; }

        [EntityNameMap("msnfp_Amount_Tax")]
        [Column(TypeName = "money")]
        public decimal? AmountTax { get; set; }

        [EntityNameMap("msnfp_Description")]
        public string Description { get; set; }

        [EntityOptionSetMap("msnfp_MaxSpots")]
        public int? MaxSpots { get; set; }

        [EntityNameMap("msnfp_Name")]
        public string Name { get; set; }

        [EntityOptionSetMap("msnfp_RegistrationsPerTicket")]
        public int? RegistrationsPerTicket { get; set; }

        [EntityOptionSetMap("msnfp_Sum_RegistrationsAvailable")]
        public int? SumRegistrationsAvailable { get; set; }

        [EntityOptionSetMap("msnfp_Sum_RegistrationSold")]
        public int? SumRegistrationSold { get; set; }

        [EntityOptionSetMap("msnfp_Sum_Available")]
        public int? SumAvailable { get; set; }

        [EntityOptionSetMap("msnfp_Sum_Sold")]
        public int? SumSold { get; set; }

        [EntityOptionSetMap("msnfp_Tickets")]
        public int? Tickets { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        [EntityNameMap("msnfp_Val_Tickets")]
        [Column(TypeName = "money")]
        public decimal? ValTickets { get; set; }

        public virtual TransactionCurrency TransactionCurrency { get; set; }

        public virtual Event Event { get; set; }

        public virtual ICollection<Registration> Registration { get; set; }
    }
}
