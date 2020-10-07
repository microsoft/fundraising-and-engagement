using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentProcessors;
using PaymentProcessors.Models;
using Newtonsoft.Json;

namespace FundraisingandEngagement.Tests.PaymentProcessors
{
	public class PaymentProcessorTestBase
	{
		private readonly PaymentProcessorType paymentGatewayCode;
		private readonly IServiceProvider serviceProvider;

		private string? GatewayName => Enum.GetName(typeof(PaymentProcessorType), this.paymentGatewayCode);

		private string GetJsonDataPath() => $"{Directory.GetCurrentDirectory()}\\PaymentProcessors\\Data\\{GatewayName}\\";

		public PaymentProcessorTestBase(PaymentProcessorType paymentGatewayCode)
		{
			this.paymentGatewayCode = paymentGatewayCode;

			var services = new ServiceCollection();
			services.AddPaymentProcessors();
			this.serviceProvider = services.BuildServiceProvider();
		}

		protected async Task ProcessDonation_ReturnsPaymentGatewayResponse<TFactoryInput>(bool isRecurring) where TFactoryInput : PaymentInput
		{
			var jsonName = isRecurring ? "donationRecurring.json" : "donation.json";
			var input = LoadJsonSingleObject<TFactoryInput>(jsonName);

			await ProcessDonation_ReturnsPaymentGatewayResponse(input);
		}

		protected async Task ProcessDonation_ReturnsPaymentGatewayResponse<TFactoryInput>(TFactoryInput input) where TFactoryInput : PaymentInput
		{
			var paymentProcessorGateway = this.serviceProvider.GetRequiredService<IPaymentProcessorGateway>();

			// Act
			var response = await paymentProcessorGateway.MakePaymentAsync(input);

			// Assert
			StringAssert.Contains(response.TransactionResult, GetSuccessMessage());
			Assert.IsTrue(response.IsSuccessful, response.TransactionResult);
		}

		protected async Task RecurringDonation_ReturnsPaymentGatewayResponse<TRecurringInput>(bool goodInput) where TRecurringInput : RecurringPaymentInput
		{
			var jsonName = goodInput ? "InputGood.json" : "InputBad.json";
			var input = LoadJsonSingleObject<TRecurringInput>(jsonName);

			await RecurringDonation_ReturnsPaymentGatewayResponse(goodInput, input);
		}

		protected async Task RecurringDonation_ReturnsPaymentGatewayResponse<TRecurringInput>(bool goodInput, TRecurringInput input) where TRecurringInput : RecurringPaymentInput
		{
			var paymentProcessorGateway = this.serviceProvider.GetRequiredService<IPaymentProcessorGateway>();

			try
			{
				var response = await paymentProcessorGateway.MakePreAuthorizedPaymentAsync(input);
				Assert.AreEqual(response.IsSuccessful, goodInput, response.TransactionResult);
			}
			catch (Exception e)
			{
				Assert.IsFalse(goodInput, e.Message);
			}
		}

		private string GetSuccessMessage()
		{
			var type = typeof(PaymentProcessorType);
			var name = Enum.GetName(type, this.paymentGatewayCode)!;
			var memberInfo = type.GetMember(name);
			return memberInfo[0].GetCustomAttribute<PaymentProcessorOutcomeAttribute>()?.SuccessMessage ?? String.Empty;
		}

		protected T LoadJsonSingleObject<T>(string fileName)
		{
			var json = LoadJsonFile(fileName);
			return JsonConvert.DeserializeObject<T>(json);
		}

		protected string LoadJsonFile(string fileName) => File.ReadAllText(GetJsonDataPath() + fileName);
	}
}
