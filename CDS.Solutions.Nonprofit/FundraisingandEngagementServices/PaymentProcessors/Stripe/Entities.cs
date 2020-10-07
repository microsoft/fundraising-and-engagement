using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentProcessors.Stripe.Infrastructure;
using PaymentProcessors.Stripe.Middleware;
using PaymentProcessors.StripeIntegration.Helpers;
using PaymentProcessors.StripeIntegration.Model;

namespace PaymentProcessors.Stripe
{
	internal class Client
	{
		private HttpRequestMessage RequestMessage { get; set; }

		public Client(HttpRequestMessage requestMessage)
		{
			RequestMessage = requestMessage;
		}

		public void ApplyUserAgent()
		{
			RequestMessage.Headers.UserAgent.ParseAdd($"Stripe/v1 .NetBindings/{StripeConfiguration.StripeNetVersion}");
		}

		public void ApplyClientData()
		{
			RequestMessage.Headers.Add("X-Stripe-Client-User-Agent", GetClientUserAgentString());
		}

		private string GetClientUserAgentString()
		{
			var langVersion = "4.5";

#if NET45
            langVersion = typeof(object).GetTypeInfo().Assembly.ImageRuntimeVersion;
#endif

			var mono = testForMono();
			if (!String.IsNullOrEmpty(mono)) langVersion = mono;

			var values = new Dictionary<string, string>
			{
				{ "bindings_version", StripeConfiguration.StripeNetVersion },
				{ "lang", ".net" },
				{ "publisher", "Jayme Davis" },
				{ "lang_version", WebUtility.HtmlEncode(langVersion) },
				{ "uname", WebUtility.HtmlEncode(getSystemInformation()) }
			};

			return JsonConvert.SerializeObject(values);
		}

		private string testForMono()
		{
			var type = Type.GetType("Mono.Runtime");
			var getDisplayName = type?.GetTypeInfo().GetDeclaredMethod("GetDisplayName");

			return getDisplayName?.Invoke(null, null).ToString();
		}

