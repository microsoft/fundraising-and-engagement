using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	public abstract class ContactPaymentEntity : PaymentEntity
    {
		[EntityReferenceMap("msnfp_CustomerId")]
        public Guid? CustomerId { get; set; }

        public int? CustomerIdType { get; set; }
    }
}
