using System;

namespace FundraisingandEngagement.Models.Entities
{
	public interface IPaymentEntity
	{
		DateTime? CreatedOn { get; set; }

		bool? Deleted { get; set; }

		DateTime? DeletedDate { get; set; }

		int? StateCode { get; set; }

		DateTime? SyncDate { get; set; }

		void Delete();
	}
}