using System;

namespace PaymentProcessors.Models
{
	public abstract class RecurringPaymentOutput
	{
		public bool IsSuccessful { get; set; }

		public int CurrentRetry { get; set; }

		public DateTime? NextFailedRetry { get; set; }

		public DateTime? LastFailedRetry { get; set; }

		// adyen iats moneris
		public string InvoiceIdentifier { get; set; }

		//adyen iats stripe worldpay
		public string TransactionIdentifier { get; set; }

		//adyen iats moneris stripe worldpay
		public string TransactionResult { get; set; }

		//iats moneris stripe worldpay
		public string TransactionStatus { get; set; }

		public string TransactionNumber { get; set; }

		public string TransactionFraudCode { get; set; }


		protected RecurringPaymentOutput(RecurringPaymentInput input)
		{
			CurrentRetry = input.CurrentRetry;
			NextFailedRetry = input.NextFailedRetry;
			InvoiceIdentifier = input.InvoiceIdentifier;
		}

		protected RecurringPaymentOutput(int currentRetry, DateTime? nextFailedRetry, string invoiceIdentifier)
		{
			CurrentRetry = currentRetry;
			NextFailedRetry = nextFailedRetry;
			InvoiceIdentifier = invoiceIdentifier;
		}

		internal void SetFailure(RecurringPaymentInput input)
		{
			IsSuccessful = false;

			if (input.CurrentRetry <= input.MaxRetries)
			{
				NextFailedRetry = DateTime.Now.Date.AddDays(input.RetryInterval).Date;
				CurrentRetry = input.CurrentRetry + 1;
				LastFailedRetry = DateTime.Now.Date;
			}
		}
	}

	internal class MonerisRecurringPaymentOutput : RecurringPaymentOutput
	{
		public string Response { get; set; }

		public string TransactionReceiptId { get; set; }

		public MonerisRecurringPaymentOutput(RecurringPaymentInput input)
			: base(input)
		{
		}
	}

	internal class IatsRecurringPaymentOutput : RecurringPaymentOutput
	{
		public IatsRecurringPaymentOutput(IatsRecurringPaymentInput input)
			: base(input)
		{
		}
	}

	internal class StripeRecurringPaymentOutput : RecurringPaymentOutput
	{
		public StripeRecurringPaymentOutput(StripeRecurringPaymentInput input)
			: base(input)
		{
		}
	}

	internal class WorldPayRecurringPaymentOutput : RecurringPaymentOutput
	{
		public WorldPayRecurringPaymentOutput(WorldPayRecurringPaymentInput input)
			: base(input)
		{
		}
	}
}
