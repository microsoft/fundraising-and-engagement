using System.Collections.Generic;

namespace PaymentProcessors.Worldpay
{
	internal class OrderRequest
	{
		public string token { get; set; }

		public string orderDescription { get; set; }

		public string name { get; set; }

		public decimal? amount { get; set; }
		public bool is3DSOrder { get; set; }

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
		public string orderType { get; set; }

	}

	public class Address
	{
		public string address1 { get; set; }

		public string address2 { get; set; }

		//public string address3 { get; set; }

		public string postalCode { get; set; }

		public string city { get; set; }

		public string state { get; set; }

		public string countryCode { get; set; }

		public string telephoneNumber { get; set; }
	}


	internal class PartialRefund
	{
		public int refundAmount { get; set; }
	}

	public enum CountryCode
	{
		AF, AX, AL, DZ, AS, AD, AO, AI, AQ, AG, AR, AM, AW, AU, AT, AZ, BS, BH, BD, BB, BY, BE, BZ, BJ, BM, BT,
		BO, BQ, BA, BW, BV, BR, IO, BN, BG, BF, BI, KH, CM, CA, CV, KY, CF, TD, CL, CN, CX, CC, CO, KM, CG, CD,
		CK, CR, CI, HR, CU, CW, CY, CZ, DK, DJ, DM, DO, EC, EG, SV, GQ, ER, EE, ET, FK, FO, FJ, FI, FR, GF, PF,
		TF, GA, GM, GE, DE, GH, GI, GR, GL, GD, GP, GU, GT, GG, GN, GW, GY, HT, HM, VA, HN, HK, HU, IS, IN, ID,
		IR, IQ, IE, IM, IL, IT, JM, JP, JE, JO, KZ, KE, KI, KP, KR, KW, KG, LA, LV, LB, LS, LR, LY, LI, LT, LU,
		MO, MK, MG, MW, MY, MV, ML, MT, MH, MQ, MR, MU, YT, MX, FM, MD, MC, MN, ME, MS, MA, MZ, MM, NA, NR, NP,
		NL, NC, NZ, NI, NE, NG, NU, NF, MP, NO, OM, PK, PW, PS, PA, PG, PY, PE, PH, PN, PL, PT, PR, QA, RE, RO,
		RU, RW, BL, SH, KN, LC, MF, PM, VC, WS, SM, ST, SA, SN, RS, SC, SL, SG, SX, SK, SI, SB, SO, ZA, GS, SS,
		ES, LK, SD, SR, SJ, SZ, SE, CH, SY, TW, TJ, TZ, TH, TL, TG, TK, TO, TT, TN, TR, TM, TC, TV, UG, UA, AE,
		GB, US, UM, UY, UZ, VU, VE, VN, VG, VI, WF, EH, YE, ZM, ZW
	}

	public enum CurrencyCode
	{
		ALL, DZD, XCD, ARS, AWG, AUD, AZN, BSD, BHD, BDT, BBD, BZD, BMD, BOB, BWP, BRL, BND, BGN, XOF, MMK, KHR,
		XAF, CAD, KYD, CLP, CNY, COP, CRC, HRK, CZK, DKK, DJF, DOP, EGP, SVC, ERN, ETB, EUR, FJD, XPF, GEL, GHS,
		GIP, GTQ, HNL, HKD, HUF, ISK, INR, IDR, IRR, ILS, JMD, JPY, JOD, KZT, KES, KWD, LVL, LBP, LSL, LYD, LTL,
		MOP, MKD, MYR, MVR, MRO, MUR, MXN, MNT, MAD, MZN, NAD, ANG, NZD, NIO, NGN, NOK, OMR, PKR, PAB, PYG, PEN,
		PHP, PLN, QAR, RON, RUB, RWF, SAR, RSD, SCR, SLL, SGD, ZAR, KRW, LKR, SZL, SEK, CHF, SYP, TWD, TZS, THB,
		TTD, TND, TRY, UAH, AED, GBP, USD, UYU, UZS, VEF, VND, YER, ZMW
	}

	public static class WorldPayUrl
	{
		public static string apiUrl = "https://api.worldpay.com/v1";
	}
}
