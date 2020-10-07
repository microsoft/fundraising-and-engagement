using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using PaymentProcessors.Models;

namespace PaymentDriver.Models
{
	internal class PaymentInfo
	{
		public PaymentInput Input { get; set; }

		public PaymentOutput Output { get; set; }

		public TransactionCurrency TransactionCurrency { get; set; }

		public Configuration Configuration { get; set; }

		public PaymentProcessor PaymentProcessor { get; set; }

		public PaymentTypeCode PaymentTypeCode { get; set; }

		public decimal PaymentAmount { get; set; }

		public string BillingCity { get; set; }
		public string BillingCountry { get; set; }
		public string BillingLine1 { get; set; }
		public string BillingPostalCode { get; set; }
		public string BillingStateorProvince { get; set; }
		public string EmailAddress1 { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string OrganizationName { get; set; }
		public string Telephone1 { get; set; }

		public string CreditCardNo { get; set; }

		public bool IsBankProcess { get; set; }
		public bool IsRecurring { get; set; }

		public PaymentInfo()
		{
			PaymentTypeCode = PaymentTypeCode.CreditCard;

			CreditCardNo = "4111111111111111";
			IsRecurring = true;
			IsBankProcess = false;
			PaymentAmount = 100m;

			BillingCity = "Racoon City";
			BillingCountry = "Canada";
			BillingLine1 = "123 ABC";
			BillingPostalCode = "XYZ123";
			BillingStateorProvince = "Ontario";
			EmailAddress1 = "john.doe@email.com";
			FirstName = "John";
			LastName = "Doe";
			OrganizationName = "MCRM Test";

		}
	}
}
