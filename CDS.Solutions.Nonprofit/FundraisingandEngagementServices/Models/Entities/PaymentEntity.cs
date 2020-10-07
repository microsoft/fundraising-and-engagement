using System;
using FundraisingandEngagement.Models.Attributes;
using FundraisingandEngagement.Models.Enums;

namespace FundraisingandEngagement.Models.Entities
{
	public abstract class PaymentEntity<TStatusCode> : IPaymentEntity where TStatusCode : struct
	{
        [EntityNameMap("SyncDate")]
        public DateTime? SyncDate { get; set; }

        [EntityNameMap("CreatedOn")]
        public DateTime? CreatedOn { get; set; }

        public bool? Deleted { get; set; }

        public DateTime? DeletedDate { get; set; }

		[EntityOptionSetMap("StatusCode")]
		public TStatusCode? StatusCode { get; set; }

		[EntityOptionSetMap("StateCode")]
		public int? StateCode { get; set; }

		public void Delete()
		{
			Deleted = true;
			DeletedDate = DateTime.Now;
			SyncDate = null;
		}
	}

	public abstract class PaymentEntity : PaymentEntity<StatusCode>
	{
	}
}
