using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentProcessors.Models;

namespace FundraisingandEngagement.Tests.PaymentProcessors
{
	[TestClass]
	public class Moneris : PaymentProcessorTestBase
	{
		public Moneris() : base(PaymentProcessorType.Moneris)
		{
		}

		[TestMethod]
		public async Task ProcessDonation()
		{
			await ProcessDonation_ReturnsPaymentGatewayResponse<MonerisCreditCardPaymentInput>(isRecurring: false);
		}

		[TestMethod]
		public async Task Recurring_ProcessDonation()
		{
			await ProcessDonation_ReturnsPaymentGatewayResponse<MonerisCreditCardPaymentInput>(isRecurring: true);
		}

		[TestMethod]
		public async Task Moneris_Succeeds_ProcessCreditCard_Returns_PaymentGatewayInputMoneris()
		{
			await RecurringDonation_ReturnsPaymentGatewayResponse<MonerisRecurringPaymentInput>(true);
		}

		[TestMethod]
		public async Task Moneris_Rejected_ProcessCreditCard_Returns_PaymentGatewayInputMoneris()
		{
			await RecurringDonation_ReturnsPaymentGatewayResponse<MonerisRecurringPaymentInput>(true);
		}
	}
}
