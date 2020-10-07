namespace PaymentProcessors.Moneris
{
	internal class MonerisCCInput
	{
		//creditcard
		public string StoreId { get; set; }

		//creditcard
		public string Key { get; set; }

		// Moneris Test Mode
		public bool? TestMode { get; set; }

		//creditcard
		public int? ServiceModeOptionSetId { get; set; }
	}
}
