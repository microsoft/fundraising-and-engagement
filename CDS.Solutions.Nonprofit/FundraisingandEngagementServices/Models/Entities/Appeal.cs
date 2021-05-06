using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
    [EntityLogicalName("msnfp_Appeal")]
    [NotMapped]
    public class Appeal : PaymentEntity
    {
        [EntityNameMap("msnfp_AppealId")]
        public Guid AppealId { get; set; }

        [EntityNameMap("msnfp_count_transactions", PushToDataverse = true)]
        public int? DonationCount { get; set; }

        [EntityNameMap("msnfp_sum_transactions", PushToDataverse = true)]
        public decimal? DonationAmount { get; set; }

        [EntityNameMap("msnfp_count_sponsorships", PushToDataverse = true)]
        public int? SponsorshipCount { get; set; }

        [EntityNameMap("msnfp_sum_sponsorships", PushToDataverse = true)]
        public decimal? SponsorshipAmount { get; set; }

        [EntityNameMap("msnfp_count_products", PushToDataverse = true)]
        public int? ProductCount { get; set; }

        [EntityNameMap("msnfp_sum_products", PushToDataverse = true)]
        public decimal? ProductAmount { get; set; }

        [EntityNameMap("msnfp_count_registrations", PushToDataverse = true)]
        public int? RegistrationCount { get; set; }

        [EntityNameMap("msnfp_sum_registrations", PushToDataverse = true)]
        public decimal? RegistrationAmount { get; set; }

        [EntityNameMap("msnfp_sum_total", PushToDataverse = true)]
        public decimal? TotalAmount { get; set; }

        [EntityNameMap("msnfp_count_donorcommitments", PushToDataverse = true)]
        public int? DonorCommitmentCount { get; set; }

        [EntityNameMap("msnfp_sum_donorcommitments", PushToDataverse = true)]
        public decimal? DonorCommitmentAmount { get; set; }
    }
}