using System;
using System.Collections.Generic;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_BankRun")]
    public partial class BankRun : PaymentEntity, IIdentifierEntity
    {
		[EntityNameMap("msnfp_bankrunid")]
        public Guid BankRunId { get; set; }

		[EntityLogicalName("msnfp_PaymentProcessor")]
		[EntityReferenceMap("msnfp_PaymentProcessorId")]
		public Guid? PaymentProcessorId { get; set; }

		[EntityLogicalName("msnfp_PaymentMethod")]
		[EntityReferenceMap("msnfp_AccountToCreditId")]
		public Guid? AccountToCreditId { get; set; }

		[EntityOptionSetMap("msnfp_bankrunstatus")]
        public int? BankRunStatus { get; set; }

		[EntityNameMap("msnfp_startdate", Format = "yyyy-MM-dd")]
        public DateTime? StartDate { get; set; }

        [EntityNameMap("msnfp_enddate", Format = "yyyy-MM-dd")]
        public DateTime? EndDate { get; set; }

        [EntityNameMap("msnfp_datetobeprocessed", Format = "yyyy-MM-dd")]
        public DateTime? DateToBeProcessed { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

		[EntityNameMap("msnfp_filecreationnumber")]
		public int? FileCreationNumber { get; set; }

		public virtual ICollection<Note> Note { get; set; }
	}
}
