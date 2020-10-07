using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using PaymentProcessors.Stripe;

namespace PaymentProcessors.StripeIntegration.Model
{
	[JsonObject]
	public class StripeList<T> : StripeEntity, IEnumerable<T>, IEnumerable
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("data")]
		public List<T> Data { get; set; }

		public IEnumerator<T> GetEnumerator()
		{
			return Data.GetEnumerator();
		}

		[JsonProperty("url")]
		public string Url { get; set; }

		public bool HasMore { get; set; }

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Data.GetEnumerator();
		}
	}
}
