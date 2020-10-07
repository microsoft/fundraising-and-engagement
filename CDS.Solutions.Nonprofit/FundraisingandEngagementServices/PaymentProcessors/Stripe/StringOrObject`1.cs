using System;
using Newtonsoft.Json.Linq;
using PaymentProcessors.Stripe;

namespace PaymentProcessors.StripeIntegration.Helpers
{
	internal static class StringOrObject<T> where T : StripeEntityWithId
	{
		public static void Map(object value, Action<string> updateId, Action<T> updateObject)
		{
			if (value is JObject)
			{
				var obj = ((JToken)value).ToObject<T>();
				updateId(obj.Id);
				updateObject(obj);
			}
			else
			{
				if (!(value is string))
					return;
				updateId((string)value);
				updateObject(default);
			}
		}
	}
}
