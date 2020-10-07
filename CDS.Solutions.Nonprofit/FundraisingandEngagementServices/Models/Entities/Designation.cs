using System;
using System.Collections.Generic;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_Designation")]
    public partial class Designation : PaymentEntity
    {
        [EntityNameMap("msnfp_DesignationId")]
        public Guid DesignationId { get; set; }

        [EntityNameMap("msnfp_Name")]
        public string Name { get; set; }

		public virtual ICollection<Event> Events { get; set; }
	}
}
