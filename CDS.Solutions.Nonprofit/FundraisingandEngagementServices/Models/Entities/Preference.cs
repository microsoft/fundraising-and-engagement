using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_Preference")]
    public partial class Preference : PaymentEntity
    { 
        public Guid preferenceid { get; set; }

        public Guid? preferencecategoryid { get; set; }

        public string name { get; set; }

        public virtual PreferenceCategory PreferenceCategory { get; set; }    
    }
}
