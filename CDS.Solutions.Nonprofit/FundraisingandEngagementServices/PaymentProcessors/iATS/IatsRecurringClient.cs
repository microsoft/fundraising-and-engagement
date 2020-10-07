using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using PaymentProcessors.Models;

namespace PaymentProcessors.iATS
{
	internal class IatsRecurringClient : IPaymentClient<IatsRecurringPaymentInput, IatsRecurringPaymentOutput>
	{
		private readonly iATSProcess iATSProcess;

		public IatsRecurringClient(IHttpClientFactory httpClientFactory)
		{
			var httpClient = httpClientFactory.CreateClient("PaymentProcessor");
			this.iATSProcess = new iATSProcess(httpClient);
		}

		public async Task<IatsRecurringPaymentOutput> MakePaymentAsync(IatsRecurringPaymentInput input, CancellationToken cancellationToken = default)
		{
			XmlDocument xml;

			if (input.IsBankAccountPayment)
				xml = await PayIatsRecurringBankAccount(input, cancellationToken);
			else
				xml = await PayIatsRecurringCreditCard(input, cancellationToken);

			var output = new IatsRecurringPaymentOutput(input);

			var xnList = xml.GetElementsByTagName("AUTHORIZATIONRESULT");//You can get any Name here.

			foreach (XmlNode xNode in xnList)
			{
				var authResult = xNode.InnerText;

				if (authResult.Contains("OK"))
				{
					//Completed Donation
					output.IsSuccessful = true;

					if (xml.GetElementsByTagName("STATUS").Count > 0)
						output.TransactionStatus = xml.GetElementsByTagName("STATUS")[0].InnerText;
					if (xml.GetElementsByTagName("AUTHORIZATIONRESULT").Count > 0)
						output.TransactionResult = xml.GetElementsByTagName("AUTHORIZATIONRESULT")[0].InnerText;
					if (xml.GetElementsByTagName("TRANSACTIONID").Count > 0)
						output.TransactionIdentifier = xml.GetElementsByTagName("TRANSACTIONID")[0].InnerText;
				}
				else // not OK
				{
					//Failed Donation
					output.SetFailure(input);
					output.TransactionStatus = "Failed";

					if (xml.GetElementsByTagName("TRANSACTIONID").Count > 0)
						output.TransactionIdentifier = xml.GetElementsByTagName("TRANSACTIONID")[0].InnerText;
					if (xml.GetElementsByTagName("AUTHORIZATIONRESULT").Count > 0)
						output.TransactionResult = xml.GetElementsByTagName("AUTHORIZATIONRESULT")[0].InnerText;
				}
			}

			return output;
		}

		private Task<XmlDocument> PayIatsRecurringBankAccount(IatsRecurringPaymentInput input, CancellationToken cancellationToken)
		{
			return this.iATSProcess.ProcessACHEFTWithCustomerCodeAsync(SetAcheft(input), cancellationToken);
		}

		private Task<XmlDocument> PayIatsRecurringCreditCard(IatsRecurringPaymentInput input, CancellationToken cancellationToken)
		{
			return this.iATSProcess.ProcessCreditCardWithCustomerCodeAsync(SetIatsCreditCard(input), cancellationToken);
		}

		private ProcessACHEFTWithCustomerCode SetAcheft(IatsRecurringPaymentInput input)
		{
			var obj = new ProcessACHEFTWithCustomerCode();

			obj.agentCode = input.AgentCode;
			obj.password = input.Password;
			obj.customerIPAddress = !String.IsNullOrEmpty(input.IpAddress) ? input.IpAddress : "127.0.0.1";

			obj.customerCode = input.IatsCustomerCode;
			obj.invoiceNum = input.InvoiceIdentifier;
			obj.total = String.Format("{0:0.00}", input.Amount);
			obj.comment = "Debited by Azure on " + DateTime.Now.ToString();

			return obj;
		}

		private static ProcessCreditCardWithCustomerCode SetIatsCreditCard(IatsRecurringPaymentInput input)
		{
			var obj = new ProcessCreditCardWithCustomerCode();

			obj.agentCode = input.AgentCode;
			obj.password = input.Password;
			obj.customerIPAddress = !String.IsNullOrEmpty(input.IpAddress) ? input.IpAddress : "127.0.0.1";

			obj.customerCode = input.IatsCustomerCode;
			obj.invoiceNum = input.InvoiceIdentifier;
			obj.total = String.Format("{0:0.00}", input.Amount);
			obj.comment = "Debited by Azure on " + DateTime.Now.ToString();

			return obj;
		}
	}
}
