using System;
using PaymentProcessors.Models;

namespace FundraisingandEngagement.RecurringDonations.Services
{
	internal class RecurringPaymentErrorOutput : RecurringPaymentOutput
	{
		public RecurringPaymentErrorOutput(int currentRetry, DateTime? nextFailedRetry, string invoiceIdentifier)
			: base(currentRetry, nextFailedRetry, invoiceIdentifier)
		{
		}
	}
}
