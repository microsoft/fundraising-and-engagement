// the code has a lot of warning messages, possble possible issues
#pragma warning disable CS8629 // Nullable value type may be null.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

using System;
using System.Linq;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement
{
	public class BankRunGenerateList
	{
		// Bank Run Process Notes:
		/*
		 * 1. User clicks the generate list on the bank run. This uses the query to get the records from SQL DB.
		 * 2. With these records we attempt to create Bank Run Schedule records in SQL DB.
		 * 3. Those records are synced back to Dynamics via the data sync process.
		 * 4. Those records are used for the Report generation (taken from SQL DB).
		 * 5. After the report is generated the user confirms the list changing the bank run status to List Confirmed.
		 *		- This generates the child transactions (links them to the bank run) and updates all the payment schedules.
		 */
		public static void GetPaymentSchedulesForBankRun(BankRun bankRunEntity, PaymentContext dataContext, ILogger logger)
		{
			logger.LogInformation("----------Entering GetPaymentSchedulesForBankRun()----------");
			try
			{
				var paymentSchedules = dataContext.PaymentSchedule.Where(ps => (ps.Deleted == null || ps.Deleted == false)
								&& ps.StatusCode == StatusCode.Active // Active
								&& ps.PaymentMethodId != null // Has a payment method
								&& ps.PaymentTypeCode == PaymentTypeCode.BankAccount
								&& (ps.NextPaymentDate.Value.Date <= bankRunEntity.EndDate.Value.Date && ps.NextPaymentDate.Value.Date >= bankRunEntity.StartDate.Value.Date) // If it is within the start-end range
								).ToList();
				
				//Local debugging: var paymentSchedules = DataContext.PaymentSchedule.Where(ps => ps.PaymentScheduleId == new Guid("3e5fec3c-f96d-ea11-a811-000d3a0c8a65"));

				logger.LogInformation("Payment Schedules to associate = " + paymentSchedules.Count());
				logger.LogInformation("Bank Run Start Date = " + bankRunEntity.StartDate.Value.Date);
				logger.LogInformation("Bank Run End Date = " + bankRunEntity.EndDate.Value.Date);

				// Now that we have the payment schedules, attach them to this bank run:
				foreach (PaymentSchedule ps in paymentSchedules)
				{
					logger.LogInformation("Adding Payment Schedule: " + ps.Name + " (" + ps.PaymentScheduleId + ")");

					// Create the bank run schedule in SQL DB:
					try
					{
						bool duplicate = false;
						if (dataContext.BankRunSchedule.Count() > 0)
						{
							// We make sure to not add a duplicate for the same bank run:
							if (dataContext.BankRunSchedule.Where(brs => brs.PaymentScheduleId == ps.PaymentScheduleId && brs.BankRunId == bankRunEntity.BankRunId && (brs.Deleted == null || brs.Deleted == false)).Count() > 0)
							{
								// Duplicate, ignore.
								duplicate = true;
							}
						}

						if (!duplicate)
						{
							BankRunSchedule newBRS = new BankRunSchedule();
							newBRS.BankRunId = bankRunEntity.BankRunId;
							newBRS.PaymentScheduleId = ps.PaymentScheduleId;
							newBRS.CreatedOn = DateTime.UtcNow;
							newBRS.StateCode = 0; // Active state 
							newBRS.StatusCode = StatusCode.Active; // Active status reason code (default status reason)
							dataContext.BankRunSchedule.Add(newBRS);
						}
					}
					catch (Exception e)
					{
						// It may already exist, so ignore and move on.
						logger.LogError("Issue with adding new bank run schedule record for payment schedule " + ps.Name + " (" + ps.PaymentScheduleId + "): " + e.Message);
					}
				}

				try
				{
					if (bankRunEntity.BankRunStatus == 844060000 || bankRunEntity.BankRunStatus == 844060004)
					{
						logger.LogInformation("Setting Bank Run Status to 'Gift List Retrieved'.");
						bankRunEntity.BankRunStatus = 844060003; // Gift List Retrieved
						bankRunEntity.SyncDate = null; // Sync this change back to Dynamics.
					}

					logger.LogInformation("Saving new Bank Run Schedules to DB.");
					dataContext.SaveChanges();
					logger.LogInformation("Save Complete.");
				}
				catch (Exception e)
				{
					// It may already exist, so ignore and move on.
					logger.LogError("Issue saving Bank Run Schedules: " + e.Message);
					if (e.InnerException != null)
					{
						logger.LogError("Inner exception: " + e.InnerException.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error has occured in GetPaymentSchedulesForBankRun(): " + ex.Message);
				if (ex.InnerException != null)
				{
					Console.WriteLine("Inner exception: " + ex.InnerException.ToString());
				}
				throw;
			}
			Console.WriteLine("----------Exiting GetPaymentSchedulesForBankRun()----------");
		}
	}
}
