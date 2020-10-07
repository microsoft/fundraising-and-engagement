using System.IO;
using System.Net.Http;

namespace PaymentProcessors.Worldpay
{
	internal class WorldpayRestClient
	{
		/// <summary>
		/// The base url for the REST service
		/// </summary>
		private readonly string _baseUrl;

		/// <summary>
		/// The service key for authorizing access
		/// </summary>
		private readonly string _serviceKey;

		private readonly HttpClient httpClient;

		/// <summary>
		/// Constructor
		/// </summary>
		public WorldpayRestClient(string baseUrl, string serviceKey, IHttpClientFactory httpClientFactory)
		{
			if (baseUrl == null)
			{
				throw new InvalidDataException("baseUrl cannot be null");
			}

			this._baseUrl = baseUrl;

			if (serviceKey == null)
			{
				throw new InvalidDataException("serviceKey cannot be null");
			}

			this._serviceKey = serviceKey;
			this.httpClient = httpClientFactory.CreateClient("PaymentProcessor");
		}

		public OrderService GetOrderService()
		{
			return new OrderService(this._baseUrl, new Http(this._serviceKey, this.httpClient));
		}
	}
}
