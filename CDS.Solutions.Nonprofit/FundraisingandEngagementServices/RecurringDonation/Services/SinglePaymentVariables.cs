using System;
using System.Collections.ObjectModel;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using PaymentProcessors.Models;

namespace FundraisingandEngagement.RecurringDonations.Services
{
	public class SinglePaymentVariables
	{
		public PaymentSchedule PaymentSchedule { get; set; } = null!;

		public Transaction? Transaction { get; set; }

		public Response Response { get; private set; } = null!;

		public Guid PaymentProcessorId { get; set; }

		public Guid PaymentMethodId { get; set; }

		public Guid ConfigurationId { get; set; }

		public Guid TransactionCurrencyId { get; internal set; }

		// Configuration

		public int? ScheRetryinterval { get; set; }

		public int? ScheMaxRetries { get; set; }

		public bool ShouldSyncResponse { get; set; }

		// TransactionCurrency

		public string IsoCurrencyCode { get; internal set; } = null!;

		// PaymentMethod

		public string AuthToken { get; internal set; } = null!;

		public string StripeCustomerId { get; internal set; } = null!;

		// PaymentProcessor

		public string MonerisStoreId { get; internal set; } = null!;

		public string MonerisApiKey { get; internal set; } = null!;

		public bool? MonerisTestMode { get; internal set; }

		public PaymentGatewayCode PaymentGatewayType { get; internal set; }

		public string WorldPayServiceKey { get; internal set; } = null!;

		public string IatsAgentCode { get; internal set; } = null!;

		public string IatsPassword { get; internal set; } = null!;


		internal RecurringPaymentInput CreateRecurringPaymentInput()
		{
			switch (PaymentGatewayType)
			{
				case PaymentGatewayCode.Moneris:
					return new MonerisRecurringPaymentInput
					{
						InvoiceIdentifier = PaymentSchedule.InvoiceIdentifier,

						CustID = AuthToken,
						StoreId = MonerisStoreId,
						ServiceKey = MonerisApiKey,
						IsTestMode = MonerisTestMode ?? false,
						MonerisAuthToken = AuthToken,

						RetryInterval = ScheRetryinterval ?? 0,
						MaxRetries = ScheMaxRetries ?? 0,
						CurrentRetry = Transaction?.CurrentRetry ?? 0,
						NextFailedRetry = Transaction?.NextFailedRetry,

						IsBankAccountPayment = PaymentSchedule.PaymentTypeCode == PaymentTypeCode.BankAccount,
						IsoCurrencyCode = IsoCurrencyCode,
						Amount = PaymentSchedule.RecurringAmount ?? 0m,
					};
				case PaymentGatewayCode.Stripe:
					return new StripeRecurringPaymentInput
					{
						InvoiceIdentifier = PaymentSchedule.InvoiceIdentifier,

						ServiceKey = MonerisApiKey,
						StripeCustomerId = StripeCustomerId,
						StripeCardId = AuthToken,

						RetryInterval = ScheRetryinterval ?? 0,
						MaxRetries = ScheMaxRetries ?? 0,
						CurrentRetry = Transaction?.CurrentRetry ?? 0,
						NextFailedRetry = Transaction?.NextFailedRetry,

						IsBankAccountPayment = PaymentSchedule.PaymentTypeCode == PaymentTypeCode.BankAccount,
						IsoCurrencyCode = IsoCurrencyCode,
						Amount = PaymentSchedule.RecurringAmount ?? 0m,
					};
				case PaymentGatewayCode.Iats:
					return new IatsRecurringPaymentInput
					{
						InvoiceIdentifier = PaymentSchedule.InvoiceIdentifier,

						AgentCode = IatsAgentCode,
						Password = IatsPassword,
						IatsCustomerCode = AuthToken,

						RetryInterval = ScheRetryinterval ?? 0,
						MaxRetries = ScheMaxRetries ?? 0,
						CurrentRetry = Transaction?.CurrentRetry ?? 0,
						NextFailedRetry = Transaction?.NextFailedRetry,

						IsBankAccountPayment = PaymentSchedule.PaymentTypeCode == PaymentTypeCode.BankAccount,
						IsoCurrencyCode = IsoCurrencyCode,
						Amount = PaymentSchedule.RecurringAmount ?? 0m
					};
				case PaymentGatewayCode.WorldPay:
					return new WorldPayRecurringPaymentInput
					{
						InvoiceIdentifier = PaymentSchedule.InvoiceIdentifier,

						FirstName = PaymentSchedule.FirstName,
						LastName = PaymentSchedule.LastName,
						Address1 = PaymentSchedule.BillingLine1,
						Address2 = PaymentSchedule.BillingLine2,
						City = PaymentSchedule.BillingCity,
						PostalCode = PaymentSchedule.BillingPostalCode,

						ServiceKey = WorldPayServiceKey,
						WorldPayToken = AuthToken,

						RetryInterval = ScheRetryinterval ?? 0,
						MaxRetries = ScheMaxRetries ?? 0,
						CurrentRetry = Transaction?.CurrentRetry ?? 0,
						NextFailedRetry = Transaction?.NextFailedRetry,

						IsBankAccountPayment = PaymentSchedule.PaymentTypeCode == PaymentTypeCode.BankAccount,
						IsoCurrencyCode = IsoCurrencyCode,
						Amount = PaymentSchedule.RecurringAmount ?? 0m,
					};
				default:
					throw new NotSupportedException($"Type {PaymentGatewayType} is not supported");
			}
		}


