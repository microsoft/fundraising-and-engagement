using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentProcessors.Stripe;

namespace PaymentProcessors.StripeIntegration.Model
{
	public static class Mapper<T>
	{
		public static List<T> MapCollectionFromJson(
		  string json,
		  string token = "data",
		  StripeResponse stripeResponse = null)
		{
			return JObject.Parse(json).SelectToken(token).Select(tkn => Mapper<T>.MapFromJson(tkn.ToString(), null, stripeResponse)).ToList();
		}

		public static List<T> MapCollectionFromJson(StripeResponse stripeResponse, string token = "data")
		{
			return Mapper<T>.MapCollectionFromJson(stripeResponse.ResponseJson, token, stripeResponse);
		}

		public static T MapFromJson(string json, string parentToken = null, StripeResponse stripeResponse = null)
		{
			var obj = JsonConvert.DeserializeObject<T>(String.IsNullOrEmpty(parentToken) ? json : JObject.Parse(json).SelectToken(parentToken).ToString());
			Mapper<T>.applyStripeResponse(json, stripeResponse, obj);
			return obj;
		}

		public static T MapFromJson(StripeResponse stripeResponse, string parentToken = null)
		{
			return Mapper<T>.MapFromJson(stripeResponse.ResponseJson, parentToken, stripeResponse);
		}

		private static void applyStripeResponse(string json, StripeResponse stripeResponse, object obj)
		{
			if (stripeResponse == null)
				return;
			foreach (var runtimeProperty in obj.GetType().GetRuntimeProperties())
			{
				if (runtimeProperty.Name == "StripeResponse")
					runtimeProperty.SetValue(obj, stripeResponse);
			}
			stripeResponse.ObjectJson = json;
		}
	}
}
