namespace PaymentProcessors.Models
{
	public abstract class PaymentInput
	{
		public bool IsRecurring { get; set; }

		public bool IsBankProcess { get; set; }
	}

	public abstract class CreditCardPaymentInput : PaymentInput
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string CcExpMmYy { get; set; }

		public string CreditCardNo { get; set; }

		public decimal Amount { get; set; }

		public string BillingPostalCode { get; set; }

		public string BillingCity { get; set; }

		public string BillingLine1 { get; set; }

		public string Cvc { get; set; }

		public string EmailAddress { get; set; }
	}

	public class IatsCreditCardPaymentInput : CreditCardPaymentInput
	{
		public string IatsAgentCode { get; set; }

		public string IatsPassword { get; set; }

		public string BillingStateorProvince { get; set; }

		public string Telephone { get; set; }

		public string BankNumber { get; set; }

		public string AccountNumber { get; set; }

		public string RoutingNumber { get; set; }

		public string TransitNumber { get; set; }

		public BankAccountType AccountType { get; set; }

		public AccountNumberFormat AccountNumberFormat { get; set; }

		public string IpAddress { get; set; }
	}

	public class MonerisCreditCardPaymentInput : CreditCardPaymentInput
	{
		public string BillingLine2 { get; set; }

		public string MonerisStoreId { get; set; }

		public string ServiceKey { get; set; }

		public bool IsTestMode { get; set; } = true;

		public int ServiceModeOptionSetId { get; set; }
	}

	public class StripeCreditCardPaymentInput : CreditCardPaymentInput
	{
		public string IsoCurrencyCode { get; set; }

		public string ServiceKey { get; set; }
	}

	public class WorldPayCreditCardPaymentInput : CreditCardPaymentInput
	{
		public string IsoCurrencyCode { get; set; }

		public string ServiceKey { get; set; }

		public string WorldPayToken { get; set; }
	}

	public enum BankAccountType
	{
		Checking,
		Saving,
		Other
	}
}
