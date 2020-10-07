using System;
using System.ComponentModel.DataAnnotations;

namespace FundraisingandEngagement.Models.Entities
{
	public class SyncLog
    {
		[Key]
        public Guid SyncExceptionId { get; set; }

        public Guid? PaymentEntityPK { get; set; }

        public string EntityType { get; set; }

        public string ExceptionMessage { get; set; }

        public string StackTrace { get; set; }

		public Guid? TransactionId { get; set; }

		public DateTime? CreatedOn { get; set; }

		public virtual Transaction Transaction { get; set; }
	}
}
