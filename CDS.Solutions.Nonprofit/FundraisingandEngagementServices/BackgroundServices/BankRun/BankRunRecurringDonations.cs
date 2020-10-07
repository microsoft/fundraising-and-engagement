using System;
using System.Collections.Generic;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using FundraisingandEngagement.Utils;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement
{
	public class BankRunRecurringDonations
	{
		public static void GenerateBankRunRecurringDonations(BankRun bankRunEntity, PaymentContext dataContext, ILogger logger)
		{
			logger.LogInformation("----------Entering GenerateBankRunRecurringDonations()----------");

			// Get all of the payment schedules from the bank runs:
            List<BankRunSchedule> bankRunSchedules = dataContext.BankRunSchedule.Where(brs =>
                brs.BankRunId == bankRunEntity.BankRunId
                && brs.StatusCode == StatusCode.Active
                && (brs.Deleted == null || brs.Deleted == false)).ToList(); // Active and not deleted.
			
			if (bankRunSchedules.Count() < 1)
			{
				logger.LogInformation("No Bank Run Schedules found for this bank run ("+ bankRunEntity.BankRunId + "). Exiting webjob.");
				return;
			}

			logger.LogInformation("Bank Run Schedules meeting the criteria for this bank run (" + bankRunEntity.BankRunId + "): " + bankRunSchedules.Count());

			int numberOfFailedPaymentSchedules = 0;
			int currentPaymentScheduleNumber = 0; // Used for visual counting, not for code operations.
			List<Guid> paymentSchedulesProcessed = new List<Guid>();

			foreach (BankRunSchedule brs in bankRunSchedules)
			{
				try
				{
					bool generateTransaction = false;
					PaymentSchedule paymentSchedule = dataContext.PaymentSchedule.Where(ps => ps.PaymentScheduleId == brs.PaymentScheduleId).FirstOrDefault();
					BankRun paymentSchedulesAssociatedRun = dataContext.BankRun.Where(br => br.BankRunId == brs.BankRunId).FirstOrDefault();
					currentPaymentScheduleNumber++;

					// Make sure we don't process anything twice (just in case):
					if (!paymentSchedulesProcessed.Contains(paymentSchedule.PaymentScheduleId))
					{
						generateTransaction = true;
					}
					else
					{
						logger.LogInformation("Skipping potential duplicate: " + paymentSchedule.PaymentScheduleId);
					}

					if (generateTransaction)
					{
						logger.LogInformation("--PS-" + currentPaymentScheduleNumber.ToString() + "--");
						logger.LogInformation("Processing Payment Schedule = " + paymentSchedule.Name + "  (" + paymentSchedule.PaymentScheduleId.ToString() + ")");
						logger.LogInformation("Previous Next Donation Date = " + paymentSchedule.NextPaymentDate.ToString());
						// Set the last donation date:
						paymentSchedule.LastPaymentDate = paymentSchedulesAssociatedRun.DateToBeProcessed;
						// If so, we process it (create the child transaction, update next donation date) and move on to the next one:
						paymentSchedule.NextPaymentDate = paymentSchedule.GetNextDonationDate();
						logger.LogInformation("New Next Donation Date = " + paymentSchedule.NextPaymentDate.ToString());
						paymentSchedule.SyncDate = null;
						Transaction newChildTransaction = GenerateChildTransactionForPaymentSchedule(paymentSchedule, paymentSchedulesAssociatedRun);
						// Add to DB changes:
						logger.LogInformation("Created new transaction (" + newChildTransaction.TransactionId.ToString() + ") adding to transaction list.");
						dataContext.Transaction.Add(newChildTransaction);
						// Update the bank run schedule to status to "Completed":
						brs.StatusCode = StatusCode.Completed;
						brs.SyncDate = null;
						// Update the bank run:
						paymentSchedulesAssociatedRun.BankRunStatus = 844060001; // Processed
						paymentSchedulesAssociatedRun.SyncDate = null;
						paymentSchedulesProcessed.Add(paymentSchedule.PaymentScheduleId);
						logger.LogInformation("----");
					}
				}
				catch (Exception e)
				{
					numberOfFailedPaymentSchedules++;
					logger.LogError("Error with processing payment schedule (" + brs.PaymentScheduleId + "): " + e.Message);
					if (e.InnerException != null)
					{
						logger.LogError("Inner exception: " + e.InnerException.ToString());
					}
				}
			}

			logger.LogInformation("Processed all payment schedules (" + currentPaymentScheduleNumber + " total, " + numberOfFailedPaymentSchedules.ToString() + " failed payment schedules).");
			logger.LogInformation("Saving all payment schedules and new transactions to the DB.");

			dataContext.SaveChanges();
			logger.LogInformation("Save Complete.");

			logger.LogInformation("----------Exiting GenerateBankRunRecurringDonations()----------");
		}

		private static Transaction GenerateChildTransactionForPaymentSchedule(PaymentSchedule parentPaymentSchedule, BankRun paymentSchedulesAssociatedRun)
		{
			Transaction newChildTransaction = new Transaction();
			// Get all the same fields and copy them to the new child record:
			newChildTransaction = parentPaymentSchedule.CopyCommonFieldsTo(newChildTransaction);

			// Some fields are named different or have different states for the child vs parent, so we fix those here:
			newChildTransaction.TransactionId = Guid.NewGuid();
			newChildTransaction.StatusCode = StatusCode.Completed; // Completed
			newChildTransaction.TransactionPaymentScheduleId = parentPaymentSchedule.PaymentScheduleId;
			newChildTransaction.BookDate = DateTime.UtcNow;
			newChildTransaction.CreatedOn = DateTime.UtcNow;
			newChildTransaction.Amount = parentPaymentSchedule.RecurringAmount; // Total Header
			newChildTransaction.AmountMembership = parentPaymentSchedule.AmountMembership;
			newChildTransaction.AmountNonReceiptable = parentPaymentSchedule.AmountNonReceiptable;
			newChildTransaction.AmountReceipted = parentPaymentSchedule.AmountReceipted;
			newChildTransaction.TransactionPaymentMethodId = parentPaymentSchedule.PaymentMethodId;
			newChildTransaction.DepositDate = paymentSchedulesAssociatedRun.DateToBeProcessed;
			newChildTransaction.TransactionResult = "Bank Run ID: " + paymentSchedulesAssociatedRun.BankRunId.ToString();
			newChildTransaction.DesignationId = parentPaymentSchedule.DesignationId;
			newChildTransaction.AppealId = parentPaymentSchedule.AppealId;
			newChildTransaction.TransactionCurrencyId = parentPaymentSchedule.TransactionCurrencyId;
			newChildTransaction.MembershipInstanceId = parentPaymentSchedule.MembershipId;
			newChildTransaction.MembershipId = parentPaymentSchedule.MembershipCategoryId;
			newChildTransaction.SyncDate = null;
			newChildTransaction.ReceivedDate = paymentSchedulesAssociatedRun.DateToBeProcessed;

			return newChildTransaction;
		}
	}
}