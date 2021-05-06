using System;
using System.Linq;
using Data.Utils;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement.BackgroundServices
{
    public class YearlyGivingFromEntity
    {
        private readonly PaymentContext _dataContext;
        private readonly ILogger _logger;
        private readonly IYearlyGivingCalculator _yearlyGivingCalculator;

        public YearlyGivingFromEntity(PaymentContext dataContext, ILogger logger, IYearlyGivingCalculator yearlyGivingCalculator)
        {
            this._logger = logger;
            this._dataContext = dataContext;
            this._yearlyGivingCalculator = yearlyGivingCalculator;
        }

        public void CalculateFromPaymentEntity(Guid entityId, string entityName)
        {
            this._logger.LogInformation($"Entering CalculateFromPaymentEntity with entityId {entityId} and entityName {entityName}");

            this._yearlyGivingCalculator.SynchronizeEntitiesForYearlyGiving();

            var contactPaymentEntity = getEntityToUpdate(entityId, entityName.ToLower());
            if (contactPaymentEntity == null)
            {
                this._logger.LogError($"No record found of type {entityName} with Id: {entityId}. Exiting.");
                return;
            }

            this._yearlyGivingCalculator.UpdateCustomer(contactPaymentEntity.CustomerId, contactPaymentEntity.CustomerIdType);
            this._logger.LogInformation("CalculateFromPaymentEntity done.");
        }

        private IContactPaymentEntity? getEntityToUpdate(Guid entityId, string entityName)
        {
            if (entityName == typeof(Account).EntityLogicalName())
            {
                return SimpleContactPaymentEntity.Account(entityId);
            }

            if (entityName == typeof(Contact).EntityLogicalName())
            {
                return SimpleContactPaymentEntity.Contact(entityId);
            }

            if (entityName == typeof(Transaction).EntityLogicalName())
            {
                return this._dataContext.Transaction.FirstOrDefault(x => x.TransactionId == entityId);
            }

            if (entityName == typeof(EventPackage).EntityLogicalName())
            {
                return this._dataContext.EventPackage.FirstOrDefault(x => x.EventPackageId == entityId);
            }

            if (entityName == typeof(DonorCommitment).EntityLogicalName())
            {
                return this._dataContext.DonorCommitment.FirstOrDefault(x => x.DonorCommitmentId == entityId);
            }

            return null;
        }

        private class SimpleContactPaymentEntity : IContactPaymentEntity
        {
            public Guid? CustomerId { get; set; }
            public int? CustomerIdType { get; set; }

            public static SimpleContactPaymentEntity Contact(Guid contactId)
            {
                return new SimpleContactPaymentEntity { CustomerId = contactId, CustomerIdType = Models.Enums.CustomerIdType.Contact };
            }

            public static SimpleContactPaymentEntity Account(Guid accountId)
            {
                return new SimpleContactPaymentEntity { CustomerId = accountId, CustomerIdType = Models.Enums.CustomerIdType.Account };
            }
        }
    }
}