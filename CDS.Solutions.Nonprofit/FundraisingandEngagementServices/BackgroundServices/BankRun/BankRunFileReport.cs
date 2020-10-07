// the code has a lot of warning messages, possble possible issues
#pragma warning disable CS8629 // Nullable value type may be null.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement
{
	public abstract class BankRunFileReport
	{
		public PaymentContext DataContext { get; set; }
		protected ILogger logger { get; set; }
		public string ReportContents { get; set; }
		public BankRun BankRun { get; set; }
		//public Configuration Configuration { get; set; }
		public PaymentProcessor PaymentProcessor { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
		public DateTime DateTimeNow { get; set; }

		public string BankRunObjectTypeCode
		{
			get
			{
				return "msnfp_bankrun";
			}
		}

		public string fileExtension { get; set; }

		public BankRunFileReport()
		{
			DateTimeNow = DateTime.Now;
		}
		public BankRunFileReport(BankRun bankRun, PaymentProcessor paymentProcessor, PaymentMethod paymentMethod, PaymentContext dataContext, ILogger logger)
		{
			DataContext = dataContext;
			BankRun = bankRun;
			//Configuration = configuration;
			PaymentProcessor = paymentProcessor;
			PaymentMethod = paymentMethod;
			DateTimeNow = DateTime.Now;
			this.logger = logger;
		}

		public abstract Task GenerateFileReport();
		
		public async Task SaveReport()
		{
			logger.LogInformation("Entering SaveReport");

			string fileName = BankRun.Identifier;
			if (!String.IsNullOrEmpty(fileExtension))
			{
				fileName += "." + fileExtension.Replace(".", "");
			}

			string documentBody = Convert.ToBase64String(Encoding.ASCII.GetBytes(ReportContents));
			logger.LogInformation("Document Body converted to Base64. Length:" + documentBody.Length);

			Note note = new Note();
			note.Description = "Bank Run Report for:" + BankRun.Identifier + " Generated On:" + DateTimeNow;
			note.Document = documentBody;
			note.FileName = fileName;
			note.MimeType = @"text/plain";
			note.IsDocument = true;
			note.Title = "Bank Run Report for:" + BankRun.Identifier + " Generated On:" + DateTimeNow;
			note.ObjectType = BankRunObjectTypeCode;
			note.RegardingObjectId = BankRun.BankRunId;

			DateTime syncDate = DateTime.Now;
			//note.SyncDate = syncDate;
			note.CreatedOn = syncDate;

			DataContext.Note.Add(note);
			DataContext.SaveChanges();

			logger.LogInformation("Report Saved");
		}


		#region Record Retrieval Functions

		#region Static Methods

		public static Guid GetGuidParameter(string parameterName, IConfiguration configuration, ILogger logger)
		{
			// For local testing, you can use ConfigGuid=GUIDINNFPDEV as an Application Argument in the Project properties debug panel. 
			if (configuration.GetValue<string>(parameterName) == null)
			{
				throw new ArgumentException(parameterName + " is a Required Parameter");
			}

			string paramVal = configuration.GetValue<string>(parameterName);
			logger.LogInformation(parameterName + " = " + paramVal);

			return new Guid(paramVal);
		}

		public static string GetStringParameter(string parameterName, IConfiguration configuration, ILogger logger)
		{
			if (configuration.GetValue<string>(parameterName) == null)
			{
				throw new ArgumentException(parameterName + " is a Required Parameter");
			}

			string paramVal1 = configuration.GetValue<string>(parameterName);
			logger.LogInformation(parameterName + " = " + paramVal1);

			return paramVal1;
		}

		public static BankRun GetBankRunEntityFromId(Guid? bankRunGUID, PaymentContext dataContext)
		{
			BankRun bankRunEntity = dataContext.BankRun.FirstOrDefault(br => br.BankRunId == bankRunGUID);
			if (bankRunEntity == null)
				throw new Exception("Could not retrieve Bank Run record using id:" + bankRunGUID);
			return bankRunEntity;
		}

		public static Configuration GetConfigurationEntityFromId(Guid? configGuid, PaymentContext dataContext)
		{
			Configuration configEntity = dataContext.Configuration.FirstOrDefault(c => c.ConfigurationId == configGuid);
			if (configEntity == null)
				throw new Exception("Could not retrieve Configuration record using id:" + configGuid);
			return configEntity;
		}

		public static PaymentProcessor GetPaymentProcessorEntityFromBankRun(BankRun bankRunEntity, PaymentContext dataContext, ILogger logger)
		{
			if (bankRunEntity.PaymentProcessorId == null)
			{
				throw new Exception("No Payment Processor associated with the Bank Run record.");
			}

			Guid? paymentProcessorGuid = bankRunEntity.PaymentProcessorId;
			logger.LogInformation("Payment Processor Id:" + paymentProcessorGuid);
			PaymentProcessor paymentProcessorEntity =
				dataContext.PaymentProcessor.FirstOrDefault(p => p.PaymentProcessorId == paymentProcessorGuid);
			if (paymentProcessorEntity == null)
				throw new Exception("Could not retrieve Payment Processor record using id:" + paymentProcessorGuid);
			return paymentProcessorEntity;
		}

		public static PaymentMethod GetPaymentMethodEntityFromBankRun(BankRun bankRunEntity, PaymentContext dataContext, ILogger logger)
		{
			if (bankRunEntity.PaymentProcessorId == null)
			{
				throw new Exception("No Payment Processor associated with the Bank Run record.");
			}

			Guid? paymentMethodGuid = bankRunEntity.AccountToCreditId;
			logger.LogInformation("Payment Method Id:" + paymentMethodGuid);
			PaymentMethod paymentMethodEntity =
				dataContext.PaymentMethod.FirstOrDefault(p => p.PaymentMethodId == paymentMethodGuid);
			if (paymentMethodEntity == null)
				throw new Exception("Could not retrieve Payment Method record using id:" + paymentMethodGuid);

			// might as well check for Banking Information here
			if (paymentMethodEntity.BankActNumber == null || paymentMethodEntity.BankActRtNumber == null)
			{
				throw new Exception("Bank Account Number and Routing Number must both contain values.");
			}

			return paymentMethodEntity;
		}

		#endregion

		#region Instance Methods

		public PaymentMethod GetPaymentMethodFromPaymentSchedule(PaymentSchedule paymentSchedule)
		{
			if (paymentSchedule.PaymentMethodId == null)
			{
				logger.LogError("No Payment Method associated with Payment Schedule Id:" + paymentSchedule.PaymentScheduleId);
				return null;
			}

			Guid paymentMethodId = paymentSchedule.PaymentMethodId.Value;
			PaymentMethod paymentMethod =
				DataContext.PaymentMethod.FirstOrDefault(p => p.PaymentMethodId == paymentMethodId);
			return paymentMethod;
		}

		public List<PaymentSchedule> GetPaymentSchedulesFromBankRun(BankRun bankRunEntity)
		{
			List<PaymentSchedule> paymentSchedules = new List<PaymentSchedule>();

			List<BankRunSchedule> bankRunSchedules = GetBankRunSchedulesFromBankRun(bankRunEntity);
			if (bankRunSchedules != null)
			{
				foreach (BankRunSchedule curBankRunSchedule in bankRunSchedules)
				{
					PaymentSchedule curPaymentSchedule = GetPaymentScheduleFromBankRunSchedule(curBankRunSchedule);
					if (curPaymentSchedule != null)
						paymentSchedules.Add(curPaymentSchedule);
				}
			}

			return paymentSchedules;
		}

		public List<BankRunSchedule> GetBankRunSchedulesFromBankRun(BankRun bankRunEntity)
		{
			if (bankRunEntity == null) throw new Exception("Bank Run is Null. Could not retrieve Bank Run Schedules");

			List<BankRunSchedule> bankRunSchedules =
				DataContext.BankRunSchedule.Where(b => b.BankRunId == bankRunEntity.BankRunId && b.StateCode == 0 && (b.Deleted == null || b.Deleted == false))
					.ToList();
			return bankRunSchedules;
		}

		public PaymentSchedule GetPaymentScheduleFromBankRunSchedule(BankRunSchedule bankRunSchedule)
		{
			if (bankRunSchedule.PaymentScheduleId == null)
			{
				logger.LogError("No Payment Schedule associated with Bank Run Schedule Id:" +
				                  bankRunSchedule.BankRunScheduleId);
				return null;
			}

			Guid paymentScheduleId = bankRunSchedule.PaymentScheduleId.Value;
			PaymentSchedule paymentSchedule =
				DataContext.PaymentSchedule.FirstOrDefault(p => p.PaymentScheduleId == paymentScheduleId);
			return paymentSchedule;
		}

		#endregion

		#endregion


		#region Formatting Functions

		// This function will truncate or pad the sourceString in order to return a string of the desiredLength.
		// If sourceStringStartingMimounaosition is >0, then only a substring of the original sourceString is used.
		// padChar defaults to a space character, but can be any char.
		// If padRight is true (default), then padding is added (as needed) to the end (i.e. right side) of the resulting string.
		// Otherwise, padding is added (as needed) to the beginning (i.e. left side) of the string.

		public static string GetSpecificLengthString(string sourceString, int desiredLength,
			int sourceStringStartPosition = 0, char padChar = ' ', bool padRight = true)
		{
			if (sourceString == null) sourceString = "";

			string resultString = "";

			// get a new string which starts at the starting position given by sourceStringstartingPosition
			string sourceFromStartingPosition = "";
			if (sourceString.Length > sourceStringStartPosition)
			{
				sourceFromStartingPosition = sourceString.Substring(sourceStringStartPosition);
			}

			if (sourceFromStartingPosition.Length >= desiredLength)
			{
				resultString = sourceFromStartingPosition.Substring(0, desiredLength);
			}
			else
			{
				if (padRight == true)
				{
					resultString = sourceFromStartingPosition.PadRight(desiredLength, padChar);
				}
				else
				{
					resultString = sourceFromStartingPosition.PadLeft(desiredLength, padChar);
				}
			}

			return resultString;
		}

		public static string FormatCurrencyValue(decimal? origAmount, int desiredLength)
		{
			string formattedAmount;
			if (!origAmount.HasValue) origAmount = 0;


			formattedAmount = origAmount.Value.ToString("0.00").Replace(".", "");
			formattedAmount = GetSpecificLengthString(formattedAmount, desiredLength, 0, '0', false);
			return formattedAmount;
		}

		public static string FormatDateValueToJulianFormat(DateTime? origDate)
		{
			string formattedDate;
			if (!origDate.HasValue) origDate = DateTime.Now;

            formattedDate = "0" + origDate.Value.ToString("yy") +
                            GetSpecificLengthString(origDate.Value.DayOfYear.ToString(), 3, 0, '0', false);
			return formattedDate;
		}


		public static string FormatDateValueToDDMMYY(DateTime? origDate)
		{
			string formattedDate;
			if (!origDate.HasValue) origDate = DateTime.Now;

			formattedDate = origDate.Value.ToString("ddMMyy");
			return formattedDate;
		}

		#endregion
	}
}
