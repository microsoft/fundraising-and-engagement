using System;

namespace PaymentProcessors.iATS
{
	public class CreateCreditCardCustomerCode
	{
		public string agentCode { get; set; } //= "AURA88";
		public string password { get; set; } // = "AURA88";
		public string customerIPAddress { get; set; } // = "170.101.173.129";
		public string firstName { get; set; } // = "fq";
		public string lastName { get; set; } // = "lq";
		public string address { get; set; } // = "da";
		public string city { get; set; } // = "aa";
		public string state { get; set; } // = "sa";
		public string zipCode { get; set; } // = "123456";
		public string phone { get; set; } // = "989898989898";
		public string email { get; set; } // = "test123@gmail.com";
		public string comment { get; set; } // = "c";
		public bool recurring { get; set; } // = false;
		public DateTime beginDate { get; set; } // = new DateTime(2017, 3, 18);
		public DateTime endDate { get; set; } // = new DateTime(2017, 4, 18);
		public string creditCardNum { get; set; } // = "4111111111111111";
		public string creditCardExpiry { get; set; } // = "12/2018";
		public string mop { get; set; } // = "Test";
		public string country { get; set; } // = "A";
	}
}
