using System;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentProcessors.Worldpay
{
	internal class OrderService
	{
		private readonly string _baseUrl;
		protected Http Http;

		/// <summary>
		/// Constructor
		/// </summary>
		public OrderService(string baseUrl, Http http)
		{
			this._baseUrl = baseUrl;
			this.Http = http;
		}

		/// <summary>
		/// Create a new order
		/// </summary>
		/// <param name="orderRequest">Details of the order to be created</param>
		/// <returns>Confirmation of the new order</returns>
		public async Task<OrderResponse> CreateAsync(OrderRequest orderRequest, CancellationToken cancellationToken = default)
		{
			return await this.Http.PostAsync<OrderRequest, OrderResponse>(this._baseUrl + "/orders", orderRequest, cancellationToken);
		}

		/// <summary>
		/// Refund and existing order
		/// </summary>
		/// <param name="orderCode">The code of the order to be refunded</param>
		public async Task RefundAsync(string orderCode)
		{
			await this.Http.PostAsync<string>(String.Format("{0}/orders/{1}/refund", this._baseUrl, orderCode), null);
		}

		/// <summary>
		/// Partially refund an existing order
		/// </summary>
		/// <param name="orderCode">The code of the order to be partially refunded</param>
		/// <param name="amount">The amount of the order to be partially refunded</param>
		public async Task RefundAsync(string orderCode, int amount)
		{
			if (amount == 0)
			{
				await RefundAsync(orderCode);
			}
			else
			{
				var partialRefund = new PartialRefund { refundAmount = amount };
				await this.Http.PostAsync(String.Format("{0}/orders/{1}/refund", this._baseUrl, orderCode), partialRefund);
			}
		}
	}
}
