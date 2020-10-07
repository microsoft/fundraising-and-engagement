using FundraisingandEngagement.Models.Attributes;
using System;
using System.Collections.Generic;

namespace FundraisingandEngagement.Models.Entities
{
    [EntityLogicalName("msnfp_Membership")]
    public partial class Membership : PaymentEntity
    {
        [EntityNameMap("msnfp_MembershipId")]
        public Guid MembershipId { get; set; }

        [EntityReferenceMap("msnfp_MembershipCategoryId")]
        [EntityLogicalName("msnfp_MembershipCategory")]
        public Guid? MembershipCategoryId { get; set; }

        [EntityNameMap("msnfp_startdate", Format = "yyyy-MM-dd")]
        public DateTime? StartDate { get; set; }

        [EntityNameMap("msnfp_enddate", Format = "yyyy-MM-dd")]
        public DateTime? EndDate { get; set; }

        [EntityNameMap("msnfp_Primary")]
        public bool? Primary { get; set; }

        [EntityNameMap("msnfp_name")]
        public string Name { get; set; }

        public virtual MembershipCategory MembershipCategory { get; set; }

		public virtual ICollection<Transaction> Transactions { get; set; }
        
        public virtual ICollection<Account> Account { get; set; }

        public virtual ICollection<Contact> Contact { get; set; }

		public virtual ICollection<PaymentSchedule> PaymentSchedules { get; set; }
	}
}
