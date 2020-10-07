using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace PaymentProcessors.Stripe.Middleware
{
	internal class DateFilterPlugin : IParserPlugin
	{
		public bool Parse(ref string requestString, JsonPropertyAttribute attribute, PropertyInfo property, object propertyValue, object propertyParent)
		{
			if (property.PropertyType != typeof(StripeDateFilter)) return false;

			var filter = (StripeDateFilter)propertyValue;

			if (filter.EqualTo.HasValue)
				RequestStringBuilder.ApplyParameterToRequestString(ref requestString, attribute.PropertyName, filter.EqualTo.Value.ConvertDateTimeToEpoch().ToString());

			if (filter.LessThan.HasValue)
				RequestStringBuilder.ApplyParameterToRequestString(ref requestString, attribute.PropertyName + "[lt]", filter.LessThan.Value.ConvertDateTimeToEpoch().ToString());

			if (filter.LessThanOrEqual.HasValue)
				RequestStringBuilder.ApplyParameterToRequestString(ref requestString, attribute.PropertyName + "[lte]", filter.LessThanOrEqual.Value.ConvertDateTimeToEpoch().ToString());

			if (filter.GreaterThan.HasValue)
				RequestStringBuilder.ApplyParameterToRequestString(ref requestString, attribute.PropertyName + "[gt]", filter.GreaterThan.Value.ConvertDateTimeToEpoch().ToString());

			if (filter.GreaterThanOrEqual.HasValue)
				RequestStringBuilder.ApplyParameterToRequestString(ref requestString, attribute.PropertyName + "[gte]", filter.GreaterThanOrEqual.Value.ConvertDateTimeToEpoch().ToString());

			return true;
		}
	}

	internal class DictionaryPlugin : IParserPlugin
	{
		public bool Parse(ref string requestString, JsonPropertyAttribute attribute, PropertyInfo property, object propertyValue, object propertyParent)
		{
			if (!attribute.PropertyName.Contains("metadata") && !attribute.PropertyName.Contains("fraud_details")) return false;

			var dictionary = (Dictionary<string, string>)propertyValue;
			if (dictionary == null) return true;

			foreach (var key in dictionary.Keys)
				RequestStringBuilder.ApplyParameterToRequestString(ref requestString, $"{attribute.PropertyName}[{key}]", dictionary[key]);

			return true;
		}
	}

	public interface INestedOptions
	{
		// this interface just needs to be implemented by any class that is
		// a nested object under any of the primary service options
	}

	public interface IParserPlugin
	{
		bool Parse(ref string requestString, JsonPropertyAttribute attribute, PropertyInfo property, object propertyValue, object propertyParent);
	}
}
