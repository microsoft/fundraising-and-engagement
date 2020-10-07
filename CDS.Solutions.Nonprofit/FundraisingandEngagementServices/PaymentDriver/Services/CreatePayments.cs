using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentDriver.Models;
using PaymentDriver.Utils;
using PaymentProcessors;
using PaymentProcessors.Models;

namespace PaymentDriver.Services
{
	internal class CreatePayments : ICreatePayment
	{
		private readonly IPaymentContext db;
		private readonly ILogger<CreatePayments> logger;
		private readonly IPaymentProcessorGateway gateway;
		private readonly TestDataOption option;

		public CreatePayments(IPaymentProcessorGateway gateway, IPaymentContext db, ILogger<CreatePayments> logger, IOptions<TestDataOption> options)
		{
			this.logger = logger;
			this.gateway = gateway;
			this.db = db;
			this.option = options.Value;
		}

		public async Task Run()
		{
			try
			{
				//load data into the database
				//get that data and send for payment
				var random = new Random();
				var stopWatch = new Stopwatch();
				stopWatch.Start();

				await this.db.Configuration.AnyAsync();

				var paymentList = GetPaymentData(this.option.MaxRecords, this.option.PaymentProcessorCode);

				var singleProcessTimer = new Stopwatch();

				foreach (var request in paymentList)
				{
					try
					{
						request.Output = await this.gateway.MakePaymentAsync(request.Input);

						var method = CreatePaymentMethod(request);
						var schedule = CreatePaymentSchedule(request);

						schedule.PaymentMethod = method;
						this.db.Add(method);
						this.db.Add(schedule);
					}
					catch (Exception ex)
					{
						this.logger.LogError(ex, $"Failed to make a {typeof(PaymentInput)} payment.");
					}
				}

				stopWatch.Stop();

				this.logger.LogInformation($"Time taken to create entities: {stopWatch.Elapsed}");

				stopWatch.Restart();

				await this.db.SaveChangesAsync(TimeSpan.FromHours(6));

				this.logger.LogInformation($"Time taken to save: {stopWatch.Elapsed}");

			}
			catch (Exception ex)
			{
				this.logger.LogError(ex, "Unhandled exception occured in payment driver");
			}
		}

		private IEnumerable<PaymentInfo> GetPaymentData(int max, int paymentProcessorCode)
		{
			//empty the database
			//seed initial data
			//create paymentrequests out of seeded data
			var currency = DefaultData.TransactionCurrency;
			var processor = paymentProcessorCode switch
			{
				0 => DefaultData.MonerisPaymentProcessor,
				1 => DefaultData.StripePaymentProcessor,
				2 => DefaultData.IatsPaymentProcessor,
				_ => throw new NotSupportedException($"PaymentProcessorCode with value {paymentProcessorCode} not supported."),
			};

			var configuration = DefaultData.Configuration(processor);

			this.db.Add(processor);
			this.db.Add(configuration);
			this.db.Add(currency);

			var random = new Random();
			var requests = Enumerable.Range(0, max).Select(x =>
			{
				var info = new PaymentInfo
				{
					PaymentProcessor = processor,
					Configuration = configuration,
					TransactionCurrency = currency
				};

				switch (processor.PaymentGatewayType)
				{
					case PaymentGatewayCode.Moneris:
						info.Input = CreateMonerisPaymentInput(info);
						break;
					case PaymentGatewayCode.Iats:
						info.Input = CreateIatsPaymentInput(info);
						break;
					case PaymentGatewayCode.Stripe:
						info.Input = CreateStripePaymentInput(info);
						break;
					default:
						break;
				}

				return info;
			});

			return requests;
		}

		private Transaction CreateTransaction(PaymentInfo paymentInfo)
		{
			return new Transaction
			{
				PaymentProcessor = paymentInfo.PaymentProcessor,
				Configuration = paymentInfo.Configuration,
				Amount = paymentInfo.PaymentAmount,
				AmountReceipted = paymentInfo.PaymentAmount - 0m,
				AmountMembership = 0m,
				AmountTax = 0m,
				BookDate = DateTime.Now,
				CreatedOn = DateTime.Now,
				PaymentTypeCode = paymentInfo.IsBankProcess ? PaymentTypeCode.BankAccount : PaymentTypeCode.CreditCard,
				StateCode = 0, // active
				TransactionCurrency = paymentInfo.TransactionCurrency
			};
		}

		private PaymentMethod CreatePaymentMethod(PaymentInfo paymentInfo)
		{
			return new PaymentMethod
			{
				CreatedOn = DateTime.Now,
				Type = 703650000,
				NameOnFile = paymentInfo.FirstName + " " + paymentInfo.LastName,
				PaymentProcessor = paymentInfo.PaymentProcessor,
				IsReusable = paymentInfo.IsRecurring, // Recurring donations need this to be set to true, false otherwise.
				Name = paymentInfo.CreditCardNo.Substring(paymentInfo.CreditCardNo.Length - 4),
				AuthToken = paymentInfo.Output.AuthToken,
				StripeCustomerId = paymentInfo.Output.StripeCustomerId,
				CcLast4 = "1234",
				CcExpMmYy = "1229",
			};
		}

