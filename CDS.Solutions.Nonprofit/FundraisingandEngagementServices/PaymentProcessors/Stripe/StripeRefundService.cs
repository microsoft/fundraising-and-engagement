using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PaymentProcessors.StripeIntegration.Model;

namespace PaymentProcessors.Stripe
{
	public class StripeRefundService : StripeService
	{
		public StripeRefundService(string apiKey = null) : base(apiKey) { }

		public bool ExpandBalanceTransaction { get; set; }
		public bool ExpandCharge { get; set; }

		//Async
		public virtual async Task<StripeRefund> CreateAsync(string chargeId, StripeRefundCreateOptions createOptions = null, StripeRequestOptions requestOptions = null, CancellationToken cancellationToken = default)
		{
			return Mapper<StripeRefund>.MapFromJson(
				await Requestor.PostStringAsync(this.ApplyAllParameters(createOptions, $"{Urls.Charges}/{chargeId}/refunds", false),
				SetupRequestOptions(requestOptions),
				cancellationToken)
			);
		}

		public virtual async Task<StripeRefund> GetAsync(string refundId, StripeRequestOptions requestOptions = null, CancellationToken cancellationToken = default)
		{
			return Mapper<StripeRefund>.MapFromJson(
				await Requestor.GetStringAsync(
					this.ApplyAllParameters(null, $"{Urls.BaseUrl}/refunds/{refundId}"),
					SetupRequestOptions(requestOptions),
					cancellationToken
				)
			);
		}

		public virtual async Task<StripeRefund> UpdateAsync(string refundId, StripeRefundUpdateOptions updateOptions, StripeRequestOptions requestOptions = null, CancellationToken cancellationToken = default)
		{
			return Mapper<StripeRefund>.MapFromJson(
				await Requestor.PostStringAsync(
					this.ApplyAllParameters(updateOptions, $"{Urls.BaseUrl}/refunds/{refundId}"),
					SetupRequestOptions(requestOptions),
					cancellationToken
				)
			);
		}

		public virtual async Task<IEnumerable<StripeRefund>> ListAsync(StripeRefundListOptions listOptions = null, StripeRequestOptions requestOptions = null, CancellationToken cancellationToken = default)
		{
			return Mapper<StripeRefund>.MapCollectionFromJson(
				await Requestor.GetStringAsync(
					this.ApplyAllParameters(listOptions, $"{Urls.BaseUrl}/refunds", true),
					SetupRequestOptions(requestOptions),
					cancellationToken
				)
			);
		}
	}
}
