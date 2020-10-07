using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_receiptlog")]
    public partial class ReceiptLog : PaymentEntity, IIdentifierEntity
    {
        [EntityNameMap("msnfp_receiptlogid")]
        public Guid ReceiptLogId { get; set; }

        [EntityReferenceMap("msnfp_ReceiptStackId")]
        [EntityLogicalName("msnfp_ReceiptStack")]
        public Guid? ReceiptStackId { get; set; }

        [EntityNameMap("msnfp_EntryBy")]
        public string EntryBy { get; set; }

        [EntityNameMap("msnfp_EntryReason")]
        public string EntryReason { get; set; }

        [EntityNameMap("msnfp_ReceiptNumber")]
        public string ReceiptNumber { get; set; }

		[EntityNameMap("msnfp_Identifier")]
		public string Identifier { get; set; }

        public virtual ReceiptStack ReceiptStack { get; set; }
    }
}
