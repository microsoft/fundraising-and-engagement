using System;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;

namespace PaymentDriver.Utils
{
	internal static class DefaultData
	{
		internal static TransactionCurrency TransactionCurrency => new TransactionCurrency
		{
			TransactionCurrencyId = Guid.NewGuid(),
			SyncDate = DateTime.Now,
			CreatedOn = DateTime.Now,
			CurrencyName = "Canadian Dollar",
			CurrencySymbol = "$",
			IsoCurrencyCode = "CAD",
			StateCode = 0,
			StatusCode = StatusCode.Active
		};

		internal static PaymentProcessor MonerisPaymentProcessor => new PaymentProcessor
		{
			PaymentProcessorId = Guid.NewGuid(),
			SyncDate = DateTime.Now,
			CreatedOn = DateTime.Now,
			Deleted = false,
			Name = "Moneris Load Test",
			Identifier = "Moneris Load Test",
			PaymentGatewayType = PaymentGatewayCode.Moneris,
			MonerisTestMode = true,
			MonerisApiKey = "yesguy",
			MonerisStoreId = "store5",
		};

		internal static PaymentProcessor StripePaymentProcessor => new PaymentProcessor
		{
			PaymentProcessorId = Guid.NewGuid(),
			SyncDate = DateTime.Now,
			CreatedOn = DateTime.Now,
			Deleted = false,
			Name = "Stripe Load Test",
			Identifier = "Stripe Load Test",
			PaymentGatewayType = PaymentGatewayCode.Stripe,
			MonerisTestMode = true,
			MonerisApiKey = "sk_test_yTS3swz0tSk977K7rCYK3yZP",
			StripeServiceKey = "sk_test_yTS3swz0tSk977K7rCYK3yZP"
		};

		internal static PaymentProcessor IatsPaymentProcessor => new PaymentProcessor
		{
			PaymentProcessorId = Guid.NewGuid(),
			SyncDate = DateTime.Now,
			CreatedOn = DateTime.Now,
			Deleted = false,
			Name = "Iats Load Test",
			Identifier = "Iats Load Test",
			PaymentGatewayType = PaymentGatewayCode.Iats,
			MonerisTestMode = true,
			IatsAgentCode = "TE8188",
			IatsPassword = "mcrmx2020",
		};

		internal static Configuration Configuration(PaymentProcessor processor) => new Configuration
		{
			ConfigurationId = Guid.NewGuid(),
			PaymentProcessorId = processor.PaymentProcessorId,
			CharityTitle = $"{processor.Name} Configuration",
			Identifier = processor.Identifier,
			StatusCode = StatusCode.Active,
			ScheRecurrenceStart = 844060001,
			ScheRetryinterval = 5,
			ScheMaxRetries = 5,
			CreatedOn = DateTime.Now,
			SyncDate = processor.SyncDate
		};
	}
}
