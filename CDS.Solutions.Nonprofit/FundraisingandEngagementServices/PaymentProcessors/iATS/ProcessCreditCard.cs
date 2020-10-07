namespace PaymentProcessors.iATS
{
	public class ProcessCreditCard
	{
		public string agentCode { get; set; }
		public string password { get; set; }
		public string customerIPAddress { get; set; }
		public string creditCardNum { get; set; }
		public string creditCardExpiry { get; set; }
		public string invoiceNum { get; set; }
		public string cvv2 { get; set; }
		public string mop { get; set; }
		public string firstName { get; set; } // = "fq";
		public string lastName { get; set; } // = "lq";
		public string address { get; set; } // = "da";
		public string city { get; set; } // = "aa";
		public string state { get; set; } // = "sa";
		public string zipCode { get; set; }
		public string total { get; set; }
		public string comment { get; set; }
		public string title { get; set; }
		public string phone { get; set; }
		public string phone2 { get; set; }
		public string fax { get; set; }
		public string email { get; set; }
		public string country { get; set; }
	}
}
