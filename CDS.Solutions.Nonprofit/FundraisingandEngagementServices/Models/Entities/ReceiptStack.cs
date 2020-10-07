using System;
using System.Collections.Generic;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_receiptstack")]
    public partial class ReceiptStack : PaymentEntity, IIdentifierEntity
    {
        public ReceiptStack()
        {
            Receipt = new HashSet<Receipt>();
            ReceiptLog = new HashSet<ReceiptLog>();
        }

        [EntityNameMap("msnfp_receiptstackid")]
        public Guid ReceiptStackId { get; set; }

        [EntityReferenceMap("msnfp_configurationid")]
        [EntityLogicalName("msnfp_configuration")]
        public Guid? ConfigurationId { get; set; }

        [EntityNameMap("msnfp_currentrange")]
        public double? CurrentRange { get; set; }

        [EntityOptionSetMap("msnfp_numberrange")]
        public int? NumberRange { get; set; }

        [EntityNameMap("msnfp_prefix")]
        public string Prefix { get; set; }

        [EntityOptionSetMap("msnfp_receiptyear")]
        public int? ReceiptYear { get; set; }

        [EntityNameMap("msnfp_startingrange")]
        public double? StartingRange { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

		public Guid? OwningBusinessUnitId { get; set; }

		public virtual Configuration Configuration { get; set; }

        public virtual ICollection<Receipt> Receipt { get; set; }

        public virtual ICollection<ReceiptLog> ReceiptLog { get; set; }
    }
}
