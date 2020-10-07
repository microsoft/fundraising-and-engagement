namespace PaymentProcessors.Models
{
	public class PaymentOutput
	{
		public bool IsSuccessful { get; set; }

		public string TransactionResult { get; set; }

		public string TransactionIdentifier { get; internal set; }

		public string InvoiceNumber { get; set; }

		public string AuthToken { get; protected internal set; }

		public string CardType { get; set; }

		public string StripeCustomerId { get; set; }
	}
}
