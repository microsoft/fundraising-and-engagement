using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FundraisingandEngagement.Models.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentProcessors;
using PaymentProcessors.Models;

namespace FundraisingandEngagement.RecurringDonations.Services
{
	public class RecurringDonationBackgroundService : BackgroundService
	{
		private readonly IPaymentRepoService repoService;
		private readonly IPaymentProcessorGateway paymentProcessorGateway;
		private readonly IHostApplicationLifetime hostLifetime;
		private readonly ILogger logger;

		public RecurringDonationBackgroundService(
				IPaymentRepoService repoService,
				IPaymentProcessorGateway paymentProcessorGateway,
				IHostApplicationLifetime hostLifetime, ILogger logger)
		{
			this.repoService = repoService;
			this.paymentProcessorGateway = paymentProcessorGateway;
			this.hostLifetime = hostLifetime;
			this.logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			this.logger.LogInformation("Running RecurringDonation");

			var stopWatch = new Stopwatch();

			try
			{
				stopWatch.Start();
				var count = await ProcessFailedPaymentsAsync(stoppingToken);
				stopWatch.Stop();

				this.logger.LogInformation($"Processed {count} failed transaction in {stopWatch.Elapsed}");
			}
			catch (Exception e)
			{
				this.logger.LogCritical(e, "Critical error in processing failed transactions");
			}

			try
			{
				stopWatch.Restart();
				var count = await ProcessScheduledPaymentsAsync(stoppingToken);
				stopWatch.Stop();

				this.logger.LogInformation($"Processed {count} payments in {stopWatch.Elapsed}");
			}
			catch (Exception e)
			{
				this.logger.LogCritical(e, "Critical error in processing scheduled payments");
			}

			this.hostLifetime.StopApplication();
		}


		private async Task<int> ProcessFailedPaymentsAsync(CancellationToken token)
		{
			var paymentCount = 0;
			var failedPayments = await this.repoService.GetFailedPaymentsAsync(token);

			this.logger.LogInformation($"There are {failedPayments.Count} failed payments to process");

			foreach (var failedPayment in failedPayments)
			{
				if (token.IsCancellationRequested)
				{
					this.logger.LogError("Operation cancelled");
					break;
				}

				RecurringPaymentOutput response;

				try
				{
					var input = failedPayment.CreateRecurringPaymentInput();
					response = await this.paymentProcessorGateway.MakePreAuthorizedPaymentAsync(input, token);
				}
				catch (Exception e)
				{
					this.logger.LogError(e, "Failed to make retry payment.");

					response = failedPayment.CreateErrorOutput(e.Message);
				}

				failedPayment.UpdateExistingTransaction(response);

				if (response.IsSuccessful)
				{
					this.logger.LogInformation($"Retried Payment Successful, TransactionId : {failedPayment.Transaction!.TransactionId}");

					paymentCount++;
				}
				else
				{
					this.logger.LogWarning($"Retried Payment Failed, TransactionId : {failedPayment.Transaction!.TransactionId} with message '{response.TransactionResult ?? response.TransactionStatus}'");

					var nextRetryDays = failedPayment.ScheRetryinterval ?? 1;

					failedPayment.Transaction!.NextFailedRetry = DateTime.Now.AddDays(nextRetryDays);
					failedPayment.Transaction!.LastFailedRetry = DateTime.Now;

					if (failedPayment.Transaction.CurrentRetry == null)
						failedPayment.Transaction.CurrentRetry = 0;
					failedPayment.Transaction.CurrentRetry++;
				}

				await this.repoService.SaveChangesAsync(token, failedPayment.Response);
			}

			this.logger.LogDebug($"Exiting {nameof(ProcessFailedPaymentsAsync)}");

			return paymentCount;
		}

		private async Task<int> ProcessScheduledPaymentsAsync(CancellationToken token)
		{
			var paymentCount = 0;
			var scheduledPayments = await this.repoService.GetScheduledPaymentsAsync(token);

			this.logger.LogInformation($"There are {scheduledPayments.Count} scheduled payments to process");

			foreach (var scheduledPayment in scheduledPayments)
			{
				if (token.IsCancellationRequested)
				{
					this.logger.LogError("Operation cancelled");
					break;
				}

				RecurringPaymentOutput response;

				try
				{
					var input = scheduledPayment.CreateRecurringPaymentInput();
					response = await this.paymentProcessorGateway.MakePreAuthorizedPaymentAsync(input, token);
				}
				catch (Exception e)
				{
					this.logger.LogError(e, "Failed to make payment.");

					response = scheduledPayment.CreateErrorOutput(e.Message);
				}

				scheduledPayment.Transaction = scheduledPayment.CreateNewTransaction(response);

				scheduledPayment.PaymentSchedule.LastPaymentDate = DateTime.Now;
				scheduledPayment.PaymentSchedule.NextPaymentDate = scheduledPayment.PaymentSchedule.GetNextDonationDate();

				if (response.IsSuccessful)
				{
					this.logger.LogInformation($"Payment Successful, PaymentScheduleId : {scheduledPayment.PaymentSchedule.PaymentScheduleId}");

					if (scheduledPayment.PaymentSchedule.TotalAmount == null)
						scheduledPayment.PaymentSchedule.TotalAmount = 0m;
					if (scheduledPayment.PaymentSchedule.NumberOfSuccesses == null)
						scheduledPayment.PaymentSchedule.NumberOfSuccesses = 0;

					scheduledPayment.PaymentSchedule.TotalAmount += (scheduledPayment.PaymentSchedule.RecurringAmount ?? 0m);
					scheduledPayment.PaymentSchedule.NumberOfSuccesses++;
					scheduledPayment.PaymentSchedule.ConcurrentFailures = 0;
					scheduledPayment.PaymentSchedule.SyncDate = null;
					paymentCount++;
				}
				else
				{
					this.logger.LogWarning($"Payment Failed, PaymentScheduleId : {scheduledPayment.PaymentSchedule.PaymentScheduleId} with message '{response.TransactionResult ?? response.TransactionStatus}'");

					if (scheduledPayment.PaymentSchedule.NumberOfFailures == null)
						scheduledPayment.PaymentSchedule.NumberOfFailures = 0;
					if (scheduledPayment.PaymentSchedule.ConcurrentFailures == null)
						scheduledPayment.PaymentSchedule.ConcurrentFailures = 0;
					if (scheduledPayment.PaymentSchedule.AmountOfFailures == null)
						scheduledPayment.PaymentSchedule.AmountOfFailures = 0m;

					scheduledPayment.PaymentSchedule.NumberOfFailures++;
					scheduledPayment.PaymentSchedule.ConcurrentFailures++;
					scheduledPayment.PaymentSchedule.AmountOfFailures += (scheduledPayment.PaymentSchedule.RecurringAmount ?? 0m);
					scheduledPayment.PaymentSchedule.SyncDate = null;
				}

				await this.repoService.SaveChangesAsync(token, scheduledPayment.Response);
			}

			this.logger.LogDebug($"Exiting {nameof(ProcessScheduledPaymentsAsync)}");

			return paymentCount;
		}
	}
}
