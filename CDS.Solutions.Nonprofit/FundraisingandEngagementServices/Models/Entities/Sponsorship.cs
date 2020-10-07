using System;
using System.ComponentModel.DataAnnotations.Schema;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_Sponsorship")]
    public partial class Sponsorship : ContactPaymentEntity, IIdentifierEntity
    {
        [EntityNameMap("msnfp_Sponsorshipid")]
        public Guid SponsorshipId { get; set; }

        [EntityReferenceMap("msnfp_EventId")]
        [EntityLogicalName("msnfp_Event")]
        public Guid? EventId { get; set; }

        [EntityReferenceMap("msnfp_EventPackageId")]
        [EntityLogicalName("msnfp_EventPackage")]
        public Guid? EventPackageId { get; set; }

        [EntityReferenceMap("msnfp_EventSponsorshipId")]
        [EntityLogicalName("msnfp_EventSponsorship")]
        public Guid? EventSponsorshipId { get; set; }

		[EntityNameMap("msnfp_Amount_Receipted")]
        [Column(TypeName = "money")]
        public decimal? AmountReceipted { get; set; }

        [EntityNameMap("msnfp_Amount_Nonreceiptable")]
        [Column(TypeName = "money")]
        public decimal? AmountNonreceiptable { get; set; }

        [EntityNameMap("msnfp_Amount_Tax")]
        [Column(TypeName = "money")]
        public decimal? AmountTax { get; set; }

        [EntityNameMap("msnfp_Amount")]
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        [EntityNameMap("msnfp_Date", Format = "yyyy-MM-dd")]
        public DateTime? Date { get; set; }

        [EntityNameMap("msnfp_Description")]
        public string Description { get; set; }

        [EntityNameMap("msnfp_Name")]
        public string Name { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        public virtual Event Event { get; set; }

        public virtual EventPackage EventPackage { get; set; }

        public virtual EventSponsorship EventSponsorship { get; set; }
    }
}
