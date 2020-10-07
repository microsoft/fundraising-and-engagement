using System.Net;
using Moneris;

namespace PaymentProcessors.Moneris
{
	internal static class MonerisProcess
	{
		public static Receipt PurchaseWithCC(TransactionDetail transaction)
		{
			var STOREID = transaction.StoreId;
			var APIKEY = transaction.ApiKey;

			var resPurchaseCC = new ResPurchaseCC();
			resPurchaseCC.SetDataKey(transaction.DataKey);
			resPurchaseCC.SetOrderId(transaction.Identifier);  //order_id
			resPurchaseCC.SetCustId(transaction.CustID);   //CustomerId
			resPurchaseCC.SetAmount(transaction.Amount);
			resPurchaseCC.SetCryptType(transaction.CryptType);

			var mpgReq = new HttpsPostRequest();
			mpgReq.SetProcCountryCode(transaction.ProcessingCountryCode);
			mpgReq.SetTestMode(transaction.IsTestMode); //false or comment out this line for production transactions
			mpgReq.SetStoreId(STOREID);
			mpgReq.SetApiToken(APIKEY);
			mpgReq.SetTransaction(resPurchaseCC);
			mpgReq.SetStatusCheck(false);
			mpgReq.Send();
			/*********************   REQUEST  ***********************/

			return mpgReq.GetReceipt();
		}

		public static Receipt PurchaseWithBA(TransactionDetail transaction, MonerisCCInput input)
		{
			var status_check = false;

			var resPurchaseAch = new ResPurchaseAch();
			resPurchaseAch.SetDataKey(transaction.DataKey);
			resPurchaseAch.SetOrderId(transaction.Identifier);  //order_id
			resPurchaseAch.SetCustId(transaction.CustID);   //CustomerId
			resPurchaseAch.SetAmount(transaction.Amount);
			resPurchaseAch.SetCryptType(transaction.CryptType);

			var mpgReq = new HttpsPostRequest();
			mpgReq.SetProcCountryCode(transaction.ProcessingCountryCode);

			var isTestMode = input.TestMode ?? false;
			mpgReq.SetTestMode(isTestMode);

			mpgReq.SetStoreId(input.StoreId);
			mpgReq.SetApiToken(input.Key);
			mpgReq.SetTransaction(resPurchaseAch);
			mpgReq.SetStatusCheck(status_check);
			mpgReq.Send();
			/**********************   REQUEST  ************************/

			return mpgReq.GetReceipt();
		}

		public static Receipt AddCC(CreditCardDetail CCD, AvsInfo avsCheck, MonerisCCInput input)
		{
			var isTestMode = input.TestMode ?? false;

			var host = isTestMode ? "esqa.moneris.com" : "www3.moneris.com";

			var status_check = false;

			var resAddCC = new ResAddCC(CCD.CCNumber, CCD.ExpDate, CCD.CryptType);
			//resAddCC.SetAvsInfo(avsCheck);
			resAddCC.SetEmail(CCD.Email);
			resAddCC.SetPhone(CCD.Phone);
			resAddCC.SetNote(CCD.Note);

			var mpgReq = new HttpsPostRequest(host, input.StoreId, input.Key, resAddCC);
			mpgReq.SetProcCountryCode(CCD.ProcessingCountryCode);
			mpgReq.SetTestMode(isTestMode); //false or comment out this line for production transactions
			mpgReq.SetStatusCheck(status_check);
			mpgReq.Send();

			/**********************   REQUEST  ************************/

			return mpgReq.GetReceipt();
		}
	}
}
