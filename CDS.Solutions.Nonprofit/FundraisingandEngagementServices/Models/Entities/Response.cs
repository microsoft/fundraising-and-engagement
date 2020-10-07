using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
    [EntityLogicalName("msnfp_response")]
    public partial class Response : PaymentEntity, IIdentifierEntity
    {
        [EntityNameMap("msnfp_responseid")]
        public Guid ResponseId { get; set; }

        [EntityReferenceMap("msnfp_TransactionId")]
        [EntityLogicalName("msnfp_Transaction")]
        public Guid? TransactionId { get; set; }

        public Guid? PaymentScheduleId { get; set; }

        [EntityReferenceMap("msnfp_RegistrationPackageId")]
        [EntityLogicalName("msnfp_RegistrationPackage")]
        public Guid? RegistrationPackageId { get; set; }

        [EntityNameMap("msnfp_response")]
        public string Result { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        public virtual PaymentSchedule PaymentSchedule { get; set; }

        public virtual Registration RegistrationPackage { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
