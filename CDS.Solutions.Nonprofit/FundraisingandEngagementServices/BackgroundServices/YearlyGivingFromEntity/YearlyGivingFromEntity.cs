using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement
{
	public class YearlyGivingFromEntity
	{
		private readonly PaymentContext dataContext;
		private readonly ILogger logger;
		private YearlyGivingCalculator yearlyGivingCalculator;

		public YearlyGivingFromEntity(PaymentContext dataContext, ILogger logger)
		{
			this.logger = logger;
			this.dataContext = dataContext;
			this.yearlyGivingCalculator = new YearlyGivingCalculator(dataContext, logger);
		}

		public void CalculateFromPaymentEntity(Guid entityId, string entityName)
		{
			this.logger.LogInformation("Entering CalculateFromPaymentEntity with entityId " + entityId + " and entityName " + entityName);
			
			ContactPaymentEntity? contactPaymentEntity = null;
			if (String.Compare(entityName, "msnfp_transaction", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.logger.LogInformation("Looking for Transaction with Id:" + entityId);
				contactPaymentEntity = dataContext.Transaction.FirstOrDefault(x => x.TransactionId == entityId);
			}
			else if (String.Compare(entityName, "msnfp_eventpackage", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.logger.LogInformation("Looking for Event Package with Id:" + entityId);
				contactPaymentEntity = dataContext.EventPackage.FirstOrDefault(x => x.EventPackageId == entityId);
			}
			else if (String.Compare(entityName, "msnfp_donorcommitment", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.logger.LogInformation("Looking for Donor Commitment with Id:" + entityId);
				contactPaymentEntity = dataContext.DonorCommitment.FirstOrDefault(x => x.DonorCommitmentId == entityId);
			}

			if (contactPaymentEntity == null)
			{
				logger.LogError("No record found of type " + entityName + " with Id:" + entityId + ". Exiting.");
				return;
			}

			this.logger.LogInformation("Customer Id:" + contactPaymentEntity.CustomerId +", Type:" + contactPaymentEntity.CustomerIdType);
			yearlyGivingCalculator.UpdateCustomer(contactPaymentEntity.CustomerId, contactPaymentEntity.CustomerIdType);

			this.logger.LogInformation("CalculateFromPaymentEntity done.");
		}
	}
}
