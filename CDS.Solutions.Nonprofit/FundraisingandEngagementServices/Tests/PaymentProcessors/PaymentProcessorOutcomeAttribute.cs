using System;

namespace FundraisingandEngagement.Tests.PaymentProcessors
{
	[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
	public class PaymentProcessorOutcomeAttribute : Attribute
	{
		public string SuccessMessage { get; }

		public PaymentProcessorOutcomeAttribute(string successMessage)
		{
			SuccessMessage = successMessage;
		}
	}
}
