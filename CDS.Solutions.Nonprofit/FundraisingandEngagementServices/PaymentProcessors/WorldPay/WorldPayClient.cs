using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PaymentProcessors.Models;
using PaymentProcessors.Worldpay;

namespace PaymentProcessors.WorldPay
{
	internal class WorldPayClient : IPaymentClient<WorldPayCreditCardPaymentInput, PaymentOutput>
	{
		private readonly IHttpClientFactory httpClientFactory;

		public WorldPayClient(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
		}

		public async Task<PaymentOutput> MakePaymentAsync(WorldPayCreditCardPaymentInput input, CancellationToken cancellationToken = default)
		{
			var restClient = new WorldpayRestClient(WorldPayUrl.apiUrl, input.ServiceKey, this.httpClientFactory);
			var chargeAmount = (input.Amount * 100).ToString().Split('.')[0];

			var orderRequest = new OrderRequest()
			{
				token = input.WorldPayToken,
				amount = Convert.ToInt32(chargeAmount),
				currencyCode = String.IsNullOrEmpty(input.IsoCurrencyCode) ? "CAD" : input.IsoCurrencyCode,
				name = input.FirstName + " " + input.LastName,
				orderDescription = input.FirstName + " " + input.LastName,
				customerOrderCode = Guid.NewGuid().ToString()
			};

			if (input.IsRecurring)
			{
				orderRequest.orderType = "RECURRING";
			}

			var address = new Address()
			{
				address1 = input.BillingLine1,
				city = input.BillingCity,
				postalCode = input.BillingPostalCode
			};

			orderRequest.billingAddress = address;

			var response = await restClient.GetOrderService().CreateAsync(orderRequest, cancellationToken);

			var result = new PaymentOutput();

			if (String.Equals(response?.paymentStatus, "SUCCESS", StringComparison.OrdinalIgnoreCase))
			{
				result.IsSuccessful = true;
				result.TransactionResult = response.paymentStatus;
				result.TransactionIdentifier = response.orderCode;
				result.AuthToken = response.token;
			}
			else
			{
				result.IsSuccessful = false;
				result.TransactionResult = response?.paymentStatus;
			}

			result.InvoiceNumber = "MIS" + StringUtils.RandomString(6).ToUpper();

			return result;
		}
	}
}
