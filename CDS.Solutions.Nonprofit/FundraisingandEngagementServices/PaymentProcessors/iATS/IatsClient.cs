using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using PaymentProcessors.Models;

namespace PaymentProcessors.iATS
{
	internal class IatsClient : IPaymentClient<IatsCreditCardPaymentInput, PaymentOutput>
	{
		private readonly iATSProcess iATSProcess;

		public IatsClient(IHttpClientFactory httpClientFactory)
		{
			var httpClient = httpClientFactory.CreateClient("PaymentProcessor");
			this.iATSProcess = new iATSProcess(httpClient);
		}

		public async Task<PaymentOutput> MakePaymentAsync(IatsCreditCardPaymentInput input, CancellationToken cancellationToken = default)
		{
			var customerXmlDoc = await CreateCustomerProfileAsync(input, cancellationToken);

			var response = new PaymentOutput();

			if (customerXmlDoc != null)
			{
				response.CardType = CreditCardTypeDetection.IatsFromNumber(input.CreditCardNo)?.ToString();

				var customerCode = customerXmlDoc.GetElementsByTagName("CUSTOMERCODE")[0].InnerText;

				if (!String.IsNullOrEmpty(customerCode))
				{
					XmlDocument transactionXmlDoc;

					var str = "MIS" + StringUtils.RandomString(6).ToUpper();

					if (input.IsBankProcess)
					{
						var obj = new ProcessACHEFTWithCustomerCode();

						obj.agentCode = input.IatsAgentCode;
						obj.password = input.IatsPassword;
						obj.customerIPAddress = input.IpAddress;

						obj.customerCode = customerXmlDoc.GetElementsByTagName("CUSTOMERCODE")[0].InnerText;
						obj.total = String.Format("{0:0.00}", input.Amount);
						obj.invoiceNum = str;

						response.InvoiceNumber = str;
						transactionXmlDoc = await this.iATSProcess.ProcessACHEFTWithCustomerCodeAsync(obj, cancellationToken);
					}
					else
					{
						var obj = new ProcessCreditCardWithCustomerCode();

						obj.agentCode = input.IatsAgentCode;
						obj.password = input.IatsPassword;
						obj.customerIPAddress = input.IpAddress;

						obj.customerCode = customerXmlDoc.GetElementsByTagName("CUSTOMERCODE")[0].InnerText;
						obj.total = String.Format("{0:0.00}", input.Amount);
						obj.invoiceNum = str;

						response.InvoiceNumber = str;
						transactionXmlDoc = await this.iATSProcess.ProcessCreditCardWithCustomerCodeAsync(obj, cancellationToken);
					}

					if (transactionXmlDoc != null)
					{
						SetProcessTransactionResposne(response, transactionXmlDoc);
						response.AuthToken = customerCode;
					}
				}
				else
				{
					response.IsSuccessful = false;
					response.TransactionResult = customerXmlDoc.GetElementsByTagName("AUTHORIZATIONRESULT")[0].InnerText;
				}
			}

			return response;
		}

		private async Task<XmlDocument> CreateCustomerProfileAsync(IatsCreditCardPaymentInput input, CancellationToken cancellationToken)
		{
			if (input.IsBankProcess)
			{
				var iatsBankCustomerProfile = new CreateACHEFTCustomerCode();

				iatsBankCustomerProfile.agentCode = input.IatsAgentCode;
				iatsBankCustomerProfile.password = input.IatsPassword;
				iatsBankCustomerProfile.customerIPAddress = input.IpAddress;
				iatsBankCustomerProfile.firstName = input.FirstName;
				iatsBankCustomerProfile.lastName = input.LastName;
				iatsBankCustomerProfile.address = input.BillingLine1;
				iatsBankCustomerProfile.city = input.BillingCity;
				iatsBankCustomerProfile.state = input.BillingStateorProvince;
				iatsBankCustomerProfile.zipCode = input.BillingPostalCode;
				iatsBankCustomerProfile.phone = input.Telephone;
				iatsBankCustomerProfile.email = input.EmailAddress;

				iatsBankCustomerProfile.recurring = input.IsRecurring;
				iatsBankCustomerProfile.beginDate = DateTime.Now;
				iatsBankCustomerProfile.endDate = DateTime.Now;

				var accNo = input.AccountNumber;
				if (input.AccountNumberFormat == AccountNumberFormat.BankAndTransitNumber)
					iatsBankCustomerProfile.accountNum = input.BankNumber + input.TransitNumber + accNo;
				else if (input.AccountNumberFormat == AccountNumberFormat.RoutingNumber)
					iatsBankCustomerProfile.accountNum = input.RoutingNumber + accNo;

				if (input.AccountType == BankAccountType.Checking)
					iatsBankCustomerProfile.accountType = "CHECKING";
				else if (input.AccountType == BankAccountType.Saving)
					iatsBankCustomerProfile.accountType = "SAVING";

				return await this.iATSProcess.CreateACHEFTCustomerCodeAsync(iatsBankCustomerProfile, cancellationToken);
			}
			else
			{
				var iatsCustomerProfile = new CreateCreditCardCustomerCode();

				iatsCustomerProfile.agentCode = input.IatsAgentCode;
				iatsCustomerProfile.password = input.IatsPassword;
				iatsCustomerProfile.customerIPAddress = input.IpAddress;
				iatsCustomerProfile.firstName = input.FirstName;
				iatsCustomerProfile.lastName = input.LastName;
				iatsCustomerProfile.address = input.BillingLine1;
				iatsCustomerProfile.city = input.BillingCity;
				iatsCustomerProfile.state = input.BillingStateorProvince;
				iatsCustomerProfile.zipCode = input.BillingPostalCode;
				iatsCustomerProfile.phone = input.Telephone;
				iatsCustomerProfile.email = input.EmailAddress;

				iatsCustomerProfile.recurring = input.IsRecurring;
				iatsCustomerProfile.beginDate = DateTime.Now;
				iatsCustomerProfile.endDate = DateTime.Now;
				iatsCustomerProfile.creditCardNum = input.CreditCardNo;
				iatsCustomerProfile.comment = "Created by CRM Azure App on " + DateTime.Now.ToString();
				var mm = input.CcExpMmYy.Substring(0, 2);
				var yy = input.CcExpMmYy.Substring(2, 2);
				iatsCustomerProfile.creditCardExpiry = mm + "/" + yy;

				return await this.iATSProcess.CreateCreditCardCustomerCodeAsync(iatsCustomerProfile, cancellationToken);
			}
		}

		private void SetProcessTransactionResposne(PaymentOutput response, XmlDocument transactionXmlDoc)
		{
			var xnListTrans = transactionXmlDoc.GetElementsByTagName("AUTHORIZATIONRESULT");//You can get any Name here.

			foreach (XmlNode itemTrans in xnListTrans)
			{
				var authResultTrans = itemTrans.InnerText;
				if (authResultTrans.Contains("OK"))
				{
					response.IsSuccessful = true;
					response.TransactionResult = transactionXmlDoc.GetElementsByTagName("AUTHORIZATIONRESULT")[0].InnerText;
					response.TransactionIdentifier = transactionXmlDoc.GetElementsByTagName("TRANSACTIONID")[0].InnerText;
				}
				else
				{
					response.IsSuccessful = false;
					response.TransactionResult = transactionXmlDoc.GetElementsByTagName("AUTHORIZATIONRESULT")[0].InnerText;
					response.TransactionIdentifier = transactionXmlDoc.GetElementsByTagName("TRANSACTIONID")[0].InnerText;
				}
			}
		}
	}
}
