using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_tributeormemory")]
    public partial class TributeOrMemory : PaymentEntity, IIdentifierEntity
    {
        [EntityNameMap("msnfp_tributeormemoryid")]
        public Guid TributeOrMemoryId { get; set; }

        [EntityNameMap("msnfp_name")]
        public string Name { get; set; }

        [EntityNameMap("msnfp_identifier")]
        public string Identifier { get; set; }

        [EntityOptionSetMap("msnfp_tributeormemory")]
        public int? TributeOrMemoryTypeCode { get; set; }
    }
}
