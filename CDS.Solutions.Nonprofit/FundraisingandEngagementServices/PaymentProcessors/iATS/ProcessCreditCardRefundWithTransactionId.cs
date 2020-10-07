namespace PaymentProcessors.iATS
{
	public class ProcessCreditCardRefundWithTransactionId
	{
		public string agentCode { get; set; }
		public string password { get; set; }
		public string customerIPAddress { get; set; }
		public string transactionId { get; set; }
		public string total { get; set; }
		public string comment { get; set; }
	}
}
