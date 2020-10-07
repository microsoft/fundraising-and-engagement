namespace FundraisingandEngagement.Tests.PaymentProcessors
{
	public enum PaymentProcessorType
	{
		None,

		[PaymentProcessorOutcome("Authorised")]
		Adyen,

		[PaymentProcessorOutcome("OK")]
		Iats,

		[PaymentProcessorOutcome("APPROVED")]
		Moneris,

		[PaymentProcessorOutcome("succeeded")]
		Stripe,

		[PaymentProcessorOutcome("SUCCESS")]
		WorldPay
	}
}