		internal Transaction CreateNewTransaction(RecurringPaymentOutput paymentResponse)
		{
			var transaction = new Transaction
			{
				Amount = PaymentSchedule.RecurringAmount,
				TransactionCurrencyId = TransactionCurrencyId,
				TransactionPaymentSchedule = PaymentSchedule,
				TransactionPaymentMethodId = PaymentMethodId,
				PaymentProcessorId = PaymentProcessorId,
				ConfigurationId = ConfigurationId,
				DesignationId = PaymentSchedule.DesignationId,
				MembershipId = PaymentSchedule.MembershipCategoryId,
				MembershipInstanceId = PaymentSchedule.MembershipId,
				Event = null,
				StateCode = 0,
			};

			transaction.AppealId = PaymentSchedule.AppealId;
			transaction.OriginatingCampaignId = PaymentSchedule.OriginatingCampaignId;
			transaction.ConstituentId = PaymentSchedule.ConstituentId;
			transaction.CustomerId = PaymentSchedule.CustomerId;
			transaction.CustomerIdType = PaymentSchedule.CustomerIdType;
			transaction.GiftBatchId = PaymentSchedule.GiftBatchId;
			transaction.PackageId = PaymentSchedule.PackageId;
			transaction.TaxReceiptId = PaymentSchedule.TaxReceiptId;
			transaction.TributeId = PaymentSchedule.TributeId;
			transaction.TransactionBatchId = PaymentSchedule.TransactionBatchId;
			transaction.AmountReceipted = PaymentSchedule.AmountReceipted;
			transaction.AmountMembership = PaymentSchedule.AmountMembership;
			transaction.AmountNonReceiptable = PaymentSchedule.AmountNonReceiptable;
			transaction.AmountTax = PaymentSchedule.AmountTax;
			transaction.Anonymous = PaymentSchedule.Anonymity;
			transaction.Appraiser = PaymentSchedule.Appraiser;
			transaction.BillingCity = PaymentSchedule.BillingCity;
			transaction.BillingCountry = PaymentSchedule.BillingCountry;
			transaction.BillingLine1 = PaymentSchedule.BillingLine1;
			transaction.BillingLine2 = PaymentSchedule.BillingLine2;
			transaction.BillingLine3 = PaymentSchedule.BillingLine3;
			transaction.BillingPostalCode = PaymentSchedule.BillingPostalCode;
			transaction.BillingStateorProvince = PaymentSchedule.BillingStateorProvince;
			transaction.CcBrandCode = PaymentSchedule.CcBrandCode;
			transaction.ChargeonCreate = PaymentSchedule.ChargeonCreate;
			transaction.BookDate = PaymentSchedule.BookDate;
			transaction.DepositDate = PaymentSchedule.DepositDate;
			transaction.GaDeliveryCode = PaymentSchedule.GaDeliveryCode;
			transaction.ReceiptPreferenceCode = PaymentSchedule.ReceiptPreferenceCode;
			transaction.DataEntrySource = PaymentSchedule.DataEntrySource;
			transaction.DataEntryReference = PaymentSchedule.DataEntryReference;
			transaction.FirstName = PaymentSchedule.FirstName;
			transaction.LastName = PaymentSchedule.LastName;
			transaction.Name = PaymentSchedule.Name;
			transaction.OrganizationName = PaymentSchedule.OrganizationName;
			transaction.Telephone1 = PaymentSchedule.Telephone1;
			transaction.Telephone2 = PaymentSchedule.Telephone2;
			transaction.MobilePhone = PaymentSchedule.MobilePhone;
			transaction.Emailaddress1 = PaymentSchedule.EmailAddress1;
			transaction.InvoiceIdentifier = PaymentSchedule.InvoiceIdentifier;
			transaction.PaymentTypeCode = PaymentSchedule.PaymentTypeCode;
			transaction.TransactionDescription = PaymentSchedule.TransactionDescription;
			transaction.TransactionFraudCode = PaymentSchedule.TransactionFraudCode;
			transaction.TransactionIdentifier = PaymentSchedule.TransactionIdentifier;
			transaction.TransactionResult = PaymentSchedule.TransactionResult;
			transaction.TributeCode = PaymentSchedule.TributeCode;
			transaction.TributeAcknowledgement = PaymentSchedule.TributeAcknowledgement;
			transaction.TributeMessage = PaymentSchedule.TributeMessage;
			transaction.Response = new Collection<Response>();

			UpdateTransactionFromPaymentResponse(transaction, paymentResponse);

			var now = DateTime.Now;

			transaction.DepositDate = now;
			transaction.BookDate = now;
			transaction.CreatedOn = now;
			transaction.SyncDate = null;

			return transaction;
		}

