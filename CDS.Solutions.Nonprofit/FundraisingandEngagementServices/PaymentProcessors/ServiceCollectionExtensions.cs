using System.Net.Http;
using System.Security.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using PaymentProcessors.iATS;
using PaymentProcessors.Models;
using PaymentProcessors.Moneris;
using PaymentProcessors.Stripe;
using PaymentProcessors.WorldPay;

namespace PaymentProcessors
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddPaymentProcessors(this IServiceCollection services, ILogger logger = null)
		{
			services
				.AddHttpClient("PaymentProcessor", client => client.DefaultRequestHeaders.ExpectContinue = true)
				.ConfigurePrimaryHttpMessageHandler(() =>
				{
					var handler = new HttpClientHandler();
					handler.SslProtocols = SslProtocols.Tls12;
					return handler;
				});

			services.TryAddSingleton<IPaymentProcessorGateway, PaymentProcessorGateway>();

			services.TryAddSingleton(sp => logger ?? sp.GetRequiredService<ILoggerFactory>().CreateLogger("PaymentLogger"));

			services.TryAddSingleton<IPaymentClient<IatsCreditCardPaymentInput, PaymentOutput>, IatsClient>();
			services.TryAddSingleton<IPaymentClient<IatsRecurringPaymentInput, IatsRecurringPaymentOutput>, IatsRecurringClient>();
			services.TryAddSingleton<IPaymentClient<MonerisCreditCardPaymentInput, PaymentOutput>, MonerisClient>();
			services.TryAddSingleton<IPaymentClient<MonerisRecurringPaymentInput, MonerisRecurringPaymentOutput>, MonerisRecurringClient>();
			services.TryAddSingleton<IPaymentClient<StripeCreditCardPaymentInput, PaymentOutput>, StripeClient>();
			services.TryAddSingleton<IPaymentClient<StripeRecurringPaymentInput, StripeRecurringPaymentOutput>, StripeRecurringClient>();
			services.TryAddSingleton<IPaymentClient<WorldPayCreditCardPaymentInput, PaymentOutput>, WorldPayClient>();
			services.TryAddSingleton<IPaymentClient<WorldPayRecurringPaymentInput, WorldPayRecurringPaymentOutput>, WorldPayRecurringClient>();

			return services;
		}
	}
}
