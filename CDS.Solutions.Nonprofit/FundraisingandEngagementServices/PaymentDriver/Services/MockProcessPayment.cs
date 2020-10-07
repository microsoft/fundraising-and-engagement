using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PaymentDriver.Utils;
using PaymentProcessors;
using PaymentProcessors.Models;

namespace PaymentDriver.Services
{
	internal class MockProcessPayment : IPaymentProcessorGateway
	{
		private readonly string[] monerisTokens;
		private readonly StripeToken[] stripeTokens;
		private readonly string[] iatsTokens;

		public MockProcessPayment()
		{
			var directory = Directory.GetCurrentDirectory();
			var tokenJsonStr = FileUtils.ReadFile($"{directory}//MonerisTokens.json");
			var tokenList = JsonConvert.DeserializeObject<TokenList>(tokenJsonStr);
			this.monerisTokens = tokenList.Tokens;

			tokenJsonStr = FileUtils.ReadFile($"{directory}//StripeTokens.json");
			var stripeTokenList = JsonConvert.DeserializeObject<StripeTokenList>(tokenJsonStr);
			this.stripeTokens = stripeTokenList.Tokens;

			tokenJsonStr = FileUtils.ReadFile($"{directory}//IatsTokens.json");
			tokenList = JsonConvert.DeserializeObject<TokenList>(tokenJsonStr);
			this.iatsTokens = tokenList.Tokens;
		}

		public Task<PaymentOutput> MakePaymentAsync(PaymentInput input, CancellationToken token = default)
		{
			var result = new MockPaymentOutput
			{
				IsSuccessful = true,
				CardType = "Visa"
			};

			var random = new Random();
			var randomIndex = 0;
			switch (input)
			{
				case MonerisCreditCardPaymentInput i:
					randomIndex = random.Next(0, this.monerisTokens.Length);
					result.SetAuth(this.monerisTokens[randomIndex]);
					result.TransactionResult = "APPROVED";
					break;
				case IatsCreditCardPaymentInput i:
					randomIndex = random.Next(0, this.iatsTokens.Length);
					result.SetAuth(this.iatsTokens[randomIndex]);
					result.TransactionResult = "OK";
					break;
				case StripeCreditCardPaymentInput i:
					randomIndex = random.Next(0, this.stripeTokens.Length);
					var tokenItem = this.stripeTokens[randomIndex];
					result.SetAuth(tokenItem.AuthToken);
					result.StripeCustomerId = tokenItem.StripeCustomerId;
					result.TransactionResult = "succeeded";
					break;
				default: break;
			}

			return Task.FromResult((PaymentOutput)result);
		}

		public Task<RecurringPaymentOutput> MakePreAuthorizedPaymentAsync(RecurringPaymentInput request, CancellationToken token = default)
		{
			throw new NotImplementedException();
		}
	}

	internal class MockPaymentOutput : PaymentOutput
	{
		public string TransactionNumber { get; set; }

		public string CustomerCode { get; set; }

		public void SetAuth(string value)
		{
			AuthToken = value;
		}
	}

	internal class TokenList
	{
		public string[] Tokens { get; set; }
	}

	internal class StripeTokenList
	{
		public StripeToken[] Tokens { get; set; }
	}

	internal class StripeToken
	{
		public string AuthToken { get; set; }

		public string StripeCustomerId { get; set; }
	}
}
