using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PaymentProcessors.Models;

namespace PaymentProcessors
{
	internal class PaymentProcessorGateway : IPaymentProcessorGateway
	{
		private readonly IServiceProvider serviceProvider;

		public PaymentProcessorGateway(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public async Task<PaymentOutput> MakePaymentAsync(PaymentInput input, CancellationToken token = default)
		{
			switch (input)
			{
				case IatsCreditCardPaymentInput i:
					var iats = this.serviceProvider.GetRequiredService<IPaymentClient<IatsCreditCardPaymentInput, PaymentOutput>>();
					return await iats.MakePaymentAsync(i, token);
				case MonerisCreditCardPaymentInput i:
					var moneris = this.serviceProvider.GetRequiredService<IPaymentClient<MonerisCreditCardPaymentInput, PaymentOutput>>();
					return await moneris.MakePaymentAsync(i, token);
				case StripeCreditCardPaymentInput i:
					var stripe = this.serviceProvider.GetRequiredService<IPaymentClient<StripeCreditCardPaymentInput, PaymentOutput>>();
					return await stripe.MakePaymentAsync(i, token);
				case WorldPayCreditCardPaymentInput i:
					var worldPay = this.serviceProvider.GetRequiredService<IPaymentClient<WorldPayCreditCardPaymentInput, PaymentOutput>>();
					return await worldPay.MakePaymentAsync(i, token);
			}

			throw new NotSupportedException($"Input of type '{input.GetType().Name}' is not supported.");
		}

		public async Task<RecurringPaymentOutput> MakePreAuthorizedPaymentAsync(RecurringPaymentInput input, CancellationToken token = default)
		{
			switch (input)
			{
				case IatsRecurringPaymentInput i:
					var iats = this.serviceProvider.GetRequiredService<IPaymentClient<IatsRecurringPaymentInput, IatsRecurringPaymentOutput>>();
					return await iats.MakePaymentAsync(i, token);
				case MonerisRecurringPaymentInput i:
					var moneris = this.serviceProvider.GetRequiredService<IPaymentClient<MonerisRecurringPaymentInput, MonerisRecurringPaymentOutput>>();
					return await moneris.MakePaymentAsync(i, token);
				case StripeRecurringPaymentInput i:
					var stripe = this.serviceProvider.GetRequiredService<IPaymentClient<StripeRecurringPaymentInput, StripeRecurringPaymentOutput>>();
					return await stripe.MakePaymentAsync(i, token);
				case WorldPayRecurringPaymentInput i:
					var worldPay = this.serviceProvider.GetRequiredService<IPaymentClient<WorldPayRecurringPaymentInput, WorldPayRecurringPaymentOutput>>();
					return await worldPay.MakePaymentAsync(i, token);
			}

			throw new NotSupportedException($"Input of type '{input.GetType().Name}' is not supported.");
		}
	}
}
