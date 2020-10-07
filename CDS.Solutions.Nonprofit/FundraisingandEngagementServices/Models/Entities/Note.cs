using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("annotation")]
	public partial class Note : PaymentEntity
	{
		[EntityNameMap("annotationid")]
		public Guid NoteId { get; set; }

		[EntityNameMap("documentbody")]
		public string Document { get; set; }

		[EntityNameMap("filename")]
		public string FileName { get; set; }

		[EntityNameMap("filesize")]
		public int? FileSize { get; set; }

		[EntityNameMap("isdocument")]
		public bool? IsDocument { get; set; }

		[EntityNameMap("notetext")]
		public string Description { get; set; }

		[EntityLogicalName("msnfp_bankrun")]
		[EntityReferenceMap("objectid_msnfp_bankrun")]
		public Guid? RegardingObjectId { get; set; }

		[EntityNameMap("objecttypecode")]
		public string ObjectType { get; set; }

		[EntityNameMap("subject")]
		public string Title { get; set; }

		[EntityNameMap("mimetype")]
		public string MimeType { get; set; }

		public virtual BankRun RegardingObject { get; set; }
	}
}
