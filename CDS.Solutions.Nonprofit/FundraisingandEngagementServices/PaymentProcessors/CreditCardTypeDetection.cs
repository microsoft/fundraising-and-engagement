using System.Text.RegularExpressions;

namespace PaymentProcessors
{
	public static class CreditCardTypeDetection
	{
		private const string cardRegex = "^(?:(?<Visa>4\\d{3})|" +
		"(?<MasterCard>5[1-5]\\d{2})|(?<Discover>6011)|(?<DinersClub>" +
		"(?:3[68]\\d{2})|(?:30[0-5]\\d))|(?<Amex>3[47]\\d{2}))([ -]?)" +
		"(?(DinersClub)(?:\\d{6}\\1\\d{4})|(?(Amex)(?:\\d{6}\\1\\d{5})" +
		"|(?:\\d{4}\\1\\d{4}\\1\\d{4})))$";

		public static CreditCardTypeTypeIats? IatsFromNumber(string cardNumber)
		{
			var cardType = FromNumber(cardNumber);

			return cardType switch
			{
				CreditCardTypeType.Amex => CreditCardTypeTypeIats.Amx,
				CreditCardTypeType.MasterCard => CreditCardTypeTypeIats.MC,
				CreditCardTypeType.Visa => CreditCardTypeTypeIats.Visa,
				CreditCardTypeType.Discover => CreditCardTypeTypeIats.DSC,
				_ => null
			};
		}

		public static CreditCardTypeType? FromNumber(string cardNumber)
		{
			var cardTest = new Regex(cardRegex);

			var gc = cardTest.Match(cardNumber).Groups;

			if (gc[CreditCardTypeType.Amex.ToString()].Success)
				return CreditCardTypeType.Amex;
			else if (gc[CreditCardTypeType.MasterCard.ToString()].Success)
				return CreditCardTypeType.MasterCard;
			else if (gc[CreditCardTypeType.Visa.ToString()].Success)
				return CreditCardTypeType.Visa;
			else if (gc[CreditCardTypeType.Discover.ToString()].Success)
				return CreditCardTypeType.Discover;
			else
				return null;
		}
	}



	public enum CreditCardTypeType
	{
		Visa,
		MasterCard,
		Discover,
		Amex,
		Switch,
		Solo
	}


	public enum CreditCardTypeTypeIats
	{
		Visa,
		MC,
		DSC,
		Amx,
		MAESTR
	}
}
