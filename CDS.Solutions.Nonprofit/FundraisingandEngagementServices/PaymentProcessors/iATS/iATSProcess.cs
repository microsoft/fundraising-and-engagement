using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PaymentProcessors.iATS
{
	internal class iATSProcess
	{
		private readonly HttpClient httpClient;

		public iATSProcess(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		public Task<XmlDocument> CreateCreditCardCustomerCodeAsync(CreateCreditCardCustomerCode obj, CancellationToken cancellationToken = default)
		{
			return ExecuteRequestAsync(obj, "https://www.iatspayments.com/netgate/CustomerLinkv2.asmx", "https://www.iatspayments.com/NetGate/CreateCreditCardCustomerCode", cancellationToken);
		}

		public Task<XmlDocument> GetCustomerCodeDetailAsync(GetCustomerCodeDetail obj, CancellationToken cancellationToken = default)
		{
			return ExecuteRequestAsync(obj, "https://www.iatspayments.com/netgate/CustomerLinkv2.asmx", "https://www.iatspayments.com/NetGate/GetCustomerCodeDetail", cancellationToken);
		}

		public Task<XmlDocument> ProcessCreditCardWithCustomerCodeAsync(ProcessCreditCardWithCustomerCode obj, CancellationToken cancellationToken = default)
		{
			return ExecuteRequestAsync(obj, "https://www.iatspayments.com/netgate/ProcessLinkv2.asmx", "https://www.iatspayments.com/NetGate/ProcessCreditCardWithCustomerCode", cancellationToken);
		}

		public Task<XmlDocument> CreateACHEFTCustomerCodeAsync(CreateACHEFTCustomerCode obj, CancellationToken cancellationToken = default)
		{
			return ExecuteRequestAsync(obj, "https://www.iatspayments.com/netgate/CustomerLinkv2.asmx", "https://www.iatspayments.com/NetGate/CreateACHEFTCustomerCode", cancellationToken);
		}

		public Task<XmlDocument> ProcessACHEFTWithCustomerCodeAsync(ProcessACHEFTWithCustomerCode obj, CancellationToken cancellationToken = default)
		{
			return ExecuteRequestAsync(obj, "https://www.iatspayments.com/netgate/ProcessLinkv2.asmx", "https://www.iatspayments.com/NetGate/ProcessACHEFTWithCustomerCode", cancellationToken);
		}

		public Task<XmlDocument> UpdateCreditCardCustomerCodeAsync(UpdateCreditCardCustomerCode obj, CancellationToken cancellationToken = default)
		{
			return ExecuteRequestAsync(obj, "https://www.iatspayments.com/netgate/CustomerLinkv2.asmx", "https://www.iatspayments.com/NetGate/UpdateCreditCardCustomerCode", cancellationToken);
		}

		public Task<XmlDocument> UpdateACHEFTCustomerCodeAsync(UpdateACHEFTCustomerCode obj, CancellationToken cancellationToken = default)
		{
			return ExecuteRequestAsync(obj, "https://www.iatspayments.com/netgate/CustomerLinkv2.asmx", "https://www.iatspayments.com/NetGate/UpdateACHEFTCustomerCode", cancellationToken);
		}

		public Task<XmlDocument> ProcessCreditCardRefundWithTransactionIdAsync(ProcessCreditCardRefundWithTransactionId obj, CancellationToken cancellationToken = default)
		{
			return ExecuteRequestAsync(obj, "https://www.iatspayments.com/netgate/ProcessLinkv2.asmx", "https://www.iatspayments.com/NetGate/ProcessCreditCardRefundWithTransactionId", cancellationToken);
		}

		public Task<XmlDocument> ProcessACHEFTRefundWithTransactionIdAsync(ProcessACHEFTRefundWithTransactionId obj, CancellationToken cancellationToken = default)
		{
			return ExecuteRequestAsync(obj, "https://www.iatspayments.com/netgate/ProcessLinkv2.asmx", "https://www.iatspayments.com/NetGate/ProcessACHEFTRefundWithTransactionId", cancellationToken);
		}

		public Task<XmlDocument> CheckBankTransactionStatusIATSAsync(GetACHEFTRejectSpecificDateXML obj, CancellationToken cancellationToken = default)
		{
			return ExecuteRequestAsync(obj, "https://www.iatspayments.com/NetGate/ReportLinkv2.asmx", "https://www.iatspayments.com/NetGate/GetACHEFTRejectSpecificDateXML", cancellationToken);
		}

		public Task<XmlDocument> ProcessCreditCardAsync(ProcessCreditCard obj, CancellationToken cancellationToken = default)
		{
			return ExecuteRequestAsync(obj, "https://www.iatspayments.com/netgate/ProcessLinkv2.asmx", "https://www.iatspayments.com/NetGate/ProcessCreditCard", cancellationToken);
		}

		private async Task<XmlDocument> ExecuteRequestAsync<T>(T payload, string serviceURL, string soapAction, CancellationToken cancellationToken = default)
		{
			//Avoid adding <xml node at first line
			var settings = new XmlWriterSettings { OmitXmlDeclaration = true };
			var xmlSerializer = new XmlSerializer(typeof(T), "https://www.iatspayments.com/NetGate/");

			using var sw = new StringWriter();
			using var writer = XmlWriter.Create(sw, settings);
			sw.WriteLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
			sw.WriteLine(@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">");
			sw.WriteLine(@"<soap:Body>");

			// removes namespace
			var xmlns = new XmlSerializerNamespaces();
			xmlns.Add(String.Empty, String.Empty);
			xmlSerializer.Serialize(writer, payload, xmlns);

			sw.WriteLine(@"</soap:Body>");
			sw.WriteLine(@"</soap:Envelope>");

			var httpWebRequest = new HttpRequestMessage(HttpMethod.Post, serviceURL);

			httpWebRequest.Headers.Add("SOAPAction", soapAction);
			httpWebRequest.Headers.ConnectionClose = true;
			httpWebRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
			httpWebRequest.Headers.Add("Media-Type", "application/xml");
			httpWebRequest.Content = new StringContent(sw.ToString(), Encoding.UTF8, "text/xml");

			var httpResponse = await this.httpClient.SendAsync(httpWebRequest, cancellationToken);
			httpResponse.EnsureSuccessStatusCode();

			using var stream = await httpResponse.Content.ReadAsStreamAsync();
			using var xReader = XmlReader.Create(stream);

			var xmlDoc = new XmlDocument();
			xmlDoc.Load(xReader);

			return xmlDoc;
		}
	}
}
