using FundraisingandEngagement.Models.Attributes;
using System;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_BankRunSchedule")]
	public partial class BankRunSchedule : PaymentEntity, IIdentifierEntity
	{
		[EntityNameMap("msnfp_bankrunscheduleid")]
		public Guid BankRunScheduleId { get; set; }

		[EntityLogicalName("msnfp_PaymentSchedule")]
		[EntityReferenceMap("msnfp_PaymentScheduleId")]
		public Guid? PaymentScheduleId { get; set; }

		[EntityLogicalName("msnfp_BankRun")]
		[EntityReferenceMap("msnfp_BankRunId")]
		public Guid? BankRunId { get; set; }

		[EntityNameMap("msnfp_Identifier")]
		public string Identifier { get; set; }
	}
}
