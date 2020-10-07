namespace PaymentProcessors.iATS
{
	public class ProcessCreditCardWithCustomerCode
	{
		public string agentCode { get; set; }
		public string password { get; set; }
		public string customerIPAddress { get; set; }
		public string customerCode { get; set; }
		public string invoiceNum { get; set; }
		public string cvv2 { get; set; }
		public string total { get; set; }
		public string comment { get; set; }
	}
}
