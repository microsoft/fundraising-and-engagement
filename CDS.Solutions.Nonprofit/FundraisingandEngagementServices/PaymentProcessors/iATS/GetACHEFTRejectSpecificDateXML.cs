using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PaymentProcessors.iATS
{
	public class GetACHEFTRejectSpecificDateXML
	{
		public string agentCode
		{
			get; set;
		}
		public string password
		{
			get; set;
		}
		public string customerIPAddress
		{
			get; set;
		}
		public string date
		{
			get; set;
		}
	}

	[DataContract]
	[XmlRoot("TN")]
	public class ACHTransaction
	{
		[DataMember]
		[XmlElement("TNTYP")]
		public string TransType { get; set; }
		[DataMember]
		[XmlElement("TNID")]
		public string TransactionId { get; set; }
		[DataMember]
		[XmlElement("AGT")]
		public string Agentcode { get; set; }
		[DataMember]
		[XmlElement("CST")]
		public iATSCustomer Customer { get; set; }
		[DataMember]
		[XmlElement("ACH")]
		public BankACH BankAccount { get; set; }
		[DataMember]
		[XmlElement("INV")]
		public string InvoiceNo { get; set; }
		[DataMember]
		[XmlElement("DTM")]
		public string Datetime { get; set; }
		[DataMember]
		[XmlElement("AMT")]
		public decimal Amount { get; set; }
		[DataMember]
		[XmlElement("RST")]
		public string TransactionResult { get; set; }
		[DataMember]
		[XmlElement("CM")]
		public string Comment { get; set; }
		[DataMember]
		public string RE { get; set; }
		[DataMember]
		public string ANM { get; set; }
		[DataMember]
		public string IT1 { get; set; }
		[DataMember]
		public string IT2 { get; set; }
		[DataMember]
		public string IT3 { get; set; }
		[DataMember]
		public string IT4 { get; set; }
		[DataMember]
		public string IT5 { get; set; }
		[DataMember]
		public string IT6 { get; set; }
	}

	[DataContract]
	[XmlRoot("CST")]
	public class iATSCustomer
	{

		[DataMember]
		[XmlElement("CSTC")]
		public string CustomerCode { get; set; }
		[DataMember]
		[XmlElement("FN")]
		public string Firstname { get; set; }
		[DataMember]
		[XmlElement("LN")]
		public string Lastname { get; set; }
		[DataMember]
		[XmlElement("ADD")]
		public string Address { get; set; }
		[DataMember]
		[XmlElement("CTY")]
		public string City { get; set; }
		[DataMember]
		[XmlElement("ST")]
		public string State { get; set; }
		[DataMember]
		[XmlElement("CNT")]
		public string Country { get; set; }
		[DataMember]
		[XmlElement("ZC")]
		public string Zipcode { get; set; }
		[DataMember]
		[XmlElement("PH")]
		public string Phone { get; set; }
		[DataMember]
		[XmlElement("MB")]
		public string Mobile { get; set; }
		[DataMember]
		[XmlElement("EM")]
		public string Email { get; set; }
	}

	[DataContract]
	[XmlRoot("ACH")]
	public class BankACH
	{
		[DataMember]
		[XmlElement("ACN")]
		public string AccountNo { get; set; }
		[DataMember]
		[XmlElement("ACTYP")]
		public string AccountType { get; set; }
	}
}