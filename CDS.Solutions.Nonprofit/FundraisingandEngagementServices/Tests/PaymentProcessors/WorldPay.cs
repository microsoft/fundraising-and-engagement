using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentProcessors.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FundraisingandEngagement.Tests.PaymentProcessors
{
	[TestClass]
	public class WorldPay : PaymentProcessorTestBase
	{
		public WorldPay() : base(PaymentProcessorType.WorldPay)
		{
		}

		[TestMethod]
		public async Task ProcessDonation()
		{
			var input = new WorldPayCreditCardPaymentInput
			{
				IsoCurrencyCode = "CAD",
				ServiceKey = "T_S_09fdbfe3-1651-4813-bb43-b5eb73ed7409",
				FirstName = "John",
				LastName = "MacFlip",
				WorldPayToken = "TEST_RU_90b3bd09-b2b8-4ad4-acb5-4453224a6d69",
				EmailAddress = "12@12.com",
				IsBankProcess = false,
				BillingCity = "Victoria Harbour",
				CcExpMmYy = "1221",
				CreditCardNo = "4444333322221111",
				Amount = 100,
				BillingPostalCode = "R0I 2A0",
				BillingLine1 = "1 Tay St.",
				IsRecurring = false,
			};

			input.WorldPayToken = await GetWorldPayToken();

			await ProcessDonation_ReturnsPaymentGatewayResponse(input);
		}

		[TestMethod]
		public async Task Recurring_ProcessDonation()
		{
			await ProcessDonation_ReturnsPaymentGatewayResponse<WorldPayCreditCardPaymentInput>(isRecurring: true);
		}

		[TestMethod]
		public async Task WorldPay_Succeeds_ProcessCreditCard_Returns_PaymentGatewayOutput()
		{
			var input = new WorldPayRecurringPaymentInput
			{
				InvoiceIdentifier = "MISC015E32",
				ServiceKey = "T_S_1a203d1c-3084-491f-96b7-de6336ae25fd",
				WorldPayToken = "TEST_RU_505486e6-ed80-4ec8-82db-d82f73e2b005",
				FirstName = "WorldPay",
				LastName = "Testing",
				Address1 = "WorldPay",
				Address2 = "Street2",
				City = "City",
				PostalCode = "ZIP",
				Amount = 10.0m,
				IsoCurrencyCode = "CAD",
				NextFailedRetry = new DateTime(0001, 01, 01),
				CurrentRetry = 0,
				RetryInterval = 5,
				MaxRetries = 5,
			};

			await RecurringDonation_ReturnsPaymentGatewayResponse(true, input);
		}

		[TestMethod]
		public async Task WorldPay_Rejected_ProcessCreditCard_Returns_PaymentGatewayOutput()
		{
			await RecurringDonation_ReturnsPaymentGatewayResponse<WorldPayRecurringPaymentInput>(false);
		}

		private async Task<string> GetWorldPayToken()
		{
			var jsonInString = LoadJsonFile("client.json");
			var client = new HttpClient();
			var uri = "https://api.worldpay.com/v1/tokens";
			var content = new StringContent(jsonInString, Encoding.UTF8, "application/json");
			var response = await client.PostAsync(uri, content);
			var responseContent = await response.Content.ReadAsStringAsync();
			return GetKeyValue(responseContent, "token");
		}

		private static string GetKeyValue(string json, string key)
		{
			var data = (JObject)JsonConvert.DeserializeObject(json)!;
			return data[key]!.Value<string>();
		}
	}
}
