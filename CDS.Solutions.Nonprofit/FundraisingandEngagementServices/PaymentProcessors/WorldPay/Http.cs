using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentProcessors.Worldpay
{
	internal class Http
	{
		private readonly HttpClient httpClient;

		/// <summary>
		/// Connection timeout in milliseconds
		/// </summary>
		private const int ConnectionTimeout = 61000;

		/// <summary>
		/// JSON header value
		/// </summary>
		private const string ApplicationJson = "application/json";

		/// <summary>
		/// Service key
		/// </summary>
		private readonly string _serviceKey;

		/// <summary>
		/// Authenticated
		/// </summary>
		private readonly bool _authenticated;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="serviceKey">The authorization key for the service</param>
		public Http(string serviceKey, HttpClient httpClient)
		{
			this._serviceKey = serviceKey;
			this._authenticated = true;
			this.httpClient = httpClient;
			this.httpClient.Timeout = TimeSpan.FromMilliseconds(ConnectionTimeout);
		}

		/// <summary>
		/// Perform a GET request
		/// </summary>
		internal async Task<TOutput> GetAsync<TInput, TOutput>(string api, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync<object>(api, HttpMethod.Get, null, this._authenticated, cancellationToken);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadFromJsonAsync<TOutput>(cancellationToken: cancellationToken);
			return result;
		}

		/// <summary>
		/// Perform a POST request
		/// </summary>
		internal async Task<TOutput> PostAsync<TInput, TOutput>(string api, TInput item, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync(api, HttpMethod.Post, item, this._authenticated, cancellationToken);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadFromJsonAsync<TOutput>(cancellationToken: cancellationToken);
			return result;
		}

		/// <summary>
		/// Perform a POST request
		/// </summary>
		internal async Task PostAsync<TInput>(string api, TInput item, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync(api, HttpMethod.Post, item, this._authenticated, cancellationToken);
			response.EnsureSuccessStatusCode();
		}

		/// <summary>
		/// Perform a POST request
		/// </summary>
		internal async Task<TOutput> PutAsync<TInput, TOutput>(string api, TInput item, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync(api, HttpMethod.Put, item, this._authenticated, cancellationToken);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadFromJsonAsync<TOutput>(cancellationToken: cancellationToken);
			return result;
		}

		/// <summary>
		/// Perform a DELETE request
		/// </summary>
		public async Task DeleteAsync(string api, CancellationToken cancellationToken = default)
		{
			var response = await SendRequestAsync<object>(api, HttpMethod.Delete, null, this._authenticated, cancellationToken);
			response.EnsureSuccessStatusCode();
		}

		private async Task<HttpResponseMessage> SendRequestAsync<T>(string api, HttpMethod method, T data, bool authorize, CancellationToken cancellationToken = default)
		{
			var request = new HttpRequestMessage(method, api);
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJson));
			request.Headers.Add("x-wp-client-user-agent", "NET-Client-4");

			if (authorize)
			{
				request.Headers.Add("Authorization", this._serviceKey);
			}

			if (data != null)
			{
				request.Content = JsonContent.Create(data, new MediaTypeHeaderValue(ApplicationJson));
			}

			return await this.httpClient.SendAsync(request, cancellationToken);
		}
	}
}
