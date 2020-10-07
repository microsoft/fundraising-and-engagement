using FundraisingandEngagement.Models.Attributes;
using System;
using System.Collections.Generic;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_Configuration")]
    public partial class Configuration : PaymentEntity, IIdentifierEntity
    {
        public Configuration()
        {
            Event = new HashSet<Event>();
            EventPackage = new HashSet<EventPackage>();
            PaymentSchedule = new HashSet<PaymentSchedule>();
            ReceiptStack = new HashSet<ReceiptStack>();
            Transaction = new HashSet<Transaction>();
        }


        [EntityNameMap("msnfp_Configurationid")]
        public Guid ConfigurationId { get; set; }



        [EntityReferenceMap("msnfp_TeamOwnerId")]
        [EntityLogicalName("Team")]
        public Guid? TeamOwnerId { get; set; }

        [EntityLogicalName("msnfp_PaymentProcessor")]
        [EntityReferenceMap("msnfp_PaymentProcessorId")]
        public Guid? PaymentProcessorId { get; set; }

		public bool ShouldSyncResponse { get; set; }

        [EntityNameMap("msnfp_AddressAuth1")]
        public string AddressAuth1 { get; set; }

        [EntityNameMap("msnfp_AddressAuth2")]
        public string AddressAuth2 { get; set; }

        [EntityNameMap("msnfp_Azure_WebApiUrl")]
        public string AzureWebApiUrl { get; set; }

        [EntityNameMap("msnfp_BankRun_PregeneratedBy")]
        public int? BankRunPregeneratedBy { get; set; }

        [EntityNameMap("msnfp_CharityTitle")]
        public string CharityTitle { get; set; }

        [EntityNameMap("msnfp_Azure_WebApp")]
        public string AzureWebApp { get; set; }

        [EntityNameMap("msnfp_Sche_MaxRetries")]
        public int? ScheMaxRetries { get; set; }

        [EntityOptionSetMap("msnfp_Sche_RecurrenceStart")]
        public int? ScheRecurrenceStart { get; set; }

        [EntityNameMap("msnfp_Sche_Retryinterval")]
        public int? ScheRetryinterval { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        [EntityNameMap("msnfp_defaultconfiguration")]
        public bool? DefaultConfiguration { get; set; }



        public virtual PaymentProcessor PaymentProcessor { get; set; }
        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<EventPackage> EventPackage { get; set; }
        public virtual ICollection<PaymentSchedule> PaymentSchedule { get; set; }
        public virtual ICollection<ReceiptStack> ReceiptStack { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
