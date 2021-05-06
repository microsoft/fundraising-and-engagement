using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Data.DataSynchronization;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using FundraisingandEngagement.Services.Xrm;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement
{
    public class BankRunOperations
    {
        private readonly PaymentContext _context;
        private readonly IXrmService _xrmService;
        private readonly ILogger _logger;
        private readonly IDbEntitySynchronizer _dbEntitySynchronizer;

        public BankRunOperations(PaymentContext paymentContext, IXrmService xrmService, IDbEntitySynchronizer dbEntitySynchronizer, ILogger logger)
        {
            this._context = paymentContext;
            this._xrmService = xrmService;
            this._logger = logger;
            this._dbEntitySynchronizer = dbEntitySynchronizer;
        }

        public async Task ExecuteBankRunOperation(string selectedOperation, Guid? bankRunGuid)
        {
            this._logger.LogInformation("----------Entering ExecuteBankRunOperation()----------");

            try
            {
                this._dbEntitySynchronizer.SynchronizeEntitiesToDbTransitively(new List<Type>
                {
                    typeof(BankRun),
                    typeof(PaymentProcessor),
                    typeof(PaymentMethod),
                    typeof(BankRunSchedule),
                    typeof(PaymentSchedule)
                });

                if (selectedOperation.Equals("List"))
                {
                    await BankRunList(bankRunGuid);
                }
                else if (selectedOperation.Equals("File"))
                {
                    await BankRunFile(bankRunGuid);
                }
                else if (selectedOperation.Equals("GenerateTransactions"))
                {
                    BankRunGenerateTransactions(bankRunGuid);
                }
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "Error in ExecuteBankRunOperation(): " + e.Message);
                throw;
            }

            this._logger.LogInformation("----------Exiting ExecuteBankRunOperation()----------");
        }

        private async Task BankRunList(Guid? bankRunGuid)
        {
            BankRun bankRunEntity = BankRunFileReport.GetBankRunEntityFromId(bankRunGuid, this._context);

            // Assign the payment schedules to the bank run:
            BankRunGenerateList.GetPaymentSchedulesForBankRun(bankRunEntity, this._context, this._logger);
            // Update the Bankrun Status to "Report Available" (844060004):
            BankRun bankRunToUpdate = new BankRun { BankRunId = bankRunEntity.BankRunId, BankRunStatus = 844060004 };
            this._logger.LogInformation("Updating BankRun Status.");
            await this._xrmService.UpdateAsync(bankRunToUpdate);
            this._logger.LogInformation("Updated BankRun Status to \"Report Available\" successfully.");
        }

        private async Task BankRunFile(Guid? bankRunGuid)
        {
            BankRun bankRunEntity = BankRunFileReport.GetBankRunEntityFromId(bankRunGuid, this._context);
            PaymentProcessor paymentProcessorEntity = BankRunFileReport.GetPaymentProcessorEntityFromBankRun(bankRunEntity, this._context, this._logger);
            PaymentMethod paymentMethodEntity = BankRunFileReport.GetPaymentMethodEntityFromBankRun(bankRunEntity, this._context, this._logger);

            int? bankRunFileFormat = paymentProcessorEntity.BankRunFileFormat;
            this._logger.LogInformation("Requested Bank Run File Format:" + bankRunFileFormat);

            BankRunFileReport bankRunFileReport;
            switch (bankRunFileFormat)
            {
                case (int)BankRunFileFormat.ABA:
                    bankRunFileReport = new AbaFileReport(bankRunEntity, paymentProcessorEntity, paymentMethodEntity, this._context, this._logger);
                    break;
                case (int)BankRunFileFormat.BMO:
                    bankRunFileReport = new BMOFileReport(bankRunEntity, paymentProcessorEntity, paymentMethodEntity, this._context, this._logger);
                    break;
                case (int)BankRunFileFormat.ScotiaBank:
                    bankRunFileReport = new ScotiaBankFileReport(bankRunEntity, paymentProcessorEntity, paymentMethodEntity, this._context, this._logger);
                    break;
                case null:
                    throw new Exception("No Bank Run File Format set on the Payment Processor with ID:" + paymentProcessorEntity.PaymentProcessorId);
                default:
                    throw new Exception("Can't find Bank Run File Format for provided value:" + bankRunFileFormat);
            }

            await bankRunFileReport.GenerateFileReport();
            await bankRunFileReport.SaveReport();
        }

        private void BankRunGenerateTransactions(Guid? bankRunGuid)
        {
            BankRun bankRunEntity = BankRunFileReport.GetBankRunEntityFromId(bankRunGuid, this._context);
            BankRunRecurringDonations.GenerateBankRunRecurringDonations(bankRunEntity, this._context, this._logger);
        }
    }
}