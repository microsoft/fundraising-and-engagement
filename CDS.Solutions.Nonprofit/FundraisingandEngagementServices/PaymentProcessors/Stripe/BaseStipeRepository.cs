using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PaymentProcessors.Stripe;
using PaymentProcessors.StripeIntegration.Model;

namespace PaymentProcessors.StripeIntegration.Helpers
{
	public class BaseStipeRepository
	{
		private readonly HttpClient httpClient;

		public BaseStipeRepository(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		public async Task<T> Get<T>(string url, string apiKey, CancellationToken cancellationToken)
		{
			var request = GetRequestMessage(url, HttpMethod.Get, apiKey, null);
			var response = await this.httpClient.SendAsync(request, cancellationToken);
			response.EnsureSuccessStatusCode();
			return await ResponseHelper<T>(response);
		}

		public async Task<T> CreateAsync<T>(T stripeObject, string url, string apiKey, CancellationToken cancellationToken) where T : StripeEntityWithId
		{
			var type = typeof(T);
			var contentstring = String.Empty;
			if (type.Equals(typeof(StripeCustomer)))
				contentstring = GetStripeCustomerString((object)stripeObject as StripeCustomer);
			if (type.Equals(typeof(StripeCharge)))
				contentstring = GetStripeChargeString((object)stripeObject as StripeCharge);
			if (type.Equals(typeof(StripeCard)))
				contentstring = GetStripeChargeString((object)stripeObject as StripeCard);

			var request = GetRequestMessage(url, HttpMethod.Post, apiKey, contentstring);
			var response = await this.httpClient.SendAsync(request, cancellationToken);
			response.EnsureSuccessStatusCode();
			return await ResponseHelper<T>(response);
		}

		private string GetStripeCustomerString(StripeCustomer customer)
		{
			return String.Format("description={0}&email={1}", customer.Description != null ? customer.Description.Replace(' ', '+') : String.Empty, customer.Email);
		}

		private string GetStripeChargeString(StripeCharge charge)
		{
			return String.Format("description={0}&currency={1}&amount={2}&customer={3}&source={4}", charge.Description != null ? charge.Description.Replace(' ', '+') : String.Empty, charge.Currency, charge.Amount, charge.Customer.Id, charge.Source.Id);
		}

		private string GetStripeChargeString(StripeCard card)
		{
			return String.Format("source={0}", card.SourceToken);
		}

		private async Task<T> ResponseHelper<T>(HttpResponseMessage response)
		{
			var result = await response.Content.ReadAsStringAsync();
			if (!response.IsSuccessStatusCode)
				throw new Exception(JObject.Parse(result)["error"]["message"]?.ToString());
			return Mapper<T>.MapFromJson(BuildResponseData(response, result), null);
		}

		private HttpRequestMessage GetRequestMessage(
		  string url,
		  HttpMethod method,
		  string apiKey,
		  string contentstring = null)
		{
			var httpRequestMessage = new HttpRequestMessage(method, new Uri(url));
			httpRequestMessage.Headers.Add("Authorization", String.Format("Bearer {0}", apiKey));
			if (method == HttpMethod.Post)
				httpRequestMessage.Content = new StringContent(contentstring, Encoding.UTF8, "application/x-www-form-urlencoded");
			return httpRequestMessage;
		}

		private StripeResponse BuildResponseData(
		  HttpResponseMessage response,
		  string responseText)
		{
			return new StripeResponse()
			{
				RequestId = response.Headers.Contains("Request-Id") ? response.Headers.GetValues("Request-Id").First() : "n/a",
				RequestDate = Convert.ToDateTime(response.Headers.GetValues("Date").First(), CultureInfo.InvariantCulture),
				ResponseJson = responseText
			};
		}
	}
}
