using System;
using FundraisingandEngagement.Models.Entities;

namespace FundraisingandEngagement.BackgroundServices
{
    public interface IYearlyGivingCalculator
    {
        void UpdateCustomer(Guid? customerId, int? customerIdType);
        void SynchronizeEntitiesForYearlyGiving();
        void UpdateHousehold(Account household, Boolean updateIndividualMembers = true);
        void UpdateIndividualCustomer(ICustomer customer);
    }
}