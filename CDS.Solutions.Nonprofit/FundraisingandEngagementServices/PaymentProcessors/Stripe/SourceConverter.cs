using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentProcessors.Stripe;
using PaymentProcessors.StripeIntegration.Model;

namespace PaymentProcessors.StripeIntegration.Helpers
{
	internal class SourceConverter : JsonConverter
	{
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		public override bool CanConvert(Type objectType)
		{
			throw new NotImplementedException();
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JObject.FromObject(value).WriteTo(writer);
		}

		public override object ReadJson(
		  JsonReader reader,
		  Type objectType,
		  object existingValue,
		  JsonSerializer serializer)
		{
			var jobject = JObject.Load(reader);
			var source1 = new Source();
			source1.Id = jobject.SelectToken("id").ToString();
			var source2 = source1;
			if (jobject.SelectToken("object")?.ToString() == "card")
			{
				source2.Type = SourceType.Card;
				source2.Card = Mapper<StripeCard>.MapFromJson(jobject.ToString(), null, null);
			}
			if (jobject.SelectToken("deleted")?.ToString() == "true")
			{
				source2.Type = SourceType.Deleted;
				source2.Deleted = Mapper<StripeDeleted>.MapFromJson(jobject.ToString(), null, null);
			}
			return source2;
		}
	}
}