		internal void UpdateExistingTransaction(RecurringPaymentOutput response)
		{
			if (Transaction != null && response != null)
			{
				UpdateTransactionFromPaymentResponse(Transaction, response);
				Transaction.SyncDate = null;
			}
		}

		private void UpdateTransactionFromPaymentResponse(Transaction transaction, RecurringPaymentOutput paymentResponse)
		{
			transaction.LastFailedRetry = paymentResponse.LastFailedRetry;
			transaction.NextFailedRetry = paymentResponse.NextFailedRetry;
			transaction.CurrentRetry = paymentResponse.CurrentRetry;
			transaction.InvoiceIdentifier = paymentResponse.InvoiceIdentifier;
			transaction.TransactionIdentifier = paymentResponse.TransactionIdentifier;
			transaction.TransactionResult = paymentResponse.TransactionResult;
			transaction.TransactionNumber = paymentResponse.TransactionNumber;
			transaction.TransactionFraudCode = paymentResponse.TransactionFraudCode;
			transaction.StatusCode = paymentResponse.IsSuccessful ? StatusCode.Completed : StatusCode.Failed;

			Response = new Response
			{
				CreatedOn = DateTime.Now,
				Result = paymentResponse.TransactionResult,
				Identifier = paymentResponse.TransactionIdentifier,
				PaymentSchedule = PaymentSchedule,
				Transaction = transaction,
				StateCode = 0,
				StatusCode = StatusCode.Active,
				SyncDate = ShouldSyncResponse ? default(DateTime?) : DateTime.Now
			};
		}

		internal RecurringPaymentErrorOutput CreateErrorOutput(string errorMessage)
		{
			return new RecurringPaymentErrorOutput(Transaction?.CurrentRetry ?? 0, Transaction?.NextFailedRetry, PaymentSchedule.InvoiceIdentifier)
			{
				TransactionResult = errorMessage,
			};
		}
	}
}
