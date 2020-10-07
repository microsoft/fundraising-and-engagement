using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_pageorder")]
    public class PageOrder : PaymentEntity
    {
        public PageOrder()
        {

        }

        [EntityNameMap("msnfp_pageorderid")]
        public Guid PageOrderId { get; set; }

        //[EntityReferenceMap("createdby")]
        //[EntityLogicalName("systemuser")]
        //public Guid? CreatedBy { get; set; }

        //in the base class
        //[EntityNameMap("createdon")]
        //public DateTime? CreatedOn { get; set; }

        //[EntityNameMap("importsequencenumber")]
        //public int? ImportSequenceNumber { get; set; }

        [EntityNameMap("msnfp_order")]
        public int? Order{ get; set; }

        [EntityNameMap("msnfp_orderdate")]
        public DateTime? OrderDate { get; set; }

        [EntityNameMap("msnfp_title")]
        public string Title { get; set; }

        //[EntityReferenceMap("systemuser")]
        //[EntityNameMap("modifiedby")]
        //public Guid? ModifiedBy { get; set; }

        //[EntityNameMap("modifiedon")]
        //public DateTime? ModifiedOn { get; set; }

        //[EntityNameMap("overriddencreatedon")]
        //public DateTime? RecordCreatedOn { get; set; }

        //[EntityReferenceMap("systemuser")]
        //[EntityNameMap("ownerid")]
        //public DateTime? Owner { get; set; }

        //[EntityReferenceMap("systemuser")]
        //[EntityNameMap("owningbusinessunit")]
        //public DateTime? OwningBusinessUnit { get; set; }

        //[EntityReferenceMap("systemuser")]
        //[EntityNameMap("owningteam")]
        //public DateTime? OwningTeam { get; set; }

        //[EntityReferenceMap("systemuser")]
        //[EntityNameMap("owninguser")]
        //public DateTime? OwningUser { get; set; }

        [EntityOptionSetMap("statecode")]
        public int? Status { get; set; }

        [EntityOptionSetMap("statuscode")]
        public int? StatusReason { get; set; }

        //[EntityNameMap("timezoneruleversionnumber")]
        //public int? TimeZoneVersionNumber { get; set; }

        //[EntityNameMap("utcconversiontimezonecode")]
        //public int? UTCConversionTimeZoneCode{ get; set; }

        //[EntityNameMap("versionnumber")]
        //public Int64? VersionNumber { get; set; }

    }
}
