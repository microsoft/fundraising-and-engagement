using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace FundraisingandEngagement.RecurringDonations.Services
{
	internal class PaymentRepoService : IPaymentRepoService
	{
		private readonly IPaymentContext context;

		public PaymentRepoService(IPaymentContext context)
		{
			this.context = context;
			this.context.DisableAutoDetectChanges();
		}

		public async Task<IReadOnlyList<SinglePaymentVariables>> GetFailedPaymentsAsync(CancellationToken token = default)
		{
			var today = DateTime.Today;
			var past15days = today.AddDays(-15);

			var query = from transaction in this.context.Transaction.Include(t => t.Response)
						where transaction.StatusCode == StatusCode.Failed
						where transaction.PaymentTypeCode == PaymentTypeCode.CreditCard
						where transaction.ConfigurationId != null
						where transaction.PaymentProcessor != null
						where transaction.TransactionCurrencyId != null
						where transaction.TransactionPaymentMethod != null
						where transaction.TransactionPaymentMethod.CcLast4 != null && transaction.TransactionPaymentMethod.CcLast4 != String.Empty
						where transaction.TransactionPaymentMethod.CcExpMmYy != null && transaction.TransactionPaymentMethod.CcExpMmYy != String.Empty
						where transaction.TransactionPaymentSchedule != null
						where transaction.TransactionPaymentSchedule.RecurringAmount > 0m
						where transaction.TransactionPaymentSchedule.Deleted == null || transaction.TransactionPaymentSchedule.Deleted == false
						where transaction.NextFailedRetry.HasValue && (transaction.NextFailedRetry <= today) && transaction.NextFailedRetry > past15days
						where transaction.CurrentRetry == null || transaction.CurrentRetry < transaction.Configuration.ScheMaxRetries
						select new SinglePaymentVariables
						{
							ConfigurationId = transaction.ConfigurationId!.Value,
							ScheMaxRetries = transaction.Configuration.ScheMaxRetries,
							ScheRetryinterval = transaction.Configuration.ScheRetryinterval,
							ShouldSyncResponse = transaction.Configuration.ShouldSyncResponse,
							PaymentMethodId = transaction.TransactionPaymentMethodId!.Value,
							AuthToken = transaction.TransactionPaymentMethod.AuthToken,
							StripeCustomerId = transaction.TransactionPaymentMethod.StripeCustomerId,
							PaymentProcessorId = transaction.PaymentProcessorId!.Value,
							PaymentGatewayType = transaction.PaymentProcessor.PaymentGatewayType,
							MonerisApiKey = transaction.PaymentProcessor.MonerisApiKey,
							MonerisStoreId = transaction.PaymentProcessor.MonerisStoreId,
							MonerisTestMode = transaction.PaymentProcessor.MonerisTestMode,
							IatsAgentCode = transaction.PaymentProcessor.IatsAgentCode,
							IatsPassword = transaction.PaymentProcessor.IatsPassword,
							WorldPayServiceKey = transaction.PaymentProcessor.WorldPayServiceKey,
							PaymentSchedule = transaction.TransactionPaymentSchedule,
							TransactionCurrencyId = transaction.TransactionCurrencyId!.Value,
							IsoCurrencyCode = transaction.TransactionCurrency.IsoCurrencyCode,
							Transaction = transaction,
						};

			return await query.AsNoTracking().ToListAsync(token);
		}

		public async Task<IReadOnlyList<SinglePaymentVariables>> GetScheduledPaymentsAsync(CancellationToken token = default)
		{
			var now = DateTime.Now.Date;

			var query = from p in this.context.PaymentSchedule
						where p.Deleted == null || p.Deleted == false
						where p.EndonDate == null || p.EndonDate >= now
						where p.NextPaymentDate != null && p.NextPaymentDate.Value.Date <= now.Date
						where p.FirstPaymentDate <= now
						where p.StatusCode == StatusCode.Active
						where p.RecurringAmount != null && p.RecurringAmount > 0m
						where p.PaymentTypeCode == PaymentTypeCode.CreditCard
						where p.PaymentMethod != null
						where p.PaymentMethod.CcLast4 != null && p.PaymentMethod.CcLast4 != String.Empty
						where p.PaymentMethod.CcExpMmYy != null && p.PaymentMethod.CcExpMmYy != String.Empty
						where p.PaymentProcessor != null
						where p.TransactionCurrency != null
						where p.FrequencyInterval != null
						where p.CustomerId != null
						where p.ConfigurationId != null
						where p.TransactionCurrencyId != null
						select new SinglePaymentVariables
						{
							ConfigurationId = p.ConfigurationId!.Value,
							ScheMaxRetries = p.Configuration.ScheMaxRetries,
							ScheRetryinterval = p.Configuration.ScheRetryinterval,
							ShouldSyncResponse = p.Configuration.ShouldSyncResponse,
							PaymentMethodId = p.PaymentMethodId!.Value,
							AuthToken = p.PaymentMethod.AuthToken,
							StripeCustomerId = p.PaymentMethod.StripeCustomerId,
							PaymentProcessorId = p.PaymentProcessorId!.Value,
							PaymentGatewayType = p.PaymentProcessor.PaymentGatewayType,
							MonerisApiKey = p.PaymentProcessor.MonerisApiKey,
							MonerisStoreId = p.PaymentProcessor.MonerisStoreId,
							MonerisTestMode = p.PaymentProcessor.MonerisTestMode,
							IatsAgentCode = p.PaymentProcessor.IatsAgentCode,
							IatsPassword = p.PaymentProcessor.IatsPassword,
							WorldPayServiceKey = p.PaymentProcessor.WorldPayServiceKey,
							PaymentSchedule = p,
							TransactionCurrencyId = p.TransactionCurrencyId!.Value,
							IsoCurrencyCode = p.TransactionCurrency.IsoCurrencyCode,
						};

			return await query.AsNoTracking().ToListAsync(token);
		}

		public async Task SaveChangesAsync(CancellationToken token, Response response)
		{
			if (response.Transaction.TransactionId == Guid.Empty)
			{
				this.context.EntryAdd(response.Transaction);
			}
			else
			{
				this.context.EntryPropertyModify(response.Transaction, t => t.NextFailedRetry);
				this.context.EntryPropertyModify(response.Transaction, t => t.LastFailedRetry);
				this.context.EntryPropertyModify(response.Transaction, t => t.CurrentRetry);
				this.context.EntryPropertyModify(response.Transaction, t => t.InvoiceIdentifier);
				this.context.EntryPropertyModify(response.Transaction, t => t.TransactionIdentifier);
				this.context.EntryPropertyModify(response.Transaction, t => t.TransactionResult);
				this.context.EntryPropertyModify(response.Transaction, t => t.TransactionNumber);
				this.context.EntryPropertyModify(response.Transaction, t => t.TransactionFraudCode);
				this.context.EntryPropertyModify(response.Transaction, t => t.StatusCode);
				this.context.EntryPropertyModify(response.Transaction, t => t.SyncDate);
			}

			this.context.EntryPropertyModify(response.PaymentSchedule, ps => ps.LastPaymentDate);
			this.context.EntryPropertyModify(response.PaymentSchedule, ps => ps.NextPaymentDate);
			this.context.EntryPropertyModify(response.PaymentSchedule, ps => ps.TotalAmount);
			this.context.EntryPropertyModify(response.PaymentSchedule, ps => ps.NumberOfSuccesses);
			this.context.EntryPropertyModify(response.PaymentSchedule, ps => ps.ConcurrentFailures);
			this.context.EntryPropertyModify(response.PaymentSchedule, ps => ps.NumberOfFailures);
			this.context.EntryPropertyModify(response.PaymentSchedule, ps => ps.AmountOfFailures);
			this.context.EntryPropertyModify(response.PaymentSchedule, ps => ps.SyncDate);

			this.context.EntryAdd(response);

			await this.context.SaveChangesAsync(token);
		}
	}
}
