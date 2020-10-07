using System;
using System.Collections.Generic;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_MembershipGroup")]
    public partial class MembershipGroup : PaymentEntity, IIdentifierEntity
    {
        public MembershipGroup()
        {
            MembershipOrders = new HashSet<MembershipOrder>();
        }

        [EntityNameMap("msnfp_membershipgroup")]
        public Guid MembershipGroupId { get; set; }

        [EntityNameMap("msnfp_GroupName")]
        public string GroupName { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        public virtual ICollection<MembershipOrder> MembershipOrders { get; set; }
    }
}
