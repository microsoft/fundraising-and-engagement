using System;

namespace PaymentProcessors.iATS
{
	public class UpdateCreditCardCustomerCode
	{
		public string agentCode { get; set; }
		public string password { get; set; }
		public string customerIPAddress { get; set; }
		public string customerCode { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string companyName { get; set; }
		public string address { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string zipCode { get; set; }
		public string country { get; set; }
		public string phone { get; set; }
		public string alternatePhone { get; set; }
		public string fax { get; set; }
		public string email { get; set; }
		public string comment { get; set; }
		public bool recurring { get; set; }
		public string amount { get; set; }
		public DateTime beginDate { get; set; }
		public DateTime endDate { get; set; }
		public string creditCardCustomerName { get; set; }
		public string creditCardNum { get; set; }
		public string creditCardExpiry { get; set; }
		public string mop { get; set; }
		public bool updateCreditCardNum { get; set; }
		public string total { get; set; }
	}
}
