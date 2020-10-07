using System;
using System.Collections.Generic;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_eventsponsor")]
    public partial class EventSponsor : PaymentEntity, IIdentifierEntity
    {
        public EventSponsor()
        {
            Sponsorship = new HashSet<Sponsorship>();
        }

        [EntityNameMap("msnfp_eventsponsorid")]
        public Guid EventSponsorId { get; set; }


        [EntityReferenceMap("msnfp_eventpageid")]
        [EntityLogicalName("msnfp_eventpage")]
        public Guid? EventId { get; set; }

        [EntityReferenceMap("transactioncurrencyid")]
        [EntityLogicalName("transactioncurrency")]
        public Guid? TransactionCurrencyId { get; set; }

		[EntityNameMap("msnfp_largeimage")]
        public string LargeImage { get; set; }

        [EntityOptionSetMap("msnfp_order")]
        public int? Order { get; set; }

        [EntityNameMap("msnfp_orderdate")]
        public DateTime? OrderDate { get; set; }

        [EntityNameMap("msnfp_sponsortitle")]
        public string SponsorTitle { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        public virtual TransactionCurrency TransactionCurrency { get; set; }

        public virtual Event Event { get; set; }

        public virtual ICollection<Sponsorship> Sponsorship { get; set; }
    }
}