		private PaymentSchedule CreatePaymentSchedule(PaymentInfo paymentInfo)
		{
			DateTime? NextPaymentDate = DateTime.Now.AddMinutes(5); //should reprocess every 5 minutes
			DateTime? LastPaymentDate = DateTime.Now;
			DateTime? FirstPaymentDate = DateTime.Now.AddDays(-1);
			DateTime? BookDate = DateTime.Now;
			var customerId = Guid.NewGuid();

			var frequency = FrequencyType.Daily;
			var frequencyInterval = (int)FrequencyIntervalCode.Once;
			var frequencyStartCode = FrequencyStart.CurrentDay;

			return new PaymentSchedule
			{
				RecurringAmount = paymentInfo.PaymentAmount,
				AmountReceipted = paymentInfo.PaymentAmount - 0m,
				AmountMembership = 0m,
				AmountTax = 0m,

				Frequency = frequency,
				FrequencyInterval = frequencyInterval,
				FrequencyStartCode = frequencyStartCode,
				ScheduleTypeCode = 703650010,

				FirstPaymentDate = FirstPaymentDate,
				NextPaymentDate = NextPaymentDate,
				LastPaymentDate = LastPaymentDate,
				BookDate = BookDate,
				CreatedOn = DateTime.Now,
				StateCode = 0,
				StatusCode = StatusCode.Active,

				TransactionCurrency = paymentInfo.TransactionCurrency,
				Configuration = paymentInfo.Configuration,
				PaymentProcessor = paymentInfo.PaymentProcessor,
				PaymentTypeCode = paymentInfo.IsBankProcess ? PaymentTypeCode.BankAccount : PaymentTypeCode.CreditCard,

				BillingCity = paymentInfo.BillingCity,
				BillingCountry = paymentInfo.BillingCountry,
				BillingLine1 = paymentInfo.BillingLine1,
				BillingPostalCode = paymentInfo.BillingPostalCode,
				BillingStateorProvince = paymentInfo.BillingStateorProvince,
				EmailAddress1 = paymentInfo.EmailAddress1,
				FirstName = paymentInfo.FirstName,
				LastName = paymentInfo.LastName,
				OrganizationName = paymentInfo.OrganizationName,
				Telephone1 = paymentInfo.Telephone1,

				CustomerId = customerId,

				DataEntrySource = 703650003 // Online
			};
		}

		private PaymentInput CreatePaymentInput(CreditCardPaymentInput input, PaymentInfo info)
		{
			input.IsRecurring = info.IsRecurring;
			input.FirstName = info.FirstName;
			input.LastName = info.LastName;
			input.CreditCardNo = info.CreditCardNo;
			input.CcExpMmYy = $"{DateTime.Now:MM}{DateTime.Now.AddYears(1):yy}";
			input.Cvc = "123";
			input.EmailAddress = info.EmailAddress1;
			input.Amount = info.PaymentAmount;
			input.BillingLine1 = info.BillingLine1;
			input.BillingCity = info.BillingCity;
			input.BillingPostalCode = info.BillingPostalCode;

			return input;
		}

		private PaymentInput CreateMonerisPaymentInput(PaymentInfo info)
		{
			var input = new MonerisCreditCardPaymentInput
			{
				IsBankProcess = false,
				ServiceKey = info.PaymentProcessor.MonerisApiKey,
				MonerisStoreId = info.PaymentProcessor.MonerisStoreId,
				IsTestMode = (bool)info.PaymentProcessor.MonerisTestMode
			};

			return CreatePaymentInput(input, info);
		}

		private PaymentInput CreateIatsPaymentInput(PaymentInfo info)
		{
			var input = new IatsCreditCardPaymentInput
			{
				IatsAgentCode = info.PaymentProcessor.IatsAgentCode,
				IatsPassword = info.PaymentProcessor.IatsPassword,
				BillingStateorProvince = info.BillingStateorProvince,
				Amount = 3,
				Telephone = info.Telephone1,
				AccountNumberFormat = AccountNumberFormat.None
			};

			return CreatePaymentInput(input, info);
		}

		private PaymentInput CreateStripePaymentInput(PaymentInfo info)
		{
			var input = new StripeCreditCardPaymentInput
			{
				IsoCurrencyCode = info.TransactionCurrency.IsoCurrencyCode,
				ServiceKey = info.PaymentProcessor.StripeServiceKey,
			};

			return CreatePaymentInput(input, info);
		}
	}
}
