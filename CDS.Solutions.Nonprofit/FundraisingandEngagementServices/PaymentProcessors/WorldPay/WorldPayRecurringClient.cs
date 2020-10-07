using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PaymentProcessors.Models;
using PaymentProcessors.Worldpay;

namespace PaymentProcessors.WorldPay
{
	internal class WorldPayRecurringClient : IPaymentClient<WorldPayRecurringPaymentInput, WorldPayRecurringPaymentOutput>
	{
		private readonly IHttpClientFactory httpClientFactory;

		public WorldPayRecurringClient(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
		}

		public async Task<WorldPayRecurringPaymentOutput> MakePaymentAsync(WorldPayRecurringPaymentInput input, CancellationToken cancellationToken = default)
		{
			var worldPayAmount = Convert.ToInt32((input.Amount * 100).ToString().Split('.')[0]);

			var restClient = new WorldpayRestClient(WorldPayUrl.apiUrl, input.ServiceKey, this.httpClientFactory);

			var orderRequest = new OrderRequest()
			{
				token = input.WorldPayToken,
				amount = worldPayAmount,
				currencyCode = input.IsoCurrencyCode,
				name = input.FirstName + " " + input.LastName,
				orderType = "RECURRING",
				orderDescription = input.InvoiceIdentifier
			};

			var address = new Address()
			{
				address1 = input.Address1,
				address2 = input.Address2,
				city = input.City,
				postalCode = input.PostalCode
			};

			orderRequest.billingAddress = address;

			var response = await restClient.GetOrderService().CreateAsync(orderRequest, cancellationToken);

			var output = new WorldPayRecurringPaymentOutput(input);

			if (String.Equals(response.paymentStatus, "SUCCESS", StringComparison.OrdinalIgnoreCase))
			{
				output.IsSuccessful = true;
				output.TransactionStatus = response.paymentStatus;
				output.TransactionResult = response.paymentStatus;
				output.TransactionIdentifier = response.token;
			}
			else
			{
				output.SetFailure(input);
				output.TransactionStatus = response.paymentStatus;
				output.TransactionResult = response.paymentStatus;
				output.TransactionIdentifier = response.token;
			}

			return output;
		}
	}
}
