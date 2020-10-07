using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentProcessors.Models;

namespace FundraisingandEngagement.Tests.PaymentProcessors
{
	[TestClass]
	public class Iats : PaymentProcessorTestBase
	{
		public Iats() : base(PaymentProcessorType.Iats)
		{
		}

		[TestMethod]
		public async Task ProcessDonation()
		{
			await ProcessDonation_ReturnsPaymentGatewayResponse<IatsCreditCardPaymentInput>(isRecurring: false);
		}

		[TestMethod]
		public async Task Recurring_ProcessDonation()
		{
			await ProcessDonation_ReturnsPaymentGatewayResponse<IatsCreditCardPaymentInput>(isRecurring: true);
		}

		[TestMethod]
		public async Task Iats_Succeeds_ProcessCreditCard_Returns_Returns_PaymentGatewayOutputAdyen()
		{
			await RecurringDonation_ReturnsPaymentGatewayResponse<IatsRecurringPaymentInput>(true);
		}

		[TestMethod]
		public async Task Iats_Rejected_ProcessCreditCard_Returns_Returns_PaymentGatewayOutputAdyen()
		{
			await RecurringDonation_ReturnsPaymentGatewayResponse<IatsRecurringPaymentInput>(false);
		}
	}
}
