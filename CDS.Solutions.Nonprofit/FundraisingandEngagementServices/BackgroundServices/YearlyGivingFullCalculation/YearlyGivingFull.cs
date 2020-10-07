using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement
{
	public class YearlyGivingFull
	{
		private readonly PaymentContext dataContext;
		private readonly ILogger logger;
		private YearlyGivingCalculator yearlyGivingCalculator;

		public YearlyGivingFull(PaymentContext dataContext, ILogger logger)
		{
			this.logger = logger;
			this.dataContext = dataContext;
			this.yearlyGivingCalculator = new YearlyGivingCalculator(dataContext, logger);
		}

		// this goes through all customers and updates their yearly giving fields
		public void FullRecalculation()
		{
			this.logger.LogInformation("Entering FullRecalculation");

			// The Yearly giving values for Accounts of type Household depend on their individual members,
			// so we iterate through Contacts first
			// get a list of all active/non-deleted contacts
			var contacts = this.dataContext.Contact.Where(c => c.StateCode == 0 && c.Deleted == false).ToList();

			this.logger.LogInformation("Found " + contacts.Count + " individual Contacts to update.");

			foreach (Contact curContact in contacts)
			{
				yearlyGivingCalculator.UpdateIndividualCustomer(curContact);
			}

			// active/non-deleted accounts of type organization
			var organizations = this.dataContext.Account.Where(a => a.msnfp_accounttype == AccountType.Organization && a.StateCode == 0 && a.Deleted == false).ToList();
			this.logger.LogInformation("Found " + organizations.Count + " individual Organizations to update.");

			foreach (Account curOrganization in organizations)
			{
				yearlyGivingCalculator.UpdateIndividualCustomer(curOrganization);
			}

			// active/non-deleted accounts of type organization
			var households = this.dataContext.Account.Where(a => a.msnfp_accounttype == AccountType.Household && a.StateCode == 0 && a.Deleted == false).ToList();
			this.logger.LogInformation("Found " + households.Count + " individual Households to update.");

			foreach (var curHousehold in households)
			{
				// we've already updated the individual household members, so we don't have to do it again
				yearlyGivingCalculator.UpdateHousehold(curHousehold, false);
			}

			this.logger.LogInformation("FullRecalculation done");
		}
	}
}
