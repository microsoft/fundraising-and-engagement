using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentProcessors;

namespace FundraisingandEngagement.Tests.PaymentProcessors
{
	[TestClass]
	public class CreditCardUtilsTests
	{
		[TestMethod]
		public void TestCardTypeDetection()
		{
			var cardType = CreditCardTypeDetection.FromNumber("4012888888881881");

			Assert.AreEqual(CreditCardTypeType.Visa, cardType);
		}

		[TestMethod]
		public void TestIatsCardTypeDetection()
		{
			var cardType = CreditCardTypeDetection.IatsFromNumber("4012888888881881");

			Assert.AreEqual(CreditCardTypeTypeIats.Visa, cardType);
		}
	}
}
