using System;
using System.Collections.Generic;
using System.Text;
using FundraisingandEngagement.Models.Attributes;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_donorcommitment")]
	public class DonorCommitment : ContactPaymentEntity
	{
		[EntityNameMap("msnfp_donorcommitmentid")]
		public Guid DonorCommitmentId { get; set; }

		[EntityNameMap("msnfp_totalamount")]
		public decimal? TotalAmount { get; set; }

		[EntityReferenceMap("msnfp_appealid")]
		[EntityLogicalName("msnfp_appeal")]
		public Guid? AppealId { get; set; }

		[EntityReferenceMap("msnfp_packageid")]
		[EntityLogicalName("msnfp_package")]
		public Guid? PackageId { get; set; }

		[EntityReferenceMap("msnfp_commitment_campaignid")]
		[EntityLogicalName("campaign")]
		public Guid? CampaignId { get; set; }

		[EntityNameMap("msnfp_bookdate", Format = "yyyy-MM-dd")]
		public DateTime? BookDate { get; set; }

		[EntityNameMap("msnfp_totalamount_balance")]
		public decimal? TotalAmountBalance { get; set; }
	}
}
