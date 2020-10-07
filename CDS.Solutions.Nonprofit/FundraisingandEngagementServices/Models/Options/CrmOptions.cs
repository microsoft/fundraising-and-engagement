using System;

namespace FundraisingandEngagement.Models.Options
{
	public class CrmOptions
	{
		public Guid CrmApplicationID { get; set; }

		public string CrmApplicationKey { get; set; }

		public string CrmOrganizationURL { get; set; }

		public Guid CrmTenantId { get; set; }
	}
}
