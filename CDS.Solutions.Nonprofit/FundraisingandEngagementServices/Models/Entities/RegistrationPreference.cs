using System;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_RegistrationPreference")]
    public partial class RegistrationPreference : PaymentEntity
    {  
        public Guid RegistrationPreferenceId { get; set; }
        public Guid RegistrationId { get; set; }
        public Guid eventpreference { get; set; }
        public Guid EventId { get; set; }
        public string Other { get; set; }
    }
}
