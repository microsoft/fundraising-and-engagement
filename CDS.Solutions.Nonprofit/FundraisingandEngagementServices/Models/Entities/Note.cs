using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
    [EntityLogicalName("annotation")]
    public partial class Note : PaymentEntity
    {
        [EntityNameMap("annotationid", PushToDataverse = true)]
        public Guid NoteId { get; set; }

        [EntityNameMap("documentbody", PushToDataverse = true)]
        public string Document { get; set; }

        [EntityNameMap("filename", PushToDataverse = true)]
        public string FileName { get; set; }

        [EntityNameMap("filesize")]
        public int? FileSize { get; set; }

        [EntityNameMap("isdocument", PushToDataverse = true)]
        public bool? IsDocument { get; set; }

        [EntityNameMap("notetext", PushToDataverse = true)]
        public string Description { get; set; }

        [EntityLogicalName("msnfp_bankrun")]
        [EntityReferenceMap("objectid_msnfp_bankrun", PushToDataverse = true)]
        public Guid? RegardingObjectId { get; set; }

        [EntityNameMap("objecttypecode", PushToDataverse = true)]
        public string ObjectType { get; set; }

        [EntityNameMap("subject", PushToDataverse = true)]
        public string Title { get; set; }

        [EntityNameMap("mimetype", PushToDataverse = true)]
        public string MimeType { get; set; }

        public virtual BankRun RegardingObject { get; set; }
    }
}