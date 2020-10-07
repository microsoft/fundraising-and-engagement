using System;
using System.Threading;
using System.Threading.Tasks;
using Moneris;
using PaymentProcessors.Models;

namespace PaymentProcessors.Moneris
{
	internal class MonerisClient : IPaymentClient<MonerisCreditCardPaymentInput, PaymentOutput>
	{
		public Task<PaymentOutput> MakePaymentAsync(MonerisCreditCardPaymentInput input, CancellationToken cancellationToken = default)
		{
			var response = new PaymentOutput();

			var CCD = new CreditCardDetail();
			CCD.CCNumber = input.CreditCardNo;
			CCD.ExpDate = input.CcExpMmYy;
			CCD.CryptType = CreditCardConstants.CRYPTTYPE;
			CCD.ProcessingCountryCode = CreditCardConstants.COUNTRY_CODE_CA;

			// This is currently not used, it gets commented out in MonerisProcess.AddCC function:
			var avsCheck = new AvsInfo();
			var address1_line1 = input.BillingLine1;
			avsCheck.SetAvsStreetNumber(address1_line1);

			var address1_line2 = input.BillingLine2;
			if (String.IsNullOrEmpty(address1_line2))
			{
				var strArray = address1_line1.Split(' ');
				if (strArray.Length > 1)
					avsCheck.SetAvsStreetName(strArray[1].ToString());
			}
			else
				avsCheck.SetAvsStreetName(address1_line2);

			var address1_postalcode = input.BillingPostalCode;
			avsCheck.SetAvsZipCode(address1_postalcode);

			var monerisCCInput = new MonerisCCInput()
			{
				StoreId = input.MonerisStoreId,
				Key = input.ServiceKey,
				TestMode = input.IsTestMode,
				ServiceModeOptionSetId = input.ServiceModeOptionSetId
			};

			var addCCReceipt = MonerisProcess.AddCC(CCD, avsCheck, monerisCCInput);

			if (addCCReceipt == null)
			{
				return Task.FromResult(response);
			}


			Int32.TryParse(addCCReceipt.GetResponseCode(), out var addCCResponseCode);

			if (addCCResponseCode != 0 && addCCResponseCode < 50)
			{
				var transaction = new TransactionDetail();

				transaction.DataKey = addCCReceipt.GetDataKey();
				transaction.Amount = String.Format("{0:0.00}", input.Amount);
				transaction.CryptType = CreditCardConstants.CRYPTTYPE;
				transaction.ProcessingCountryCode = CreditCardConstants.COUNTRY_CODE_CA;
				transaction.StoreId = input.MonerisStoreId;
				transaction.ApiKey = input.ServiceKey;
				transaction.IsTestMode = input.IsTestMode;

				var order_id = Guid.NewGuid().ToString();
				response.InvoiceNumber = order_id;
				transaction.Identifier = order_id;

				// If there is no auth id, get it from the added CC receipt:
				if (transaction.CustID == null)
				{
					transaction.CustID = addCCReceipt.GetDataKey();
				}

				var transactionReceipt = MonerisProcess.PurchaseWithCC(transaction);

				if (transactionReceipt != null)
				{
					Int32.TryParse(transactionReceipt.GetResponseCode(), out var responseCode);

					if (responseCode != 0 && responseCode < 50)
					{
						response.IsSuccessful = true;
					}
					else if (responseCode == 0 || responseCode > 49)
					{
						response.IsSuccessful = false;
					}

					response.TransactionResult = $"{transactionReceipt.GetMessage()} - Response Code: {responseCode}";
					response.TransactionIdentifier = transactionReceipt.GetReferenceNum();
					response.AuthToken = transactionReceipt.GetDataKey();
					response.CardType = transactionReceipt.GetCardType();
				}
			}
			else if (addCCResponseCode == 0 || addCCResponseCode > 49)
			{
				response.IsSuccessful = false;
				response.TransactionResult = addCCReceipt.GetMessage();
			}

			return Task.FromResult(response);
		}
	}
}