		private string getSystemInformation()
		{
			var result = String.Empty;

#if NET45
            result += $"net45.platform: { Environment.OSVersion.VersionString }";
            result += $", {getOperatingSystemInfo()}"; 
#else
			result += "portable.platform: ";

			try
			{
				result += typeof(object).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
			}
			catch
			{
				result += "unknown";
			}
#endif

			return result;
		}

#if NET45
        private string getOperatingSystemInfo()
        {
            var os = Environment.OSVersion;
            var pid = os.Platform;

            switch (pid)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    return "OS: Windows";
                case PlatformID.Unix:
                    return "OS: Unix";
                default:
                    return "OS: Unknown";
            }
        }
#endif

	}

	internal class AdditionalOwnerPlugin : IParserPlugin
	{
		public bool Parse(ref string requestString, JsonPropertyAttribute attribute, PropertyInfo property, object propertyValue, object propertyParent)
		{
			if (attribute.PropertyName != "legal_entity[additional_owners]") return false;

			var owners = ((List<StripeAccountAdditionalOwner>)property.GetValue(propertyParent, null));
			if (owners == null) return true;

			var ownerIndex = 0;

			foreach (var owner in owners)
			{
				var properties = owner.GetType().GetRuntimeProperties();

				foreach (var prop in properties)
				{
					var value = prop.GetValue(owner, null);
					if (value == null) continue;

					// it must have a json attribute matching stripe's key, and only one
					var attr = prop.GetCustomAttributes<JsonPropertyAttribute>().SingleOrDefault();
					if (attr == null) continue;

					RequestStringBuilder.ApplyParameterToRequestString(ref requestString,
						$"{attribute.PropertyName}[{ownerIndex}]{attr.PropertyName}",
						value.ToString());
				}

				ownerIndex++;
			}

			return true;
		}
	}

	public class BankAccountOptions : INestedOptions
	{
		[JsonProperty("bank_account[account_holder_name]")]
		public string AccountHolderName { get; set; }

		[JsonProperty("bank_account[account_holder_type]")]
		public string AccountHolderType { get; set; }

		[JsonProperty("bank_account[account_number]")]
		public string AccountNumber { get; set; }

		[JsonProperty("bank_account[country]")]
		public string Country { get; set; }

		[JsonProperty("bank_account[currency]")]
		public string Currency { get; set; }

		[JsonProperty("bank_account[routing_number]")]
		public string RoutingNumber { get; set; }
	}

	internal static class ParameterBuilder
	{
		public static string ApplyAllParameters(this StripeService service, object obj, string url, bool isListMethod = false)
		{
			// store the original url from the service call into requestString (e.g. https://api.stripe.com/v1/accounts/account_id)
			// before the stripe attributes get applied. all of the attributes that will get passed to stripe will be applied to this string,
			// don't worry - if the request is a post, the Requestor will take care of moving the attributes to the post body
			var requestString = url;

			// obj = the options object passed from the service
			if (obj != null)
			{
				foreach (var property in obj.GetType().GetRuntimeProperties())
				{
					var value = property.GetValue(obj, null);
					if (value == null) continue;

					foreach (var attribute in property.GetCustomAttributes<JsonPropertyAttribute>())
					{
						if (value is INestedOptions)
							ApplyNestedObjectProperties(ref requestString, value);
						else
							RequestStringBuilder.ProcessPlugins(ref requestString, attribute, property, value, obj);
					}
				}
			}

			if (service != null)
			{
				// expandable properties
				var propertiesToExpand = service.GetType()
					.GetRuntimeProperties()
					.Where(p => p.Name.StartsWith("Expand") && p.PropertyType == typeof(bool))
					.Where(p => (bool)p.GetValue(service, null))
					.Select(p => p.Name);

				foreach (var propertyName in propertiesToExpand)
				{
					var expandPropertyName = propertyName.Substring("Expand".Length);
					expandPropertyName = Regex.Replace(expandPropertyName, "([a-z])([A-Z])", "$1_$2").ToLower();

					if (isListMethod)
					{
						expandPropertyName = "data." + expandPropertyName;
					}

					requestString = ApplyParameterToUrl(requestString, "expand[]", expandPropertyName);

					// note: I had no idea you could expand properties beyond the first level (up to 4 before stripe throws an exception).
					// something to consider adding to the project.
					//
					// example:
					// requestString = ApplyParameterToUrl(requestString, "expand[]", "data.charge.dispute.charge.dispute.charge.dispute");
				}
			}

			return requestString;
		}

		public static string ApplyParameterToUrl(string url, string argument, string value)
		{
			RequestStringBuilder.ApplyParameterToRequestString(ref url, argument, value);

			return url;
		}

		private static void ApplyNestedObjectProperties(ref string requestString, object nestedObject)
		{
			foreach (var property in nestedObject.GetType().GetRuntimeProperties())
			{
				var value = property.GetValue(nestedObject, null);
				if (value == null) continue;

				foreach (var attribute in property.GetCustomAttributes<JsonPropertyAttribute>())
					RequestStringBuilder.ProcessPlugins(ref requestString, attribute, property, value, nestedObject);
			}
		}

		public static string ApplyAllParameters<T>(this Service<T> service, BaseOptions obj, string url, bool isListMethod = false)
			where T : IStripeEntity
		{
			// store the original url from the service call into requestString (e.g. https://api.stripe.com/v1/accounts/account_id)
			// before the stripe attributes get applied. all of the attributes that will get passed to stripe will be applied to this string,
			// don't worry - if the request is a post, the Requestor will take care of moving the attributes to the post body
			var requestString = url;

			// obj = the options object passed from the service
			if (obj != null)
			{
				RequestStringBuilderObj.CreateQuery(ref requestString, obj);

				foreach (var pair in obj.ExtraParams)
				{
					var key = WebUtility.UrlEncode(pair.Key);
					RequestStringBuilder.ApplyParameterToRequestString(ref requestString, key, pair.Value);
				}

				foreach (var value in obj.Expand)
				{
					RequestStringBuilder.ApplyParameterToRequestString(ref requestString, "expand[]", value);
				}
			}

			if (service != null)
			{
				// expandable properties
				var propertiesToExpand = service.GetType()
					.GetRuntimeProperties()
					.Where(p => p.Name.StartsWith("Expand") && p.PropertyType == typeof(bool))
					.Where(p => (bool)p.GetValue(service, null))
					.Select(p => p.Name);

				foreach (var propertyName in propertiesToExpand)
				{
					var expandPropertyName = propertyName.Substring("Expand".Length);
					expandPropertyName = Regex.Replace(expandPropertyName, "([a-z])([A-Z])", "$1_$2").ToLower();

					if (isListMethod)
					{
						expandPropertyName = "data." + expandPropertyName;
					}

					requestString = ApplyParameterToUrl(requestString, "expand[]", expandPropertyName);
				}
			}

			return requestString;
		}
	}


	internal static class Requestor
	{
		internal static HttpClient HttpClient { get; }

		static Requestor()
		{
			HttpClient =
				StripeConfiguration.HttpMessageHandler != null
					? new HttpClient(StripeConfiguration.HttpMessageHandler)
					: new HttpClient();
		}

		// Async
		public static Task<StripeResponse> GetStringAsync(string url, StripeRequestOptions requestOptions, CancellationToken cancellationToken = default)
		{
			var wr = GetRequestMessage(url, HttpMethod.Get, requestOptions);

			return ExecuteRequestAsync(wr, cancellationToken);
		}

		public static Task<StripeResponse> PostStringAsync(string url, StripeRequestOptions requestOptions, CancellationToken cancellationToken = default)
		{
			var wr = GetRequestMessage(url, HttpMethod.Post, requestOptions);

			return ExecuteRequestAsync(wr, cancellationToken);
		}

		public static Task<StripeResponse> DeleteAsync(string url, StripeRequestOptions requestOptions, CancellationToken cancellationToken = default)
		{
			var wr = GetRequestMessage(url, HttpMethod.Delete, requestOptions);

			return ExecuteRequestAsync(wr, cancellationToken);
		}

		public static Task<StripeResponse> PostStringBearerAsync(string url, StripeRequestOptions requestOptions, CancellationToken cancellationToken = default)
		{
			var wr = GetRequestMessage(url, HttpMethod.Post, requestOptions, true);

			return ExecuteRequestAsync(wr, cancellationToken);
		}

		public static Task<StripeResponse> PostFileAsync(string url, string fileName, Stream fileStream, string purpose, StripeRequestOptions requestOptions, CancellationToken cancellationToken = default)
		{
			var wr = GetRequestMessage(url, HttpMethod.Post, requestOptions);

			ApplyMultiPartFileToRequest(wr, fileName, fileStream, purpose);

			return ExecuteRequestAsync(wr, cancellationToken);
		}

		private static async Task<StripeResponse> ExecuteRequestAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken = default)
		{
			var response = await HttpClient.SendAsync(requestMessage, cancellationToken);

			response.EnsureSuccessStatusCode();

			return BuildResponseData(response, await response.Content.ReadAsStringAsync());
		}



		private static HttpRequestMessage GetRequestMessage(string url, HttpMethod method, StripeRequestOptions requestOptions, bool useBearer = false)
		{
			requestOptions.ApiKey = requestOptions.ApiKey ?? StripeConfiguration.GetApiKey();

			var request = BuildRequest(method, url);

			request.Headers.Add("Authorization",
				!useBearer
					? GetAuthorizationHeaderValue(requestOptions.ApiKey)
					: GetAuthorizationHeaderValueBearer(requestOptions.ApiKey));

			if (requestOptions.StripeConnectAccountId != null)
				request.Headers.Add("Stripe-Account", requestOptions.StripeConnectAccountId);

			if (requestOptions.IdempotencyKey != null)
				request.Headers.Add("Idempotency-Key", requestOptions.IdempotencyKey);

			request.Headers.Add("Stripe-Version", StripeConfiguration.StripeApiVersion);

			var client = new Client(request);
			client.ApplyUserAgent();
			client.ApplyClientData();

			return request;
		}

		private static HttpRequestMessage BuildRequest(HttpMethod method, string url)
		{
			if (method != HttpMethod.Post)
				return new HttpRequestMessage(method, new Uri(url));

			var postData = String.Empty;
			var newUrl = url;

			if (!String.IsNullOrEmpty(new Uri(url).Query))
			{
				postData = new Uri(url).Query.Substring(1);
				newUrl = url.Substring(0, url.IndexOf("?", StringComparison.CurrentCultureIgnoreCase));
			}

			var request = new HttpRequestMessage(method, new Uri(newUrl))
			{
				Content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded")
			};

			return request;
		}

		private static string GetAuthorizationHeaderValue(string apiKey)
		{
			var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:"));
			return $"Basic {token}";
		}

		private static string GetAuthorizationHeaderValueBearer(string apiKey)
		{
			return $"Bearer {apiKey}";
		}

		private static void ApplyMultiPartFileToRequest(HttpRequestMessage requestMessage, string fileName, Stream fileStream, string purpose)
		{
			requestMessage.Headers.ExpectContinue = true;

			var fileContent = new StreamContent(fileStream);
			fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
			{
				Name = "\"file\"",
				FileName = $"\"{fileName}\""
			};

			fileContent.Headers.ContentType = new MediaTypeHeaderValue(MimeTypes.GetMimeType(fileName));

			var multiPartContent =
				new MultipartFormDataContent($"----------Upload: { DateTime.Now.Ticks.ToString("x") }")
				{
					{ new StringContent(purpose), "\"purpose\"" },
					fileContent
				};

			requestMessage.Content = multiPartContent;
		}

		//private static StripeException BuildStripeException(HttpStatusCode statusCode, string requestUri, string responseContent)
		//{
		//    var stripeError = requestUri.Contains("oauth")
		//        ? Mapper<StripeError>.MapFromJson(responseContent)
		//        : Mapper<StripeError>.MapFromJson(responseContent, "error");

		//    return new StripeException(statusCode, stripeError, stripeError.Message);
		//}

		private static StripeResponse BuildResponseData(HttpResponseMessage response, string responseText)
		{
			var result = new StripeResponse
			{
				RequestId = response.Headers.GetValues("Request-Id").First(),
				RequestDate = Convert.ToDateTime(response.Headers.GetValues("Date").First()),
				ResponseJson = responseText
			};

			return result;
		}
	}

	public static class RequestStringBuilder
	{
		private static IEnumerable<IParserPlugin> ParserPlugins { get; }

		static RequestStringBuilder()
		{
			if (ParserPlugins != null) return;

			// use reflection so this works on the bin directory later for additional plugin processing tools

			ParserPlugins = new List<IParserPlugin>
			{
				new AdditionalOwnerPlugin(),
				new DictionaryPlugin(),
				new DateFilterPlugin()
			};
		}

		internal static void ProcessPlugins(ref string requestString, JsonPropertyAttribute attribute, PropertyInfo property, object propertyValue, object propertyParent)
		{
			var parsedParameter = false;

			foreach (var plugin in ParserPlugins)
			{
				if (!parsedParameter)
					parsedParameter = plugin.Parse(ref requestString, attribute, property, propertyValue, propertyParent);
			}

			if (!parsedParameter)
				ApplyParameterToRequestString(ref requestString, attribute.PropertyName, propertyValue.ToString());
		}

		public static void ApplyParameterToRequestString(ref string requestString, string argument, string value)
		{
			var token = "&";

			if (!requestString.Contains("?"))
				token = "?";

			requestString = $"{requestString}{token}{argument}={WebUtility.UrlEncode(value)}";
		}
	}

	public static class RequestStringBuilderObj
	{
		public static void ApplyParameterToRequestString(ref string requestString, string argument, string value)
		{
			var token = requestString.Contains("?") ? "&" : "?";
			requestString = $"{requestString}{token}{argument}={WebUtility.UrlEncode(value)}";
		}

		/// <summary>
		/// Creates the HTTP query string for a given options class.
		/// </summary>
		/// <param name="requestString">The string to which the query string will be appended.</param>
		/// <param name="options">The options class for which to create the query string.</param>
		public static void CreateQuery(ref string requestString, INestedOptions options)
		{
			var flatParams = FlattenParams(options);

			foreach (var param in flatParams)
			{
				RequestStringBuilder.ApplyParameterToRequestString(ref requestString, param.Key, param.Value);
			}
		}

		/// <summary>
		/// Returns a list of parameters for a given options class. If the class contains
		/// containers (e.g. dictionaries, lists, arrays or other options classes), the parameters
		/// will be computed recursively and a flat list will be returned.
		/// </summary>
		/// <param name="options">The options class for which to create the list of parameters.</param>
		/// <returns>The list of parameters</returns>
		private static List<Parameter> FlattenParams(INestedOptions options)
		{
			return FlattenParamsOptions(options, null);
		}

		/// <summary>
		/// Returns a list of parameters for a given value. The value can be basically anything, as
		/// long as it can be encoded in some way.
		/// </summary>
		/// <param name="value">The value for which to create the list of parameters.</param>
		/// <param name="keyPrefix">The key under which new keys should be nested, if any.</param>
		/// <returns>The list of parameters</returns>
		private static List<Parameter> FlattenParamsValue(object value, string keyPrefix)
		{
			List<Parameter> flatParams = null;

			if (value is INestedOptions)
			{
				flatParams = FlattenParamsOptions((INestedOptions)value, keyPrefix);
			}
			else if (IsDictionary(value))
			{
				// Cast to Dictionary<string, object>
				Dictionary<string, object> dictionary = ((IDictionary)value).
					Cast<dynamic>().
					ToDictionary(
						entry => (string)entry.Key,
						entry => entry.Value);
				flatParams = FlattenParamsDictionary(dictionary, keyPrefix);
			}
			else if (IsList(value))
			{
				// Cast to List<object>
				List<object> list = ((IEnumerable)value).
					Cast<dynamic>().
					ToList();
				flatParams = FlattenParamsList(list, keyPrefix);
			}
			else if (IsArray(value))
			{
				// Cast to object[]
				object[] array = ((IEnumerable)value).
					Cast<dynamic>().
					ToArray();
				flatParams = FlattenParamsArray(array, keyPrefix);
			}
			else if (IsEnum(value))
			{
				flatParams = new List<Parameter>();

				// Use JsonConvert to grab the EnumMemberAttribute's Value for the enum element
				var paramValue = JsonConvert.SerializeObject(value).Trim('"');

				flatParams.Add(new Parameter(keyPrefix, paramValue));
			}
			else if (value is DateTime)
			{
				flatParams = new List<Parameter>();
				DateTime? dateTime = (DateTime)value;
				if (dateTime.HasValue)
				{
					flatParams.Add(new Parameter(keyPrefix, dateTime?.ConvertDateTimeToEpoch().ToString(CultureInfo.InvariantCulture)));
				}
			}
			else if (value == null)
			{
				flatParams = new List<Parameter>();
				flatParams.Add(new Parameter(keyPrefix, String.Empty));
			}
			else
			{
				flatParams = new List<Parameter>();
				flatParams.Add(new Parameter(
					keyPrefix,
					String.Format(CultureInfo.InvariantCulture, "{0}", value)));
			}

			return flatParams;
		}

		/// <summary>
		/// Returns a list of parameters for a given options class. If a key prefix is provided, the
		/// keys for the new parameters will be nested under the key prefix. E.g. if the key prefix
		/// `foo` is passed and the options class contains a parameter `bar`, then a parameter
		/// with key `foo[bar]` will be returned.
		/// </summary>
		/// <param name="options">The options class for which to create the list of parameters.</param>
		/// <param name="keyPrefix">The key under which new keys should be nested, if any.</param>
		/// <returns>The list of parameters</returns>
		private static List<Parameter> FlattenParamsOptions(INestedOptions options, string keyPrefix)
		{
			var flatParams = new List<Parameter>();
			if (options == null)
			{
				return flatParams;
			}

			foreach (var property in options.GetType().GetRuntimeProperties())
			{
				var value = property.GetValue(options, null);

				// Fields on a class which are never set by the user will contain null values (for
				// reference types), so skip those to avoid encoding them in the request.
				if (value == null)
				{
					continue;
				}

				var attribute = property.GetCustomAttribute<JsonPropertyAttribute>();
				if (attribute == null)
				{
					continue;
				}

				var key = attribute.PropertyName;
				var newPrefix = NewPrefix(key, keyPrefix);

				flatParams.AddRange(FlattenParamsValue(value, newPrefix));
			}

			return flatParams;
		}

		/// <summary>
		/// Returns a list of parameters for a given dictionary. If a key prefix is provided, the
		/// keys for the new parameters will be nested under the key prefix. E.g. if the key prefix
		/// `foo` is passed and the dictionary contains a key `bar`, then a parameter with key
		/// `foo[bar]` will be returned.
		/// </summary>
		/// <param name="dictionary">The dictionary for which to create the list of parameters.</param>
		/// <param name="keyPrefix">The key under which new keys should be nested, if any.</param>
		/// <returns>The list of parameters</returns>
		private static List<Parameter> FlattenParamsDictionary(Dictionary<string, object> dictionary, string keyPrefix)
		{
			var flatParams = new List<Parameter>();
			if (dictionary == null)
			{
				return flatParams;
			}

			foreach (var entry in dictionary)
			{
				var key = WebUtility.UrlEncode(entry.Key);
				var value = entry.Value;

				var newPrefix = NewPrefix(key, keyPrefix);

				flatParams.AddRange(FlattenParamsValue(value, newPrefix));
			}

			return flatParams;
		}

		/// <summary>
		/// Returns a list of parameters for a given list of objects. The parameter keys will be
		/// indexed under the `keyPrefix` parameter. E.g. if the `keyPrefix` is `foo`, then the
		/// key for the first element's will be `foo[0]`, etc.
		/// </summary>
		/// <param name="list">The list for which to create the list of parameters.</param>
		/// <param name="keyPrefix">The key under which new keys should be nested.</param>
		/// <returns>The list of parameters</returns>
		private static List<Parameter> FlattenParamsList(List<object> list, string keyPrefix)
		{
			var flatParams = new List<Parameter>();
			if (list == null)
			{
				return flatParams;
			}

			if (!list.Any())
			{
				flatParams.Add(new Parameter(keyPrefix, String.Empty));
			}
			else
			{
				foreach (var item in list.Select((value, index) => new { value, index }))
				{
					var newPrefix = $"{keyPrefix}[{item.index}]";
					flatParams.AddRange(FlattenParamsValue(item.value, newPrefix));
				}
			}

			return flatParams;
		}

		/// <summary>
		/// Returns a list of parameters for a given array of objects. The parameter keys will be
		/// indexed under the `keyPrefix` parameter. E.g. if the `keyPrefix` is `foo`, then the
		/// key for the first element's will be `foo[0]`, etc.
		/// </summary>
		/// <param name="array">The array for which to create the list of parameters.</param>
		/// <param name="keyPrefix">The key under which new keys should be nested.</param>
		/// <returns>The list of parameters</returns>
		private static List<Parameter> FlattenParamsArray(object[] array, string keyPrefix)
		{
			var flatParams = new List<Parameter>();

			if (array.Length == 0)
			{
				flatParams.Add(new Parameter(keyPrefix, String.Empty));
			}
			else
			{
				for (var i = 0; i < array.Length; i++)
				{
					var value = array[i];
					var newPrefix = $"{keyPrefix}[{i}]";
					flatParams.AddRange(FlattenParamsValue(value, newPrefix));
				}
			}

			return flatParams;
		}

		/// <summary>
		/// Checks if a given object is a dictionary.
		/// </summary>
		/// <param name="o">The object to check.</param>
		/// <returns>True if the object is a dictionary, false otherwise.</returns>
		private static bool IsDictionary(object o)
		{
			if (o == null)
			{
				return false;
			}

			var type = o.GetType();

			if (!type.GetTypeInfo().IsGenericType)
			{
				return false;
			}

			if (type.GetTypeInfo().GetGenericTypeDefinition() != typeof(Dictionary<,>))
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Checks if a given object is a list.
		/// </summary>
		/// <param name="o">The object to check.</param>
		/// <returns>True if the object is a list, false otherwise.</returns>
		private static bool IsList(object o)
		{
			if (o == null)
			{
				return false;
			}

			var type = o.GetType();

			if (!type.GetTypeInfo().IsGenericType)
			{
				return false;
			}

			if (type.GetTypeInfo().GetGenericTypeDefinition() != typeof(List<>))
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Checks if a given object is an array.
		/// </summary>
		/// <param name="o">The object to check.</param>
		/// <returns>True if the object is an array, false otherwise.</returns>
		private static bool IsArray(object o)
		{
			if (o == null)
			{
				return false;
			}

			var type = o.GetType();
			return type.IsArray;
		}

		/// <summary>
		/// Checks if a given object is an enum. Note that nullable enums count as enums.
		/// </summary>
		/// <param name="o">The object to check.</param>
		/// <returns>True if the object is an enum (nullable or not), false otherwise.</returns>
		private static bool IsEnum(object o)
		{
			if (o == null)
			{
				return false;
			}

			var type = o.GetType();

			if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = Nullable.GetUnderlyingType(type);
			}

			if (!type.GetTypeInfo().IsEnum)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Computes the new key prefix, given a key and an existing prefix (if any).
		/// E.g. if the key is `bar` and the existing prefix is `foo`, then `foo[bar]` is returned.
		/// If a key already contains nested values, then only the non-nested part is nested under
		/// the prefix, e.g. if the key is `bar[baz]` and the prefix is `foo`, then `foo[bar][baz]`
		/// is returned.
		/// If no prefix is provided, the key is returned unchanged.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="keyPrefix">The existing key prefix, if any.</param>
		/// <returns>The new key prefix.</returns>
		private static string NewPrefix(string key, string keyPrefix)
		{
			if (String.IsNullOrEmpty(keyPrefix))
			{
				return key;
			}
			else
			{
				var i = key.IndexOf("[", StringComparison.Ordinal);
				if (i == -1)
				{
					return $"{keyPrefix}[{key}]";
				}
				else
				{
					return $"{keyPrefix}[{key.Substring(0, i)}]{key.Substring(i)}";
				}
			}
		}

		/// <summary>
		/// Represents a parameter in a query string, i.e. a key/value pair.
		/// </summary>
		internal sealed class Parameter
		{
			public Parameter(string key, string value)
			{
				Key = key;
				Value = value;
			}

			public string Key { get; }

			public string Value { get; }
		}
	}

	[JsonConverter(typeof(SourceConverter))]
	public class Source : StripeEntityWithId
	{
		public SourceType Type { get; set; }

		public StripeDeleted Deleted { get; set; }

		public StripeCard Card { get; set; }
		public StripeBankAccount BankAccount { get; set; }
	}

	public enum SourceType
	{
		Card,
		BankAccount,
		Deleted,
	}

	public class StripeAccountAdditionalOwner : INestedOptions
	{
		[JsonProperty("[address][city]")]
		public string CityOrTown { get; set; }

		[JsonProperty("[address][country]")]
		public string Country { get; set; }

		[JsonProperty("[address][line1]")]
		public string Line1 { get; set; }

		[JsonProperty("[address][line2]")]
		public string Line2 { get; set; }

		[JsonProperty("[address][postal_code]")]
		public string PostalCode { get; set; }

		[JsonProperty("[address][state]")]
		public string State { get; set; }

		[JsonProperty("[dob][day]")]
		public int? BirthDay { get; set; }

		[JsonProperty("[dob][month]")]
		public int? BirthMonth { get; set; }

		[JsonProperty("[dob][year]")]
		public int? BirthYear { get; set; }

		[JsonProperty("[first_name]")]
		public string FirstName { get; set; }

		[JsonProperty("[last_name]")]
		public string LastName { get; set; }
	}

	public class StripeCard : StripeEntityWithId
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		public string AccountId { get; set; }

		[JsonProperty("address_city")]
		public string AddressCity { get; set; }

		[JsonProperty("address_country")]
		public string AddressCountry { get; set; }

		[JsonProperty("address_line1")]
		public string AddressLine1 { get; set; }

		[JsonProperty("address_line1_check")]
		public string AddressLine1Check { get; set; }

		[JsonProperty("address_line2")]
		public string AddressLine2 { get; set; }

		[JsonProperty("address_state")]
		public string AddressState { get; set; }

		[JsonProperty("address_zip")]
		public string AddressZip { get; set; }

		[JsonProperty("address_zip_check")]
		public string AddressZipCheck { get; set; }

		[JsonProperty("available_payout_methods")]
		public string[] AvailablePayoutMethods { get; set; }

		[JsonProperty("brand")]
		public string Brand { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		public string CustomerId { get; set; }

		[JsonIgnore]
		public StripeCustomer Customer { get; set; }

		[JsonProperty("customer")]
		internal object InternalCustomer
		{
			set
			{
				StringOrObject<StripeCustomer>.Map(value, s => CustomerId = s, o => Customer = o);
			}
		}

		[JsonProperty("cvc_check")]
		public string CvcCheck { get; set; }

		[JsonProperty("default_for_currency")]
		public bool DefaultForCurrency { get; set; }

		[JsonProperty("dynamic_last4")]
		public string DynamicLast4 { get; set; }

		[JsonProperty("exp_month")]
		public int ExpirationMonth { get; set; }

		[JsonProperty("exp_year")]
		public int ExpirationYear { get; set; }

		[JsonProperty("fingerprint")]
		public string Fingerprint { get; set; }

		[JsonProperty("funding")]
		public string Funding { get; set; }

		[JsonProperty("last4")]
		public string Last4 { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		public string RecipientId { get; set; }

		[JsonProperty("three_d_secure")]
		public string ThreeDSecure { get; set; }

		[JsonProperty("tokenization_method")]
		public string TokenizationMethod { get; set; }

		[JsonProperty("source")]
		public string SourceToken { get; set; }
	}

	public class StripeCharge : StripeEntityWithId
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("amount_refunded")]
		public int AmountRefunded { get; set; }

		[JsonProperty("captured")]
		public bool? Captured { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		public string CustomerId { get; set; }

		[JsonIgnore]
		public StripeCustomer Customer { get; set; }

		[JsonProperty("customer")]
		internal object InternalCustomer
		{
			set
			{
				StringOrObject<StripeCustomer>.Map(value, s => CustomerId = s, o => Customer = o);
			}
		}

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("failure_code")]
		public string FailureCode { get; set; }

		[JsonProperty("failure_message")]
		public string FailureMessage { get; set; }

		[JsonProperty("fraud_details")]
		public Dictionary<string, string> FraudDetails { get; set; }

		public string InvoiceId { get; set; }

		[JsonProperty("livemode")]
		public bool LiveMode { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }

		public string OnBehalfOfId { get; set; }

		[JsonProperty("paid")]
		public bool Paid { get; set; }

		[JsonProperty("receipt_email")]
		public string ReceiptEmail { get; set; }

		[JsonProperty("receipt_number")]
		public string ReceiptNumber { get; set; }

		[JsonProperty("refunded")]
		public bool Refunded { get; set; }

		public string ReviewId { get; set; }

		[JsonProperty("source")]
		public Source Source { get; set; }

		[JsonProperty("statement_descriptor")]
		public string StatementDescriptor { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("transfer_group")]
		public string TransferGroup { get; set; }
	}

	public static class StripeConfiguration
	{

		private static string apiBase;

		/// <summary>
		/// If this isn't the latest version of the Stripe API, it's news to me.
		/// </summary>
		public static string StripeApiVersion = "2016-07-06";
		public static string StripeNetVersion { get; private set; }

		/// <summary>
		/// This option allows you to provide your own HttpMessageHandler. Useful with Android/iPhone.
		/// </summary>
		public static HttpMessageHandler HttpMessageHandler { get; set; }

		private static string _apiKey;

		static StripeConfiguration()
		{
			StripeNetVersion = new AssemblyName(typeof(Requestor).GetTypeInfo().Assembly.FullName).Version.ToString(3);
		}

		internal static string GetApiKey()
		{
			if (String.IsNullOrEmpty(_apiKey))
			{
#if NET45
                _apiKey = System.Configuration.ConfigurationManager.AppSettings["StripeApiKey"];
#endif
			}

			return _apiKey;
		}

		public static void SetApiKey(string newApiKey)
		{
			_apiKey = newApiKey;
		}

		internal static string GetApiBase()
		{
			if (String.IsNullOrEmpty(apiBase))
			{
				apiBase = Urls.BaseUrl;
			}

			return apiBase;
		}
	}

	public class StripeCustomer : StripeEntityWithId
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("account_balance")]
		public int AccountBalance { get; set; }

		[JsonProperty("business_vat_id")]
		public string BusinessVatId { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("deleted")]
		public bool? Deleted { get; set; }

		public string DefaultCustomerBankAccountId { get; set; }

		public string DefaultSourceId { get; set; }

		[JsonIgnore]
		public Source DefaultSource { get; set; }

		[JsonProperty("default_source_type")]
		public string DefaultSourceType { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("sources")]
		public StripeList<Source> Sources { get; set; }

		[JsonProperty("source")]
		public string SourceToken { get; set; }
	}

	public class StripeError : StripeEntity
	{
		[JsonProperty("type")]
		public string ErrorType { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("param")]
		public string Parameter { get; set; }

		[JsonProperty("error")]
		public string Error { get; set; }

		[JsonProperty("error_description")]
		public string ErrorSubscription { get; set; }

		[JsonProperty("charge")]
		public string ChargeId { get; set; }

		[JsonProperty("decline_code")]
		public string DeclineCode { get; set; }
	}

	public class StripeToken : StripeEntityWithId
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("livemode")]
		public bool LiveMode { get; set; }

		[JsonProperty("created")]
		[JsonConverter(typeof(StripeDateTimeConverter))]
		public DateTime? Created { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("used")]
		public bool? Used { get; set; }

		[JsonProperty("bank_account[id]")]
		public string BankAccountId { get; set; }

		[JsonProperty("bank_account[object]")]
		public string BankAccountObject { get; set; }

		[JsonProperty("bank_account[country]")]
		public string BankAccountCountry { get; set; }

		[JsonProperty("bank_account[currency]")]
		public string BankAccountCurrency { get; set; }

		[JsonProperty("bank_account[last4]")]
		public string BankAccountLast4 { get; set; }

		[JsonProperty("bank_account[status]")]
		public string BankAccountStatus { get; set; }

		[JsonProperty("bank_account[bank_name]")]
		public string BankAccountName { get; set; }

		[JsonProperty("bank_account[fingerprint]")]
		public string BankAccountFingerprint { get; set; }

		[JsonProperty("bank_account[routing_number]")]
		public string BankAccountRoutingNumber { get; set; }

		[JsonProperty("card")]
		public StripeCard StripeCard { get; set; }

		[Obsolete("This property is not valid on tokens and will be removed in a later version.")]
		[JsonProperty("description")]
		public string Description { get; set; }
	}

	internal static class Urls
	{
		public static string Invoices => BaseUrl + "/invoices";

		public static string InvoiceItems => BaseUrl + "/invoiceitems";

		public static string Tokens => BaseUrl + "/tokens";

		public static string Charges => BaseUrl + "/charges";

		public static string Coupons => BaseUrl + "/coupons";

		public static string Plans => BaseUrl + "/plans";

		public static string Balance => BaseUrl + "/balance";

		public static string BalanceTransactions => BaseUrl + "/balance/history";

		public static string Customers => BaseUrl + "/customers";

		public static string CustomerSources => BaseUrl + "/customers/{0}/sources";

		public static string CountrySpecs => BaseUrl + "/country_specs";

		public static string Disputes => BaseUrl + "/disputes";

		public static string RecipientCards => BaseUrl + "/recipients/{0}/cards";

		public static string Events => BaseUrl + "/events";

		public static string Accounts => BaseUrl + "/accounts";

		public static string Recipients => BaseUrl + "/recipients";

		public static string Subscriptions => BaseUrl + "/customers/{0}/subscriptions";

		public static string Transfers => BaseUrl + "/transfers";

		public static string ApplicationFees => BaseUrl + "/application_fees";

		internal static string BaseUrl => "https://api.stripe.com/v1";

		public static string OAuthToken => BaseConnectUrl + "/oauth/token";

		public static string OAuthDeauthorize => BaseConnectUrl + "/oauth/deauthorize";

		private static string BaseConnectUrl => "https://connect.stripe.com";

		private static string BaseUploadsUrl => "https://uploads.stripe.com/v1";

		public static string FileUploads => BaseUploadsUrl + "/files";
	}

	internal static class EpochTime
	{
		private static readonly DateTime _epochStartDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static DateTime ConvertEpochToDateTime(long seconds)
		{
			return _epochStartDateTime.AddSeconds(seconds);
		}

		public static long ConvertDateTimeToEpoch(this DateTime datetime)
		{
			if (datetime < _epochStartDateTime) return 0;

			return Convert.ToInt64(datetime.Subtract(_epochStartDateTime).TotalSeconds);
		}
	}

	internal static class MimeTypes
	{
		public static string GetMimeType(string fileName)
		{
			switch (Path.GetExtension(fileName.ToLower()))
			{
				case ".jpeg":
				case ".jpg":
					return "image/jpeg";

				case ".pdf":
					return "application/pdf";

				case ".png":
					return "image/png";

				default:
					return null;
			}
		}
	}

	public class StripeDateFilter
	{
		[JsonProperty("")]
		public DateTime? EqualTo { get; set; }

		[JsonProperty("[gt]")]
		public DateTime? GreaterThan { get; set; }

		[JsonProperty("[gte]")]
		public DateTime? GreaterThanOrEqual { get; set; }

		[JsonProperty("[lt]")]
		public DateTime? LessThan { get; set; }

		[JsonProperty("[lte]")]
		public DateTime? LessThanOrEqual { get; set; }
	}

	public class StripeResponse
	{
		public string ResponseJson { get; set; }
		public string ObjectJson { get; set; }
		public string RequestId { get; set; }
		public DateTime RequestDate { get; set; }
	}

	////public class StripeException : Exception
	////{
	////    public HttpStatusCode HttpStatusCode { get; set; }
	////    public StripeError StripeError { get; set; }

	////    public StripeException()
	////    {
	////    }

	////    public StripeException(HttpStatusCode httpStatusCode, StripeError stripeError, string message) : base(message)
	////    {
	////        HttpStatusCode = httpStatusCode;
	////        StripeError = stripeError;
	////    }
	////}

	public class StripeRefund : StripeEntityWithId
	{
		[JsonProperty("id")]
		public new string Id { get; set; }

		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("amount")]
		public long Amount { get; set; }

		#region Expandable Balance Transaction
		[JsonIgnore]
		public string BalanceTransactionId { get; set; }

		[JsonIgnore]
		public StripeBalanceTransaction BalanceTransaction { get; set; }

		[JsonProperty("balance_transaction")]
		internal object InternalBalanceTransaction
		{
			get
			{
				return BalanceTransaction ?? (object)BalanceTransactionId;
			}

			set
			{
				StringOrObject<StripeBalanceTransaction>.Map(value, s => BalanceTransactionId = s, o => BalanceTransaction = o);
			}
		}
		#endregion

		#region Expandable Charge
		[JsonIgnore]
		public string ChargeId { get; set; }

		[JsonIgnore]
		public StripeCharge Charge { get; set; }

		[JsonProperty("charge")]
		internal object InternalCharge
		{
			get
			{
				return Charge ?? (object)ChargeId;
			}

			set
			{
				StringOrObject<StripeCharge>.Map(value, s => ChargeId = s, o => Charge = o);
			}
		}
		#endregion

		[JsonProperty("created")]
		[JsonConverter(typeof(StripeDateTimeConverter))]
		public DateTime Created { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		#region Expandable Failure Balance Transaction
		[JsonIgnore]
		public string FailureBalanceTransactionId { get; set; }

		[JsonIgnore]
		public StripeBalanceTransaction FailureBalanceTransaction { get; set; }

		[JsonProperty("failure_balance_transaction")]
		internal object InternalFailureBalanceTransaction
		{
			get
			{
				return FailureBalanceTransaction ?? (object)FailureBalanceTransactionId;
			}

			set
			{
				StringOrObject<StripeBalanceTransaction>.Map(value, s => FailureBalanceTransactionId = s, o => FailureBalanceTransaction = o);
			}
		}
		#endregion

		[JsonProperty("failure_reason")]
		public string FailureReason { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }

		[JsonProperty("reason")]
		public string Reason { get; set; }

		[JsonProperty("receipt_number")]
		public string ReceiptNumber { get; set; }


		[JsonProperty("status")]
		public string Status { get; set; }

	}

	internal static class ExpandableProperty<T> where T : StripeEntityWithId
	{
		public static void Map(object value, Action<string> updateId, Action<T> updateObject)
		{
			if (value is JObject)
			{
				var item = ((JToken)value).ToObject<T>();
				updateId(item.Id);
				updateObject(item);
			}
			else if (value is string)
			{
				updateId((string)value);
				updateObject(null);
			}
		}
	}

	public class StripeBalanceTransaction : StripeEntityWithId
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("available_on")]
		[JsonConverter(typeof(StripeDateTimeConverter))]
		public DateTime AvailableOn { get; set; }

		[JsonProperty("created")]
		[JsonConverter(typeof(StripeDateTimeConverter))]
		public DateTime Created { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("fee")]
		public int Fee { get; set; }

		[JsonProperty("fee_details")]
		public List<StripeFee> FeeDetails { get; set; }

		[JsonProperty("net")]
		public int Net { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("source")]
		public string Source { get; set; }

		[JsonProperty("sourced_transfers")]
		public StripeList<StripeTransfer> SourcedTransfers { get; set; }
	}

	public class StripeFee : StripeEntity
	{
		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("application")]
		public string Application { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }
	}

	public class StripeTransfer : StripeEntityWithId
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("livemode")]
		public bool LiveMode { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }

		#region Expandable Application Fee
		public string ApplicationFeeId { get; set; }

		[JsonIgnore]
		public StripeApplicationFee ApplicationFee { get; set; }

		[JsonProperty("application_fee")]
		internal object InternalApplicationFee
		{
			set
			{
				ExpandableProperty<StripeApplicationFee>.Map(value, s => ApplicationFeeId = s, o => ApplicationFee = o);
			}
		}
		#endregion

		[JsonProperty("created")]
		[JsonConverter(typeof(StripeDateTimeConverter))]
		public DateTime Created { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("date")]
		[JsonConverter(typeof(StripeDateTimeConverter))]
		public DateTime Date { get; set; }

		[JsonProperty("reversals")]
		public StripeList<StripeTransferReversal> StripeTransferReversalList { get; set; }

		[JsonProperty("reversed")]
		public bool Reversed { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("amount_reversed")]
		public int AmountReversed { get; set; }

		public string BalanceTransactionId { get; set; }
		public StripeBalanceTransaction BalanceTransaction { get; set; }

		[JsonProperty("balance_transaction")]
		internal object InternalBalanceTransaction
		{
			set
			{
				ExpandableProperty<StripeBalanceTransaction>.Map(value, s => BalanceTransactionId = s, o => BalanceTransaction = o);
			}
		}

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("destination")]
		public string Destination { get; set; }

		[JsonProperty("destination_payment")]
		public string DestinationPayment { get; set; }

		[JsonProperty("failure_code")]
		public string FailureCode { get; set; }

		[JsonProperty("failure_message")]
		public string FailureMessage { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }

		[JsonProperty("bank_account")]
		public StripeBankAccount StripeBankAccount { get; set; }

		[JsonProperty("card")]
		public StripeCard Card { get; set; }

		[JsonProperty("source_transaction")]
		public string SourceTransactionId { get; set; }

		[JsonProperty("source_type")]
		public string SourceType { get; set; }

		#region Recipient (Obsolete)

		[Obsolete("Recipients are deprecated. Use Destination or Connect instead.")]
		public string RecipientId { get; set; }

		[Obsolete("Recipients are deprecated. Use Destination or Connect instead.")]
		[JsonIgnore]
		public StripeRecipient Recipient { get; set; }

		[JsonProperty("recipient")]
		internal object InternalRecipient
		{
#pragma warning disable CS0618 // Type or member is obsolete
			set => ExpandableProperty<StripeRecipient>.Map(value, s => RecipientId = s, o => Recipient = o);
#pragma warning restore CS0618 // Type or member is obsolete
		}

		#endregion

		[JsonProperty("statement_descriptor")]
		public string StatementDescriptor { get; set; }
	}

	public class StripeBankAccount : StripeEntityWithId
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("account")]
		public string AccountId { get; set; }

		[JsonProperty("account_holder_type")]
		public string AccountHolderType { get; set; }

		[JsonProperty("account_holder_name")]
		public string AccountHolderName { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("default_for_currency")]
		public bool DefaultForCurrency { get; set; }

		[JsonProperty("last4")]
		public string Last4 { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }

		[JsonProperty("bank_name")]
		public string BankName { get; set; }

		[JsonProperty("fingerprint")]
		public string Fingerprint { get; set; }

		[JsonProperty("routing_number")]
		public string RoutingNumber { get; set; }
	}

	public class StripeRecipient : StripeEntityWithId
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("livemode")]
		public bool LiveMode { get; set; }

		[JsonProperty("created")]
		[JsonConverter(typeof(StripeDateTimeConverter))]
		public DateTime Created { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("active_account")]
		public StripeRecipientActiveAccount ActiveAccount { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("cards")]
		public StripeList<StripeCard> StripeCardList { get; set; }

		[JsonProperty("verified")]
		public bool Verified { get; set; }

		#region Expandable Default Card
		public string StripeDefaultCardId { get; set; }

		[JsonIgnore]
		public StripeCard StripeDefaultCard { get; set; }

		[JsonProperty("default_card")]
		internal object InternalDefaultCard
		{
			set
			{
				ExpandableProperty<StripeCard>.Map(value, s => StripeDefaultCardId = s, o => StripeDefaultCard = o);
			}
		}
		#endregion
	}

	public class StripeRecipientActiveAccount : StripeEntityWithId
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("default_for_currency")]
		public bool DefaultForCurrency { get; set; }

		[JsonProperty("last4")]
		public string Last4 { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("bank_name")]
		public string BankName { get; set; }

		[JsonProperty("fingerprint")]
		public string Fingerprint { get; set; }

		[JsonProperty("routing_number")]
		public string RoutingNumber { get; set; }

		[JsonProperty("disabled")]
		public bool? Disabled { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }

		[JsonProperty("validated")]
		public bool? Validated { get; set; }
	}

	public class StripeTransferReversal : StripeEntityWithId
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("created")]
		[JsonConverter(typeof(StripeDateTimeConverter))]
		public DateTime Created { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		// todo: this should be an expandable property
		[JsonProperty("balance_transaction")]
		public string BalanceTransactionId { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }

		// todo: this should be an expandable property
		[JsonProperty("transfer")]
		public string TransferId { get; set; }

		[JsonProperty("refund_application_fee")]
		public int? RefundApplicationFee { get; set; }
	}

	public class StripeApplicationFee : StripeEntityWithId
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("livemode")]
		public bool LiveMode { get; set; }

		#region Expandable Account
		public string AccountId { get; set; }

		[JsonIgnore]
		public StripeAccount Account { get; set; }

		[JsonProperty("account")]
		internal object InternalAccount
		{
			set
			{
				ExpandableProperty<StripeAccount>.Map(value, s => AccountId = s, o => Account = o);
			}
		}
		#endregion

		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("application")]
		public string ApplicationId { get; set; }

		#region Expandable Balance Transaction
		public string BalanceTransactionId { get; set; }

		[JsonIgnore]
		public StripeBalanceTransaction BalanceTransaction { get; set; }

		[JsonProperty("balance_transaction")]
		internal object InternalBalanceTransaction
		{
			set
			{
				ExpandableProperty<StripeBalanceTransaction>.Map(value, s => BalanceTransactionId = s, o => BalanceTransaction = o);
			}
		}
		#endregion

		#region Expandable Card
		public string CardId { get; set; }

		[JsonIgnore]
		public StripeCard Card { get; set; }

		[JsonProperty("card")]
		internal object InternalCard
		{
			set
			{
				ExpandableProperty<StripeCard>.Map(value, s => CardId = s, o => Card = o);
			}
		}
		#endregion

		#region Expandable Charge
		public string ChargeId { get; set; }

		[JsonIgnore]
		public StripeCharge Charge { get; set; }

		[JsonProperty("charge")]
		internal object InternalCharge
		{
			set
			{
				ExpandableProperty<StripeCharge>.Map(value, s => ChargeId = s, o => Charge = o);
			}
		}
		#endregion

		[JsonProperty("created")]
		[JsonConverter(typeof(StripeDateTimeConverter))]
		public DateTime Created { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("refunded")]
		public bool Refunded { get; set; }

		[JsonProperty("refunds")]
		public StripeList<StripeApplicationFeeRefund> StripeApplicationFeeRefundList { get; set; }

		[JsonProperty("amount_refunded")]
		public int AmountRefunded { get; set; }
	}

	public class StripeApplicationFeeRefund : StripeEntityWithId
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("created")]
		[JsonConverter(typeof(StripeDateTimeConverter))]
		public DateTime Created { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("balance_transaction")]
		public string BalanceTransaction { get; set; }

		[JsonProperty("fee")]
		public string Fee { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }
	}

	public class StripeAccount : StripeEntityWithId
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("business_logo")]
		public string BusinessLogoFileId { get; set; }

		[JsonProperty("business_name")]
		public string BusinessName { get; set; }

		[JsonProperty("business_primary_color")]
		public string BusinessPrimaryColor { get; set; }

		[JsonProperty("business_url")]
		public string BusinessUrl { get; set; }

		[JsonProperty("charges_enabled")]
		public bool ChargesEnabled { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("currencies_supported")]
		public string[] CurrenciesSupported { get; set; }

		[JsonProperty("debit_negative_balances")]
		public bool DebitNegativeBalances { get; set; }

		[JsonProperty("decline_charge_on")]
		public StripeDeclineChargeOn DeclineChargeOn { get; set; }

		[JsonProperty("default_currency")]
		public string DefaultCurrency { get; set; }

		[JsonProperty("details_submitted")]
		public bool DetailsSubmitted { get; set; }

		[JsonProperty("display_name")]
		public string DisplayName { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("external_accounts")]
		[JsonConverter(typeof(SourceListConverter))]
		public StripeList<Source> ExternalAccounts { get; set; }

		[JsonProperty("legal_entity")]
		public StripeLegalEntity LegalEntity { get; set; }

		[JsonProperty("managed")]
		public bool Managed { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }

		[JsonProperty("product_description")]
		public string ProductDescription { get; set; }

		[JsonProperty("statement_descriptor")]
		public string StatementDescriptor { get; set; }

		[JsonProperty("transfer_statement_descriptor")]
		public string TransferStatementDescriptor { get; set; }

		[JsonProperty("support_email")]
		public string SupportEmail { get; set; }

		[JsonProperty("support_phone")]
		public string SupportPhone { get; set; }

		[JsonProperty("support_url")]
		public string SupportUrl { get; set; }

		[JsonProperty("timezone")]
		public string Timezone { get; set; }

		[JsonProperty("tos_acceptance")]
		public StripeTermsOfServiceAcceptance TermsOfServiceAcceptance { get; set; }

		[JsonProperty("transfer_schedule")]
		public StripeTransferSchedule TransferSchedule { get; set; }

		[JsonProperty("transfers_enabled")]
		public bool TransfersEnabled { get; set; }

		[JsonProperty("verification")]
		public StripeAccountVerification AccountVerification { get; set; }

		[JsonProperty("keys")]
		public StripeManagedAccountKeys ManagedAccountKeys { get; set; }
	}

	public class StripeDeclineChargeOn : StripeEntity
	{
		[JsonProperty("avs_failure")]
		public bool AvsFailure { get; set; }

		[JsonProperty("cvc_failure")]
		public bool CvcFailure { get; set; }
	}

	internal class SourceListConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			throw new NotImplementedException();
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var incoming = JObject.FromObject(value);

			incoming.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var list = JObject.Load(reader).ToObject<StripeList<dynamic>>();

			var result = new StripeList<Source>
			{
				Data = new List<Source>(),
				HasMore = list.HasMore,
				Object = list.Object,
				Url = list.Url
			};

			foreach (var item in list.Data)
			{
				var source = new Source();

				if (item.SelectToken("object").ToString() == "bank_account")
				{
					source.Type = SourceType.BankAccount;
					source.BankAccount = Mapper<StripeBankAccount>.MapFromJson(item.ToString());
				}

				if (item.SelectToken("object").ToString() == "card")
				{
					source.Type = SourceType.Card;
					source.Card = Mapper<StripeCard>.MapFromJson(item.ToString());
				}

				result.Data.Add(source);
			}

			return result;
		}
	}

	public class StripeManagedAccountKeys : StripeEntity
	{
		[JsonProperty("secret")]
		public string Secret { get; set; }

		[JsonProperty("publishable")]
		public string Publishable { get; set; }
	}

	public class StripeAccountVerification : StripeEntity
	{
		[JsonProperty("disabled_reason")]
		public string DisabledReason { get; set; }

		[JsonProperty("due_by")]
		[JsonConverter(typeof(StripeDateTimeConverter))]
		public DateTime? DueBy { get; set; }

		[JsonProperty("fields_needed")]
		public string[] FieldsNeeded { get; set; }
	}

	public class StripeTransferSchedule : StripeEntity
	{
		[JsonProperty("delay_days")]
		public int DelayDays { get; set; }

		[JsonProperty("interval")]
		public string Interval { get; set; }

		[JsonProperty("monthly_anchor")]
		public int MonthlyAnchor { get; set; }

		[JsonProperty("weekly_anchor")]
		public string WeeklyAnchor { get; set; }
	}

	public class StripeTermsOfServiceAcceptance : StripeEntity
	{
		[JsonProperty("date")]
		[JsonConverter(typeof(StripeDateTimeConverter))]
		public DateTime? Date { get; set; }

		[JsonProperty("ip")]
		public string Ip { get; set; }

		[JsonProperty("user_agent")]
		public string UserAgent { get; set; }
	}

	public class StripeAdditionalOwners : StripeEntity
	{
		[JsonProperty("address")]
		public StripeAddress Address { get; set; }

		[JsonProperty("dob")]
		public StripeBirthDay BirthDay { get; set; }

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		[JsonProperty("verification")]
		public StripeLegalEntityVerification LegalEntityVerification { get; set; }
	}

	public class StripeLegalEntity : StripeEntity
	{
		[JsonProperty("additional_owners")]
		public List<StripeAdditionalOwners> AdditionalOwners { get; set; }

		[JsonProperty("address")]
		public StripeAddress Address { get; set; }

		[JsonProperty("business_name")]
		public string BusinessName { get; set; }

		[JsonProperty("dob")]
		public StripeBirthDay BirthDay { get; set; }

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("gender")]
		public string Gender { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		[JsonProperty("personal_address")]
		public StripeAddress PersonalAddress { get; set; }

		[JsonProperty("personal_id_number_provided")]
		public bool PersonalIdNumberProvided { get; set; }

		[JsonProperty("ssn_last_4_provided")]
		public bool SocialSecurityNumberLastFourProvided { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("verification")]
		public StripeLegalEntityVerification LegalEntityVerification { get; set; }
	}

	public class StripeAddress : StripeEntity
	{
		[JsonProperty("city")]
		public string CityOrTown { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("line1")]
		public string Line1 { get; set; }

		[JsonProperty("line2")]
		public string Line2 { get; set; }

		[JsonProperty("postal_code")]
		public string PostalCode { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }
	}

	public class StripeLegalEntityVerification : StripeEntity
	{
		[JsonProperty("details")]
		public string Details { get; set; }

		[JsonProperty("details_code")]
		public string DetailsCode { get; set; }

		#region Expandable Document
		public string DocumentId { get; set; }

		[JsonIgnore]
		public StripeFileUpload Document { get; set; }

		[JsonProperty("document")]
		internal object InternalDocument
		{
			set
			{
				ExpandableProperty<StripeFileUpload>.Map(value, s => DocumentId = s, o => Document = o);
			}
		}
		#endregion

		[JsonProperty("status")]
		public string Status { get; set; }
	}

	public class StripeFileUpload : StripeEntityWithId
	{
		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("created")]
		[JsonConverter(typeof(StripeDateTimeConverter))]
		public DateTime Created { get; set; }

		[JsonProperty("size")]
		public int Size { get; set; }

		[JsonProperty("purpose")]
		public string Purpose { get; set; }

		[JsonProperty("Url")]
		public string Url { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }
	}

	public class StripeBirthDay : StripeEntity
	{
		[JsonProperty("day")]
		public int? Day { get; set; }

		[JsonProperty("month")]
		public int? Month { get; set; }

		[JsonProperty("year")]
		public int? Year { get; set; }
	}

	public static class StripeRefundReasons
	{
		public const string Unknown = null;
		public const string Duplicate = "duplicate";
		public const string Fradulent = "fraudulent";
		public const string RequestedByCustomer = "requested_by_customer";
	}

	public class StripeRefundListOptions : StripeListOptions
	{
		[JsonProperty("charge")]
		public string ChargeId { get; set; }
	}

	public class StripeRefundUpdateOptions
	{
		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }
	}

	public class StripeListOptions
	{
		[JsonProperty("limit")]
		public int? Limit { get; set; }

		[JsonProperty("starting_after")]
		public string StartingAfter { get; set; }

		[JsonProperty("ending_before")]
		public string EndingBefore { get; set; }
	}

	public class StripeDeleted : StripeEntityWithId
	{
		[JsonProperty("deleted")]
		public bool Deleted { get; set; }
	}

	public abstract class StripeEntity
	{
		public StripeResponse StripeResponse { get; set; }
	}

	public abstract class StripeEntityWithId : StripeEntity
	{
		[JsonProperty("id")]
		public string Id { get; set; }
	}

	public class RequestOptions
	{
		public string ApiKey { get; set; }

		public string StripeConnectAccountId { get; set; }

		public string IdempotencyKey { get; set; }

		// This is used specifically for Ephemeral Keys as they have to be created
		// with a specific API version. It should not be used for anything else which
		// is why it is set as internal.
		internal string StripeVersion { get; set; }
	}

	public interface IStripeEntity
	{
		StripeResponse StripeResponse { get; set; }
	}

	public class ListOptions : BaseOptions
	{
		/// <summary>
		/// A limit on the number of objects to be returned, between 1 and 100.
		/// </summary>
		[JsonProperty("limit")]
		public long? Limit { get; set; }

		/// <summary>
		/// A cursor for use in pagination. <code>starting_after</code> is an object ID that defines
		/// your place in the list. For instance, if you make a list request and receive 100
		/// objects, ending with <code>obj_foo</code>, your subsequent call can include
		/// <code>starting_after=obj_foo</code> in order to fetch the next page of the list.
		/// </summary>
		[JsonProperty("starting_after")]
		public string StartingAfter { get; set; }

		/// <summary>
		/// A cursor for use in pagination. <code>ending_before</code> is an object ID that defines
		/// your place in the list. For instance, if you make a list request and receive 100
		/// objects, starting with <code>obj_bar</code>, your subsequent call can include
		/// <code>ending_before=obj_bar</code> in order to fetch the previous page of the list.
		/// </summary>
		[JsonProperty("ending_before")]
		public string EndingBefore { get; set; }
	}

	public interface IHasId
	{
		/// <summary>
		/// Unique identifier for the object.
		/// </summary>
		string Id { get; set; }
	}

	public class Service<EntityReturned>
		where EntityReturned : IStripeEntity
	{
		protected Service()
		{
		}

		protected Service(string apiKey)
		{
			ApiKey = apiKey;
		}

		public string ApiKey { get; set; }

		public string BasePath { get; }

		protected Task<EntityReturned> CreateEntityAsync(BaseOptions options, StripeRequestOptions requestOptions, CancellationToken cancellationToken)
		{
			return PostRequestAsync<EntityReturned>(ClassUrl(), options, requestOptions, cancellationToken);
		}

		protected Task<EntityReturned> DeleteEntityAsync(string id, BaseOptions options, StripeRequestOptions requestOptions, CancellationToken cancellationToken)
		{
			return DeleteRequestAsync<EntityReturned>(InstanceUrl(id), options, requestOptions, cancellationToken);
		}

		protected Task<EntityReturned> GetEntityAsync(string id, BaseOptions options, StripeRequestOptions requestOptions, CancellationToken cancellationToken)
		{
			return GetRequestAsync<EntityReturned>(InstanceUrl(id), options, requestOptions, false, cancellationToken);
		}

		protected Task<StripeList<EntityReturned>> ListEntitiesAsync(ListOptions options, StripeRequestOptions requestOptions, CancellationToken cancellationToken)
		{
			return GetRequestAsync<StripeList<EntityReturned>>(ClassUrl(), options, requestOptions, true, cancellationToken);
		}

		protected Task<EntityReturned> UpdateEntityAsync(string id, BaseOptions options, StripeRequestOptions requestOptions, CancellationToken cancellationToken)
		{
			return PostRequestAsync<EntityReturned>(InstanceUrl(id), options, requestOptions, cancellationToken);
		}

		protected async Task<T> DeleteRequestAsync<T>(string url, BaseOptions options, StripeRequestOptions requestOptions, CancellationToken cancellationToken)
		{
			return Mapper<T>.MapFromJson(
				await Requestor.DeleteAsync(
					this.ApplyAllParameters(options, url),
					SetupRequestOptions(requestOptions),
					cancellationToken).ConfigureAwait(false));
		}

		protected async Task<T> GetRequestAsync<T>(string url, BaseOptions options, StripeRequestOptions requestOptions, bool isListMethod, CancellationToken cancellationToken)
		{
			return Mapper<T>.MapFromJson(
				await Requestor.GetStringAsync(
					this.ApplyAllParameters(options, url, isListMethod),
					SetupRequestOptions(requestOptions),
					cancellationToken).ConfigureAwait(false));
		}


		protected async Task<T> PostRequestAsync<T>(string url, BaseOptions options, StripeRequestOptions requestOptions, CancellationToken cancellationToken)
		{
			return Mapper<T>.MapFromJson(
				await Requestor.PostStringAsync(
					this.ApplyAllParameters(options, url),
					SetupRequestOptions(requestOptions),
					cancellationToken).ConfigureAwait(false));
		}

		protected StripeRequestOptions SetupRequestOptions(StripeRequestOptions requestOptions)
		{
			if (requestOptions == null)
			{
				requestOptions = new StripeRequestOptions();
			}

			if (!String.IsNullOrEmpty(ApiKey))
			{
				requestOptions.ApiKey = ApiKey;
			}

			return requestOptions;
		}

		protected virtual string ClassUrl(string baseUrl = null)
		{
			baseUrl = baseUrl ?? StripeConfiguration.GetApiBase();
			return $"{baseUrl}{BasePath}";
		}

		protected virtual string InstanceUrl(string id, string baseUrl = null)
		{
			return $"{ClassUrl(baseUrl)}/{WebUtility.UrlEncode(id)}";
		}
	}

	public class BaseOptions : INestedOptions
	{
		private Dictionary<string, string> extraParams = new Dictionary<string, string>();
		private List<string> expand = new List<string>();

		public Dictionary<string, string> ExtraParams
		{
			get { return this.extraParams; }
			set { this.extraParams = value; }
		}

		public List<string> Expand
		{
			get { return this.expand; }
			set { this.expand = value; }
		}

		public void AddExtraParam(string key, string value)
		{
			ExtraParams.Add(key, value);
		}

		public void AddExpand(string value)
		{
			Expand.Add(value);
		}
	}

	public class StripeCustomerCreateOptions
	{
		[JsonProperty("account_balance")]
		public int? AccountBalance { get; set; }

		[JsonProperty("coupon")]
		public string CouponId { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }

		[JsonProperty("plan")]
		public string PlanId { get; set; }

		[JsonProperty("quantity")]
		public int? Quantity { get; set; }

		[JsonProperty("source")]
		public string SourceToken { get; set; }

		[JsonProperty("source")]
		public CardCreateNestedOptions SourceCard { get; set; }

		[JsonProperty("tax_percent")]
		public decimal? TaxPercent { get; set; }

		[JsonProperty("validate")]
		public bool? Validate { get; set; }

		#region Trial End

		public DateTime? TrialEnd { get; set; }

		public bool EndTrialNow { get; set; }

		[JsonProperty("trial_end")]
		internal string TrialEndInternal
		{
			get
			{
				if (EndTrialNow)
					return "now";
				else if (TrialEnd.HasValue)
					return EpochTime.ConvertDateTimeToEpoch(TrialEnd.Value).ToString();
				else
					return null;
			}
		}

		#endregion
	}

	public class StripeCustomerListOptions : StripeListOptions
	{
		[JsonProperty("created")]
		public StripeDateFilter Created { get; set; }
	}

	public class StripeCustomerUpdateOptions
	{
		[JsonProperty("account_balance")]
		public int? AccountBalance { get; set; }

		[JsonProperty("source")]
		public string SourceToken { get; set; }

		[JsonProperty("source")]
		public CardCreateNestedOptions SourceCard { get; set; }

		[JsonProperty("coupon")]
		public string Coupon { get; set; }

		[JsonProperty("default_source")]
		public string DefaultSource { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }
	}

	public class CardCreateNestedOptions : INestedOptions
	{
		/// <summary>
		/// The type of payment source. Should be "card".
		/// </summary>
		[JsonProperty("object")]
		internal string Object => "card";

		/// <summary>
		/// City/District/Suburb/Town/Village.
		/// </summary>
		[JsonProperty("address_city")]
		public string AddressCity { get; set; }

		/// <summary>
		/// Billing address country, if provided when creating card.
		/// </summary>
		[JsonProperty("address_country")]
		public string AddressCountry { get; set; }

		/// <summary>
		/// Address line 1 (Street address/PO Box/Company name).
		/// </summary>
		[JsonProperty("address_line1")]
		public string AddressLine1 { get; set; }

		/// <summary>
		/// Address line 2 (Apartment/Suite/Unit/Building).
		/// </summary>
		[JsonProperty("address_line2")]
		public string AddressLine2 { get; set; }

		/// <summary>
		/// State/County/Province/Region.
		/// </summary>
		[JsonProperty("address_state")]
		public string AddressState { get; set; }

		/// <summary>
		/// Zip/Postal Code.
		/// </summary>
		[JsonProperty("address_zip")]
		public string AddressZip { get; set; }

		/// <summary>
		/// USUALLY REQUIRED: Card security code. Highly recommended to always include this value, but it's only required for accounts based in European countries.
		/// </summary>
		[JsonProperty("cvc")]
		public string Cvc { get; set; }

		/// <summary>
		/// REQUIRED: Two digit number representing the card's expiration month.
		/// </summary>
		[JsonProperty("exp_month")]
		public long? ExpMonth { get; set; }

		/// <summary>
		/// REQUIRED: Two or four digit number representing the card's expiration year.
		/// </summary>
		[JsonProperty("exp_year")]
		public long? ExpYear { get; set; }

		/// <summary>
		/// A set of key/value pairs that you can attach to a card object. It can be useful for storing additional information about the card in a structured format.
		/// </summary>
		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }

		/// <summary>
		/// Cardholder's full name.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// REQUIRED: The card number, as a string without any separators.
		/// </summary>
		[JsonProperty("number")]
		public string Number { get; set; }
	}
}