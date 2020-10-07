using System.Collections.Generic;

namespace PaymentProcessors.Worldpay
{
	public class OrderResponse
	{
		public string token { get; set; }

		public string orderDescription { get; set; }

		public string name { get; set; }

		public decimal? amount { get; set; }

		public string currencyCode { get; set; }

		public string settlementCurrency { get; set; }

		public string siteCode { get; set; }

		public bool authorizeOnly { get; set; }

		public bool authoriseOnly { get; set; }

		public int? authorizedAmount { get; set; }

		public Address billingAddress { get; set; }

		public Dictionary<string, string> customerIdentifiers { get; set; }

		public string customerOrderCode { get; set; }

		public string orderCodeSuffix { get; set; }

		public string orderCodePrefix { get; set; }

		public string shopperLanguageCode { get; set; }

		public bool reusable { get; set; }

		public string orderCode { get; set; }

		public string paymentStatus { get; set; }

		public string paymentStatusReason { get; set; }

		public PaymentResponse paymentResponse { get; set; }

		public bool is3DSOrder { get; set; }

		public string oneTime3DsToken { get; set; }

		public string redirectURL { get; set; }

		public string environment { get; set; }

		public string shopperEmailAddress { get; set; }

		public string statementNarrative { get; set; }

	}

	public class PaymentResponse
	{
		public string name { get; set; }

		public int expiryMonth { get; set; }

		public int expiryYear { get; set; }

		public int? issueNumber { get; set; }

		public int? startMonth { get; set; }

		public int? startYear { get; set; }

		public string cardType { get; set; }

		public string maskedCardNumber { get; set; }

		public Address billingAddress { get; set; }

		public Dictionary<string, string> apmFields { get; set; }
		public string type { get; set; }
	}

	public enum OrderStatus
	{
		SUCCESS,
		FAILED,
		AUTHORIZED,
		PRE_AUTHORIZED,
		SENT_FOR_REFUND,
		PARTIALLY_REFUNDED,
		REFUNDED,
		SETTLED,
		INFORMATION_REQUESTED,
		INFORMATION_SUPPLIED,
		CHARGED_BACK,
		EXPIRED,
		CHARGEBACK_REVERSED
	}

	public enum Environment
	{
		TEST,
		LIVE
	}
}
