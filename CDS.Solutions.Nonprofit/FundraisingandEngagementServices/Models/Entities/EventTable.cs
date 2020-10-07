using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_EventTable")]
    public partial class EventTable : PaymentEntity, IIdentifierEntity
    {  
       public Guid EventTableId { get; set; }

        public string Identifier { get; set; }

        public string TableCapacity { get; set; }

        public string TableNumber { get; set; }

        public Guid EventId { get; set; }

        public Guid EventTicketId { get; set; }
    }
}
