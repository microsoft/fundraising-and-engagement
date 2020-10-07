using System;

namespace PaymentProcessors
{
	internal class CreditCardConstants
	{
		public static string CRYPTTYPE = "7";
		public static string COUNTRY_CODE_CA = "CA";
		public static string COUNTRY_CODE_US = "US";
		public static string APILOGINID = "4d5LA2LWrG";
		public static string APITRANSACTIONKEY = "8DTXuu74Jw6yz35G";
	}

	internal class CreditCardDetail
	{
		public string CCNumber { get; set; }
		public string ExpDate { get; set; }
		public string CVV { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string Note { get; set; }
		public string CryptType { get; set; }
		public string CustomerId { get; set; }
		public string ProcessingCountryCode { get; set; }
		public bool StatusCheck { get; set; }
		public string Identifier { get; set; }
		public string InvoiceNumber { get; set; }
		public string CVV2 { get; set; }
		public string DataKey { get; set; }

		//useful for iATS process
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool Recurring { get; set; }
		public DateTime BeginDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Amount { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string PostalCode { get; set; }
	}
}