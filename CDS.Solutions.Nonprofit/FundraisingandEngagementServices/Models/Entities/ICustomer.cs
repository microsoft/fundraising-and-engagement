using System;
using FundraisingandEngagement.Models.Enums;

namespace FundraisingandEngagement.Models.Entities
{
	public interface ICustomer
	{
		Guid CustomerId { get; }
		DonorType? msnfp_year0_category { get; set; }
		decimal? msnfp_year0_giving { get; set; }
		DonorType? msnfp_year1_category { get; set; }
		decimal? msnfp_year1_giving { get; set; }
		DonorType? msnfp_year2_category { get; set; }
		decimal? msnfp_year2_giving { get; set; }
		DonorType? msnfp_year3_category { get; set; }
		decimal? msnfp_year3_giving { get; set; }
		DonorType? msnfp_year4_category { get; set; }
		decimal? msnfp_year4_giving { get; set; }
		decimal? msnfp_lifetimegivingsum { get; set; }
		DateTime? SyncDate { get; set; }
	}
}
