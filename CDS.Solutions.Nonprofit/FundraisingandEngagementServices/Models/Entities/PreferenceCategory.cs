using FundraisingandEngagement.Models.Attributes;
using System;
using System.Collections.Generic;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_PreferenceCategory")]
    public partial class PreferenceCategory : PaymentEntity
    {
        public PreferenceCategory()
        {
            Preference = new HashSet<Preference>();
            EventPreference = new HashSet<EventPreference>();
        }

        public Guid preferencecategoryid { get; set; }

        public string Name { get; set; }

        public int? CategoryCode { get; set; }

        public virtual ICollection<Preference> Preference { get; set; }

        public virtual ICollection<EventPreference> EventPreference { get; set; } 
    }
}
