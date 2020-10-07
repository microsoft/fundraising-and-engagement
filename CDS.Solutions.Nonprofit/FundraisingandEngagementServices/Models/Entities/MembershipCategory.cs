using FundraisingandEngagement.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FundraisingandEngagement.Models.Entities
{
    [EntityLogicalName("msnfp_MembershipCategory")]
    public partial class MembershipCategory : PaymentEntity
    {
        [EntityLogicalName("msnfp_MembershipCategory")]
        public MembershipCategory()
        {
            Membership = new HashSet<Membership>();
        }

        [EntityNameMap("msnfp_MembershipCategoryId")]
        public Guid MembershipCategoryId { get; set; }

        [EntityReferenceMap("TransactionCurrencyId")]
        [EntityLogicalName("TransactionCurrency")]
        public Guid? TransactionCurrencyId { get; set; }


        [EntityNameMap("msnfp_Amount_Membership")]
        [Column(TypeName = "money")]
        public decimal? AmountMembership { get; set; }

        [EntityNameMap("msnfp_Amount_Tax")]
        [Column(TypeName = "money")]
        public decimal? AmountTax { get; set; }

        [EntityNameMap("msnfp_Amount")]
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        [EntityNameMap("msnfp_goodwilldate")]
        public DateTime? GoodWillDate { get; set; }

        [EntityOptionSetMap("msnfp_membershipduration")]
        public int? MembershipDuration { get; set; }

        [EntityNameMap("msnfp_renewaldate")]
        public DateTime? RenewalDate { get; set; }

        [EntityNameMap("msnfp_name")]
        public string Name { get; set; }

        public virtual TransactionCurrency TransactionCurrency { get; set; }

		public virtual ICollection<Membership> Membership { get; set; }

        public virtual ICollection<MembershipOrder> MembershipOrder { get; set; }

		public virtual ICollection<Transaction> Transactions { get; set; }

		public virtual ICollection<PaymentSchedule> PaymentSchedules { get; set; }
	}
}
