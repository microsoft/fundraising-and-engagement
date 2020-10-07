using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PaymentProcessors.Models;
using PaymentProcessors.StripeIntegration.Helpers;

namespace PaymentProcessors.Stripe
{
	internal class StripeClient : IPaymentClient<StripeCreditCardPaymentInput, PaymentOutput>
	{
		private readonly HttpClient httpClient;

		public StripeClient(IHttpClientFactory httpClientFactory)
		{
			this.httpClient = httpClientFactory.CreateClient("PaymentProcessor");
		}

		public async Task<PaymentOutput> MakePaymentAsync(StripeCreditCardPaymentInput input, CancellationToken cancellationToken = default)
		{
			var output = new PaymentOutput();

			if (!input.IsBankProcess)
			{
				var isoCurrencyCode = input.IsoCurrencyCode;

				var baseStipeRepository = new BaseStipeRepository(this.httpClient);
				var secretKey = input.ServiceKey;
				StripeConfiguration.SetApiKey(secretKey);

				var custName = input.FirstName + " " + input.LastName;
				var custEmail = input.EmailAddress;

				var repository = new CustomerServiceBaseStipeRepository(this.httpClient);
				var stripeCustomer = await repository.GetStripeCustomerAsync(custName, custEmail, secretKey, cancellationToken);

				var myToken = new StripeTokenCreateOptions();
				var expMMYY = input.CcExpMmYy;
				myToken.Card = new StripeCreditCardOptions()
				{
					Number = input.CreditCardNo,
					ExpirationYear = expMMYY.Substring(expMMYY.Length - 2),
					ExpirationMonth = expMMYY.Substring(0, expMMYY.Length - 2),
					Cvc = input.Cvc
				};
				var tokenService = new StripeTokenService();
				var stripeTokenFinal = await tokenService.CreateAsync(myToken);

				var stripeCardObj = new StripeCard();
				stripeCardObj.SourceToken = stripeTokenFinal.Id;
				var url = String.Format("https://api.stripe.com/v1/customers/{0}/sources", stripeCustomer.Id);
				var stripeCard = await baseStipeRepository.CreateAsync(stripeCardObj, url, secretKey, cancellationToken);
				if (String.IsNullOrEmpty(stripeCard.Id))
					throw new Exception("Unable to add card to customer");

				var cardId = stripeCard.Id;

				var chargeAmount = (input.Amount * 100).ToString().Split('.')[0];
				var stripeObject = new StripeCharge();
				stripeObject.Amount = Convert.ToInt32(chargeAmount);
				stripeObject.Currency = isoCurrencyCode;
				stripeObject.Customer = stripeCustomer;
				var source = new Source();
				source.Id = cardId;
				stripeObject.Source = source;
				stripeObject.Description = Guid.NewGuid().ToString();

				var stripePayment = await baseStipeRepository.CreateAsync(stripeObject, "https://api.stripe.com/v1/charges", secretKey, cancellationToken);

				if (stripePayment != null)
				{
					if (String.Equals(stripePayment.Status, "succeeded", StringComparison.OrdinalIgnoreCase))
					{
						output.IsSuccessful = true;
						output.TransactionResult = stripePayment.Status;
					}
					else
					{
						//fail transaction
						output.IsSuccessful = false;
						output.TransactionResult = stripePayment.Status;
					}

					output.TransactionIdentifier = stripePayment.Id;
					output.InvoiceNumber = stripePayment.Description;
					output.AuthToken = stripePayment.Source.Id;
					output.StripeCustomerId = stripePayment.CustomerId;
					output.CardType = stripeCard.Brand;
				}
			}
			else
			{
				output.IsSuccessful = false;
				output.TransactionResult = "422";
			}

			return output;
		}
	}
}
