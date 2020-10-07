using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PaymentProcessors.Models;
using PaymentProcessors.StripeIntegration.Helpers;

namespace PaymentProcessors.Stripe
{
	internal class StripeRecurringClient : IPaymentClient<StripeRecurringPaymentInput, StripeRecurringPaymentOutput>
	{
		private readonly HttpClient httpClient;

		public StripeRecurringClient(IHttpClientFactory httpClientFactory)
		{
			this.httpClient = httpClientFactory.CreateClient("PaymentProcessor");
		}

		public async Task<StripeRecurringPaymentOutput> MakePaymentAsync(StripeRecurringPaymentInput input, CancellationToken cancellationToken = default)
		{
			var output = new StripeRecurringPaymentOutput(input);
			var baseStipeRepository = new BaseStipeRepository(this.httpClient);

			StripeCharge stripePayment = null;
			StripeConfiguration.SetApiKey(input.ServiceKey);

			if (!String.IsNullOrEmpty(input.StripeCardId) && !String.IsNullOrEmpty(input.StripeCustomerId))
			{
				var custService = new StripeCustomerService();
				var stripeCustomer = await custService.GetAsync(input.StripeCustomerId, cancellationToken: cancellationToken);

				var chargeAmount = Convert.ToDecimal((input.Amount));
				var stripeAmount = Convert.ToInt32((chargeAmount * 100).ToString().Split('.')[0]);

				var stripeObject = new StripeCharge();
				stripeObject.Amount = stripeAmount;
				stripeObject.Currency = input.IsoCurrencyCode;
				stripeObject.Customer = stripeCustomer;
				var source = new Source();
				source.Id = input.StripeCardId;
				stripeObject.Source = source;
				stripeObject.Description = input.InvoiceIdentifier;

				stripePayment = await baseStipeRepository.CreateAsync(stripeObject, "https://api.stripe.com/v1/charges", input.ServiceKey, cancellationToken);
			}

			if (stripePayment == null)
			{
				output.SetFailure(input);
			}
			else
			{
				if (String.Equals(stripePayment.Status, "succeeded", StringComparison.OrdinalIgnoreCase))
				{
					output.IsSuccessful = true;
					output.TransactionStatus = stripePayment.Status;
					output.TransactionResult = stripePayment.Status;
					output.TransactionIdentifier = stripePayment.Id;
				}
				else
				{
					output.SetFailure(input);
					output.TransactionStatus = stripePayment.Status;
					output.TransactionResult = stripePayment.Status;
					output.TransactionIdentifier = stripePayment.Id;
				}
			}

			return output;
		}
	}
}
