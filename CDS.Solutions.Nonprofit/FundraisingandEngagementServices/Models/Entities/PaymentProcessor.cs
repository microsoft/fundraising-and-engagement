using System;
using System.Collections.Generic;
using FundraisingandEngagement.Models.Attributes;
using FundraisingandEngagement.Models.Enums;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_PaymentProcessor")]
    public partial class PaymentProcessor : PaymentEntity, IIdentifierEntity
    {
        public PaymentProcessor()
        {
            Configuration = new HashSet<Configuration>();
            PaymentMethod = new HashSet<PaymentMethod>();
        }


        [EntityNameMap("msnfp_paymentprocessorid")]
        public Guid PaymentProcessorId { get; set; }
		
		[EntityOptionSetMap("msnfp_BankRunFileFormat")]
		public int? BankRunFileFormat { get; set; }

		[EntityNameMap("msnfp_Name")]
        public string Name { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        [EntityNameMap("msnfp_updated")]
        public DateTime? Updated { get; set; }

        [EntityOptionSetMap("msnfp_PaymentGatewayType")]
        public PaymentGatewayCode PaymentGatewayType { get; set; }

        [EntityNameMap("msnfp_AdyenMerchantAccount")]
        public string AdyenMerchantAccount { get; set; }

        [EntityNameMap("msnfp_AdyenPassword")]
        public string AdyenUsername { get; set; }

        [EntityNameMap("msnfp_AdyenPassword")]
        public string AdyenPassword { get; set; }

        [EntityNameMap("msnfp_AdyenURL")]
        public string AdyenUrl { get; set; }

        [EntityNameMap("msnfp_AdyenCheckoutUrl")]
        public string AdyenCheckoutUrl { get; set; }

        [EntityNameMap("msnfp_iATSAgentCode")]
        public string IatsAgentCode { get; set; }

        [EntityNameMap("msnfp_iATSPassword")]
        public string IatsPassword { get; set; }

        [EntityNameMap("msnfp_StoreId")]
        public string MonerisStoreId { get; set; }

        [EntityNameMap("msnfp_ApiKey")]
        public string MonerisApiKey { get; set; }

        [EntityNameMap("msnfp_TestMode")]
        public bool? MonerisTestMode { get; set; }

        [EntityNameMap("msnfp_StripeServiceKey")]
        public string StripeServiceKey { get; set; }

        [EntityNameMap("msnfp_WorldPayServiceKey")]
        public string WorldPayServiceKey { get; set; }

        [EntityNameMap("msnfp_WorldPayClientKey")]
        public string WorldPayClientKey { get; set; }

        [EntityNameMap("msnfp_scotiabankcustomernumber")]
        public string ScotiabankCustomerNumber { get; set; }

        [EntityNameMap("msnfp_originatorshortname")]
        public string OriginatorShortName { get; set; }

        [EntityNameMap("msnfp_originatorlongname")]
        public string OriginatorLongName { get; set; }

        [EntityNameMap("msnfp_bmooriginatorid")]
        public string BmoOriginatorId { get; set; }

		[EntityNameMap("msnfp_abaremittername")]
		public string AbaRemitterName { get; set; }

		[EntityNameMap("msnfp_abausername")]
		public string AbaUserName { get; set; }

		[EntityNameMap("msnfp_abausernumber")]
		public string AbaUserNumber { get; set; }


        public virtual ICollection<Configuration> Configuration { get; set; }

        public virtual ICollection<PaymentMethod> PaymentMethod { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
