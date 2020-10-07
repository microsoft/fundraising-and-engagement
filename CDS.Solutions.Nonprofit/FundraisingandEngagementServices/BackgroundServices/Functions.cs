using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using FundraisingandEngagement.Services.Xrm;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement
{
	public class Functions
	{
		private readonly PaymentContext context;
		private readonly IXrmService xrmService;
		private readonly ILogger logger;

		public Functions(PaymentContext paymentContext, IXrmService xrmService, ILogger logger)
		{
			this.context = paymentContext; // Select/Update from SQL DB
			this.xrmService = xrmService;
			this.logger = logger;
		}

		public async Task BankRunAppSelector(string selectedProcess, Guid? bankRunGUID, Guid? entityId, string entityName)
		{
			logger.LogInformation("----------Entering BankRunAppSelector()----------");


			try
			{

				#region Bank Run Generate List
				// ----------Bank Run Generate List----------
				if (selectedProcess.Equals("List"))
				{
					BankRun bankRunEntity = BankRunFileReport.GetBankRunEntityFromId(bankRunGUID, this.context);

					if (bankRunEntity == null)
					{
						logger.LogInformation("Exiting web job.");
						return;
					}

					// Assign the payment schedules to the bank run:
					BankRunGenerateList.GetPaymentSchedulesForBankRun(bankRunEntity, context, this.logger);
					// Now do the same for one off transactions (TODO in future sprint).

					try
					{
						// Update the Bankrun Status to "Report Available" (844060004):
						BankRun bankRunToUpdate = new BankRun();
                        bankRunToUpdate.BankRunId = bankRunEntity.BankRunId;
                        bankRunToUpdate.BankRunStatus = 844060004;
                        logger.LogInformation("Updating BankRun Status.");
                        await this.xrmService.UpdateAsync(bankRunToUpdate);
                        logger.LogInformation("Updated BankRun Status to \"Report Available\" successfully.");
					}
					catch (Exception ex)
					{
						logger.LogError("Could not Update Bank Run Status. Exception:" + ex.Message);
					}
				}
				#endregion
				#region Bank Run Generate File
				// ----------Bank Run Generate File----------
				else if (selectedProcess.Equals("File"))
				{
					BankRun bankRunEntity = BankRunFileReport.GetBankRunEntityFromId(bankRunGUID, this.context);
					//Configuration configEntity = Common.GetConfigurationEntityFromId(configGuid, this.DataContext);
					PaymentProcessor paymentProcessorEntity = BankRunFileReport.GetPaymentProcessorEntityFromBankRun(bankRunEntity, this.context, this.logger);
					PaymentMethod paymentMethodEntity = BankRunFileReport.GetPaymentMethodEntityFromBankRun(bankRunEntity, this.context, this.logger);

					int? bankRunFileFormat = paymentProcessorEntity.BankRunFileFormat;
					logger.LogInformation("Requested Bank Run File Format:" + bankRunFileFormat);

					BankRunFileReport bankRunFileReport;
					switch (bankRunFileFormat)
					{
						case (int)BankRunFileFormat.ABA:
							bankRunFileReport = new AbaFileReport(bankRunEntity, paymentProcessorEntity, paymentMethodEntity, this.context, this.logger);
							break;
						case (int)BankRunFileFormat.BMO:
							bankRunFileReport = new BMOFileReport(bankRunEntity, paymentProcessorEntity, paymentMethodEntity, this.context, this.logger);
							break;
						case (int)BankRunFileFormat.ScotiaBank:
							bankRunFileReport = new ScotiaBankFileReport(bankRunEntity, paymentProcessorEntity, paymentMethodEntity, this.context, this.logger);
							break;
						case null:
							throw new Exception("No Bank Run File Format set on the Payment Processor with ID:" + paymentProcessorEntity.PaymentProcessorId);
						default:
							throw new Exception("Can't find Bank Run File Format for provided value:" + bankRunFileFormat);
					}

					await bankRunFileReport.GenerateFileReport();
					await bankRunFileReport.SaveReport();
				}
				#endregion
				#region Bank Run Generate Recurring Donation Records
				else if (selectedProcess.Equals("GenerateTransactions"))
				{
					BankRun bankRunEntity = BankRunFileReport.GetBankRunEntityFromId(bankRunGUID, this.context);
					BankRunRecurringDonations.GenerateBankRunRecurringDonations(bankRunEntity, this.context, this.logger);
				}
				#endregion

				#region Event Receipting
				else if (selectedProcess.Equals("EventReceipting") && entityId.HasValue)
				{
					List<EventPackage> eventPackages = new List<EventPackage>();
					switch (entityName)
					{
						case EventReceipting.EventTicket:
							EventTicket eventTicket = EventReceipting.GetEventTicketFromId(entityId.Value, this.context);
							EventReceipting.UpdateTicketsFromEventTicket(eventTicket, this.context);
							eventPackages = EventReceipting.GetEventPackagesFromEventTicket(eventTicket, this.context);
							break;

						case EventReceipting.EventProduct:
							EventProduct eventProduct = EventReceipting.GetEventProductFromId(entityId.Value, this.context);
							EventReceipting.UpdateProductsFromEventProduct(eventProduct, this.context);
							eventPackages = EventReceipting.GetEventPackagesFromEventProduct(eventProduct, this.context);
							break;

						case EventReceipting.EventSponsorship:
							EventSponsorship eventSponsorship = EventReceipting.GetEventSponsorshipFromId(entityId.Value, this.context);
							EventReceipting.UpdateSponsorshipsFromEventSponsorship(eventSponsorship, this.context);
							eventPackages = EventReceipting.GetEventPackagesFromEventSponsorship(eventSponsorship, this.context);
							break;

						default:
							throw new Exception("Unknown Entity for Event Receipting: " + entityName + ". Exiting.");
					}

					EventReceipting.UpdateEventPackages(eventPackages, this.context);
				}

				#endregion
			}
			catch (Exception e)
			{
				logger.LogError("Error in BankRunAppSelector(): " + e.Message);
				if (e.InnerException != null)
				{
					logger.LogError("Inner exception: " + e.InnerException.ToString());
				}

			}

			logger.LogInformation("----------Exiting BankRunAppSelector()----------");
			logger.LogInformation("----------Exiting Web Job----------");
		}
	}
}