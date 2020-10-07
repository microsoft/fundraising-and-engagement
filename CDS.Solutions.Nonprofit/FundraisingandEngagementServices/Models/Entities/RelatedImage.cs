using FundraisingandEngagement.Models.Attributes;
using FundraisingandEngagement.Models.Entities;
using System;

namespace FundraisingandEngagement
{
	[EntityLogicalName("msnfp_relatedimage")]
    public partial class RelatedImage : PaymentEntity, IIdentifierEntity
    {
        [EntityNameMap("msnfp_relatedimageid")]
        public Guid RelatedImageId { get; set; }

        [EntityNameMap("msnfp_smallimage")]
        public string SmallImage { get; set; }

        [EntityNameMap("msnfp_lastpublished")]
        public DateTime? LastPublished { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }
    }
}
