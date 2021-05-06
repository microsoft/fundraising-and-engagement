using System;
using System.Linq;
using FundraisingandEngagement.BackgroundServices;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement
{
    public class YearlyGivingFull
    {
        Func<PaymentContext> _makePaymentContext;
        private readonly ILogger _logger;
        private readonly IYearlyGivingCalculator _yearlyGivingCalculator;

        public YearlyGivingFull(Func<PaymentContext> makePaymentContext, IYearlyGivingCalculator yearlyGivingCalculator, ILoggerFactory loggerFactory)
        {
            this._makePaymentContext = makePaymentContext;
            this._logger = loggerFactory.CreateLogger<YearlyGivingFull>();
            this._yearlyGivingCalculator = yearlyGivingCalculator;
        }

        // this goes through all customers and updates their yearly giving fields
        public void FullRecalculation()
        {
            this._logger.LogInformation("Entering FullRecalculation");

            this._yearlyGivingCalculator.SynchronizeEntitiesForYearlyGiving();

            // Work-around for bug #3920, Dispose the _dataContext early
            using (var dataContext = this._makePaymentContext())
            {
                // The Yearly giving values for Accounts of type Household depend on their individual members,
                // so we iterate through Contacts first
                // get a list of all active/non-deleted contacts
                var contacts = dataContext.Contact.Where(c => c.StateCode == 0 && c.Deleted == false).ToList();

                this._logger.LogInformation("Found " + contacts.Count + " individual Contacts to update.");

                foreach (Contact curContact in contacts)
                {
                    this._yearlyGivingCalculator.UpdateIndividualCustomer(curContact);
                }
            }

            using (var dataContext = this._makePaymentContext())
            {
                // active/non-deleted accounts of type organization
                var organizations = dataContext.Account.Where(a => a.msnfp_accounttype == AccountType.Organization && a.StateCode == 0 && a.Deleted == false).ToList();
                this._logger.LogInformation("Found " + organizations.Count + " individual Organizations to update.");

                foreach (Account curOrganization in organizations)
                {
                    this._yearlyGivingCalculator.UpdateIndividualCustomer(curOrganization);
                }
            }

            using (var dataContext = this._makePaymentContext())
            {
                // active / non - deleted accounts of type organization
                var households = dataContext.Account.Where(a => a.msnfp_accounttype == AccountType.Household && a.StateCode == 0 && a.Deleted == false).ToList();
                this._logger.LogInformation("Found " + households.Count + " individual Households to update.");

                foreach (var curHousehold in households)
                {
                   // we've already updated the individual household members, so we don't have to do it again
                   this._yearlyGivingCalculator.UpdateHousehold(curHousehold, false);
                }
            }

            this._logger.LogInformation("FullRecalculation done");
        }
    }
}