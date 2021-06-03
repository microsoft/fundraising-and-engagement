using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
    [EntityLogicalName("msnfp_response")]
    public partial class Response : PaymentEntity, IIdentifierEntity
    {
        [EntityNameMap("msnfp_responseid", PushToDataverse = true)]
        public Guid ResponseId { get; set; }

        [EntityReferenceMap("msnfp_TransactionId", PushToDataverse = true)]
        [EntityLogicalName("msnfp_Transaction")]
        public Guid? TransactionId { get; set; }

        public Guid? PaymentScheduleId { get; set; }

        [EntityNameMap("msnfp_response", PushToDataverse = true)]
        public string Result { get; set; }

        [EntityNameMap("msnfp_Identifier", PushToDataverse = true)]
        public string Identifier { get; set; }

        public virtual PaymentSchedule PaymentSchedule { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}