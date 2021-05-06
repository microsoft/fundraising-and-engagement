using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
    [EntityLogicalName("msnfp_BankRunSchedule")]
    public partial class BankRunSchedule : PaymentEntity, IIdentifierEntity
    {
        [EntityNameMap("msnfp_bankrunscheduleid", PushToDataverse = true)]
        public Guid BankRunScheduleId { get; set; }

        [EntityLogicalName("msnfp_PaymentSchedule")]
        [EntityReferenceMap("msnfp_PaymentScheduleId", PushToDataverse = true)]
        public Guid? PaymentScheduleId { get; set; }

        [EntityLogicalName("msnfp_BankRun")]
        [EntityReferenceMap("msnfp_BankRunId", PushToDataverse = true)]
        public Guid? BankRunId { get; set; }

        [EntityNameMap("msnfp_Identifier", PushToDataverse = true)]
        public string Identifier { get; set; }
    }
}