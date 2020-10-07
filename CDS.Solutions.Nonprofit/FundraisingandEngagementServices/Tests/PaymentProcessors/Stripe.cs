using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentProcessors.Models;

namespace FundraisingandEngagement.Tests.PaymentProcessors
{
	[TestClass]
	public class Stripe : PaymentProcessorTestBase
	{
		public Stripe() : base(PaymentProcessorType.Stripe)
		{
		}

		[TestMethod]
		public async Task ProcessDonation()
		{
			await ProcessDonation_ReturnsPaymentGatewayResponse<StripeCreditCardPaymentInput>(isRecurring: false);
		}

		[TestMethod]
		public async Task Recurring_ProcessDonation()
		{
			await ProcessDonation_ReturnsPaymentGatewayResponse<StripeCreditCardPaymentInput>(isRecurring: true);
		}

		[TestMethod]
		public async Task Stripe_Succeeds_ProcessCreditCard_Returns_Returns_PaymentGatewayOutputStripe()
		{
			await RecurringDonation_ReturnsPaymentGatewayResponse<StripeRecurringPaymentInput>(true);
		}

		[TestMethod]
		public async Task Stripe_Rejected_ProcessCreditCard_Returns_Returns_PaymentGatewayOutputStripe()
		{
			await RecurringDonation_ReturnsPaymentGatewayResponse<StripeRecurringPaymentInput>(false);
		}
	}
}
