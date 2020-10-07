using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_MembershipOrder")]
    public partial class MembershipOrder : PaymentEntity, IIdentifierEntity
    {
        [EntityNameMap("msnfp_MembershipOrder")]
        public Guid MembershipOrderId { get; set; }

        [EntityReferenceMap("msnfp_FromMembershipId")]
        [EntityLogicalName("msnfp_FromMembership")]
        public Guid? FromMembershipCategoryId { get; set; }

        [EntityReferenceMap("msnfp_ToMembershipGroupId")]
        [EntityLogicalName("msnfp_ToMembershipGroup")]
        public Guid? ToMembershipGroupId { get; set; }

        [EntityOptionSetMap("msnfp_Order")]
        public int? Order { get; set; }

        [EntityNameMap("msnfp_OrderDate")]
        public DateTime? OrderDate { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        public virtual MembershipCategory FromMembershipCategory { get; set; }

        public virtual MembershipGroup ToMembershipGroup { get; set; }
    }
}
