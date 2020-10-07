using System;
using System.Collections.Generic;
using FundraisingandEngagement.Models.Attributes;

namespace FundraisingandEngagement.Models.Entities
{
	[EntityLogicalName("msnfp_termsofreference")]
    public partial class TermsOfReference : PaymentEntity, IIdentifierEntity
    {
        public TermsOfReference()
        {
            Events = new HashSet<Event>();
        }

        [EntityNameMap("msnfp_termsofreferenceid")]
        public Guid TermsOfReferenceId { get; set; }

        [EntityNameMap("msnfp_ccvmessage")]
        public string CcvMessage { get; set; }

        [EntityNameMap("msnfp_covercostsmessage")]
        public string CoverCostsMessage { get; set; }

        [EntityNameMap("msnfp_failuremessage")]
        public string FailureMessage { get; set; }

        [EntityNameMap("msnfp_footer")]
        public string Footer { get; set; }

        [EntityNameMap("msnfp_giftaidacceptence")]
        public string GiftAidAcceptence { get; set; }

        [EntityNameMap("msnfp_giftaiddeclaration")]
        public string GiftAidDeclaration { get; set; }

        [EntityNameMap("msnfp_giftaiddetails")]
        public string GiftAidDetails { get; set; }

        [EntityNameMap("msnfp_privacypolicy")]
        public string PrivacyPolicy { get; set; }

        [EntityNameMap("msnfp_privacyurl")]
        public string PrivacyUrl { get; set; }

        [EntityNameMap("msnfp_showprivacy")]
        public bool? ShowPrivacy { get; set; }

        [EntityNameMap("msnfp_showtermsconditions")]
        public bool? ShowTermsConditions { get; set; }

        [EntityNameMap("msnfp_signup")]
        public string Signup { get; set; }

        [EntityNameMap("msnfp_termsconditions")]
        public string TermsConditions { get; set; }

        [EntityNameMap("msnfp_termsconditionsurl")]
        public string TermsConditionsUrl { get; set; }

        [EntityNameMap("msnfp_Identifier")]
        public string Identifier { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
