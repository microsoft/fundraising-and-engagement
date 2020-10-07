namespace PaymentProcessors.Moneris
{
	internal class Constants
	{
		public static string CRYPTTYPE = "7";
		public static string COUNTRY_CODE_CA = "CA";
		public static string COUNTRY_CODE_US = "US";
	}

	internal class TransactionDetail
	{
		public string DataKey { get; set; }
		public string TxnNumber { get; set; }
		public string InvoiceNumber { get; set; }
		public string ReceiptID { get; set; }
		public string Identifier { get; set; }
		public string Amount { get; set; }
		public string CryptType { get; set; }
		public string ProcessingCountryCode { get; set; }
		public string CustID { get; set; }
		public string TransactionID { get; set; }


		public string StoreId { get; set; }
		public string ApiKey { get; set; }

		public bool IsTestMode { get; set; }
	}
}