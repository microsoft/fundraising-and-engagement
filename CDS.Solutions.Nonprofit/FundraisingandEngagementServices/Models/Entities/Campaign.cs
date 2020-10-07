using FundraisingandEngagement.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("campaign")]
	[NotMapped]
	public class Campaign : PaymentEntity
	{
		[EntityNameMap("CampaignId")]
		public Guid CampaignId { get; set; }

		[EntityNameMap("msnfp_count_products")]
		public int? ProductCount { get; set; }

		[EntityNameMap("msnfp_sum_products")]
		public decimal? ProductAmount { get; set; }

		[EntityNameMap("msnfp_sum_registrations")]
		public decimal? RegistrationAmount { get; set; }

		[EntityNameMap("msnfp_count_registrations")]
		public int? RegistrationCount { get; set; }

		[EntityNameMap("msnfp_sum_sponsorships")]
		public decimal? SponsorshipAmount { get; set; }

		[EntityNameMap("msnfp_count_sponsorships")]
		public int? SponsorshipCount { get; set; }

		[EntityNameMap("msnfp_count_transactions")]
		public int? DonationCount { get; set; }

		[EntityNameMap("msnfp_sum_transactions")]
		public decimal? DonationAmount { get; set; }
		
		[EntityNameMap("msnfp_sum_total")]
		public decimal? TotalAmount { get; set; }

		[EntityNameMap("msnfp_count_donorcommitments")]
		public int? DonorCommitmentCount { get; set; }

		[EntityNameMap("msnfp_sum_donorcommitments")]
		public decimal? DonorCommitmentAmount { get; set; }
	}
}
