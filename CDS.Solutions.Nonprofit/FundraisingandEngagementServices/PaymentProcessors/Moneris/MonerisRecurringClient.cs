using System;
using System.Threading;
using System.Threading.Tasks;
using PaymentProcessors.Models;

namespace PaymentProcessors.Moneris
{
	internal class MonerisRecurringClient : IPaymentClient<MonerisRecurringPaymentInput, MonerisRecurringPaymentOutput>
	{
		public Task<MonerisRecurringPaymentOutput> MakePaymentAsync(MonerisRecurringPaymentInput input, CancellationToken cancellationToken = default)
		{
			var output = new MonerisRecurringPaymentOutput(input);

			// create a unique identifier for the processing payment Moneris
			var transactionIdentifier = Guid.NewGuid().ToString();

			output.InvoiceIdentifier = transactionIdentifier;

			var transaction = new TransactionDetail
			{
				CryptType = Constants.CRYPTTYPE,
				DataKey = input.MonerisAuthToken,
				Amount = String.Format("{0:0.00}", input.Amount)
			};

			transaction.Identifier = transactionIdentifier;

			// This is the auth token on the payment method:
			transaction.CustID = input.CustID;
			transaction.ProcessingCountryCode = Constants.COUNTRY_CODE_CA;
			transaction.StoreId = input.StoreId;
			transaction.ApiKey = input.ServiceKey;
			transaction.IsTestMode = input.IsTestMode;

			var receipt = MonerisProcess.PurchaseWithCC(transaction);

			if (receipt != null)
			{
				Int32.TryParse(receipt.GetResponseCode(), out var responseCode);

				if (responseCode == 0)
				{
					output.SetFailure(input);

					output.TransactionResult = receipt.GetMessage();
					output.TransactionReceiptId = "0";
				}
				else if (responseCode < 50)
				{
					output.TransactionIdentifier = receipt.GetReferenceNum();
					output.TransactionStatus = receipt.GetComplete();
					output.TransactionReceiptId = receipt.GetResponseCode();
					output.TransactionNumber = receipt.GetTxnNumber();
					output.TransactionResult = receipt.GetResSuccess();
					output.TransactionFraudCode = receipt.GetAvsResultCode();
					output.Response = receipt.GetMessage();

					if (Boolean.TryParse(receipt.GetResSuccess(), out var isTrue) && isTrue)
					{
						output.IsSuccessful = true;
					}
				}
				else if (responseCode > 49)
				{
					output.SetFailure(input);
					output.TransactionResult = "Payment failed.";
					output.TransactionReceiptId = receipt.GetResponseCode();
				}
			}
			else
			{
				output.SetFailure(input);
				output.TransactionResult = "Payment failed.";
				output.TransactionReceiptId = "No receipt created.";
			}

			return Task.FromResult(output);
		}
	}
}
