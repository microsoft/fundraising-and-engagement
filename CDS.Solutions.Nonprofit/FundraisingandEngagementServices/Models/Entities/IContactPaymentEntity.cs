using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
    public interface IContactPaymentEntity
    {
        public Guid? CustomerId { get; set; }

        public int? CustomerIdType { get; set; }
    }
}