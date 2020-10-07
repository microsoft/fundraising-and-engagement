using System;

namespace PaymentProcessors.Models
{
	public abstract class RecurringPaymentInput
	{
		// instant gift
		public decimal Amount { get; set; }

		// instant gift
		public string IsoCurrencyCode { get; set; }

		// instant gift
		public DateTime? NextFailedRetry { get; set; }

		// instant gift
		public int CurrentRetry { get; set; }

		// all config
		public int RetryInterval { get; set; }

		// all config
		public int MaxRetries { get; set; }

		public bool IsBankAccountPayment { get; set; }

		public string InvoiceIdentifier { get; set; }
	}

	public class WorldPayRecurringPaymentInput : RecurringPaymentInput
	{
		// config
		public string ServiceKey { get; set; }

		// creditcard authtoken
		public string WorldPayToken { get; set; }

		// parent
		public string FirstName { get; set; }

		// parent
		public string LastName { get; set; }

		// parent billing_line1
		public string Address1 { get; set; }

		// parent billing_line2
		public string Address2 { get; set; }

		// parent billing_city
		public string City { get; set; }

		// parent billing_postalcode
		public string PostalCode { get; set; }
	}

	public class StripeRecurringPaymentInput : RecurringPaymentInput
	{
		// config
		public string ServiceKey { get; set; }

		// creditcard -> adyenKey
		public string StripeCustomerId { get; set; }

		// creditcard authtoken
		public string StripeCardId { get; set; }
	}

	public class MonerisRecurringPaymentInput : RecurringPaymentInput
	{
		// config
		public string StoreId { get; set; }

		// config
		public string ServiceKey { get; set; }

		// Moneris Test Mode
		public bool IsTestMode { get; set; } = true;

		// creditcard adyen and moneris stripe
		public string MonerisAuthToken { get; set; }

		// Used on recurring donations. This is the Moneris custid from the payment method NOT the customer id on the msnfp_transaction (GUID).
		public string CustID { get; set; }
	}

	public class IatsRecurringPaymentInput : RecurringPaymentInput
	{
		// config
		public string AgentCode { get; set; }

		// config
		public string Password { get; set; }

		// instant gift
		public string IpAddress { get; set; }

		public string IatsCustomerCode { get; set; }
	}
}
