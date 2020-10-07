// the code has a lot of warning messages, possble possible issues
#pragma warning disable CS8629 // Nullable value type may be null.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning disable CS8602 // Dereference of a possibly null reference.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement
{
	public class BMOFileReport : BankRunFileReport
	{
		private List<PaymentSchedule> Debits { get; set; }
		private List<PaymentSchedule> Credits { get; set; }


		public BMOFileReport(BankRun bankRun, PaymentProcessor paymentProcessor, PaymentMethod paymentMethod, PaymentContext dataContext, ILogger logger) : base(bankRun, paymentProcessor, paymentMethod, dataContext, logger)
		{
			Debits = new List<PaymentSchedule>();
			Credits = new List<PaymentSchedule>();

		}


		public override async Task GenerateFileReport()
		{
			logger.LogInformation("Generating BMO Report");

			string reportContents;

			string fileHeaderRecord = await GenerateFileHeaderRecord();
			string batchHeaderRecord = await GenerateBatchHeaderRecord();
			string debitRecords = await GenerateDebitRecords();
			string batchControlRecord = await GenerateBatchControlRecord();
			string fileControlRecord = await GenerateFileControlRecord();

			reportContents = fileHeaderRecord + batchHeaderRecord + debitRecords + batchControlRecord +
			                 fileControlRecord;
			ReportContents = reportContents;
		}

		private async Task<string> GenerateFileHeaderRecord()
		{
			logger.LogInformation("Entering GenerateHeaderRecord");

			string headerRow = "";

			string logicalRecordId = "A";
			string originatorId = GetSpecificLengthString(PaymentProcessor.BmoOriginatorId, 10);
			string fileCreationNumber = GetSpecificLengthString(BankRun.FileCreationNumber.ToString(), 4, 0, '0', false);
			string fileCreationDate = FormatDateValueToJulianFormat(DateTimeNow);
			string destinationDataCentreCode = GetSpecificLengthString("00110", 5);
			string filler = new string(' ', 54);

			headerRow = logicalRecordId + originatorId + fileCreationNumber + fileCreationDate +
			            destinationDataCentreCode + filler + Environment.NewLine;

			logger.LogInformation("Exiting GenerateHeaderRecord");
			return headerRow;
		}

		private async Task<string> GenerateBatchHeaderRecord()
		{
			logger.LogInformation("Entering GenerateBatchHeaderRecord");
			string batchHeaderRow = "";

			string logicalRecordType = "X";
			string batchPaymentType = "D"; // Only worried about Debits for now
			string transactionTypeCode = "480"; // 480 = Donations
			string datePayable = FormatDateValueToJulianFormat(BankRun.DateToBeProcessed);
			string originatorShortName = GetSpecificLengthString(PaymentProcessor.OriginatorShortName, 15);
			string originatorLongName = GetSpecificLengthString(PaymentProcessor.OriginatorLongName, 30);
			string returnInstitutionId = "0" + GetSpecificLengthString(PaymentMethod.BankActRtNumber, 3) +
			                       GetSpecificLengthString(PaymentMethod.BankActRtNumber, 5, 3);
			string returnAccountNumber = GetSpecificLengthString(PaymentMethod.BankActNumber, 12);
			string filler = new string(' ', 3);

			batchHeaderRow = logicalRecordType + batchPaymentType + transactionTypeCode + datePayable +
			                 originatorShortName + originatorLongName + returnInstitutionId + returnAccountNumber +
							 filler + Environment.NewLine;

			logger.LogInformation("Exiting GenerateBatchHeaderRecord");
			return batchHeaderRow;
		}

		private async Task<string> GenerateDebitRecords()
		{
			logger.LogInformation("Entering GenerateDebitRecords");
			
			StringBuilder debitRecords = new StringBuilder();
			List<PaymentSchedule> paymentSchedules = GetPaymentSchedulesFromBankRun(BankRun);
			foreach (PaymentSchedule curPaymentSchedule in paymentSchedules)
			{
				PaymentMethod curPaymentMethod = GetPaymentMethodFromPaymentSchedule(curPaymentSchedule);

				string logicalRecordTypeId = "D";
				string amount = FormatCurrencyValue(curPaymentSchedule.RecurringAmount, 10);
				string payorInstitutionId = "0" + GetSpecificLengthString(curPaymentMethod.BankActRtNumber, 3) +
				                            GetSpecificLengthString(curPaymentMethod.BankActRtNumber, 5, 3);
				string payorAccountNumber = GetSpecificLengthString(curPaymentMethod.BankActNumber, 12);
				string payorName = GetSpecificLengthString(curPaymentSchedule.FirstName + " " + curPaymentSchedule.LastName, 29);
				string crossReferenceNumber = GetSpecificLengthString(curPaymentSchedule.Name, 15);
				string filler = new string(' ', 4);

				string currentRecord = logicalRecordTypeId + amount + payorInstitutionId + payorAccountNumber +
				                       payorName + crossReferenceNumber + filler;
				Debits.Add(curPaymentSchedule);
				debitRecords.AppendLine(currentRecord);
			}

			logger.LogInformation("Exiting GenerateDebitRecords");
			return debitRecords.ToString();
		}

		private async Task<string> GenerateBatchControlRecord()
		{
			logger.LogInformation("Entering GenerateBatchControlRecord");

			string batchControlRow = "";

			string logicalRecordTypeID = "Y";
			string batchPaymentType = "D"; // Only worried about Debits for now
			string batchRecordCount = getTotalNumberOfDebits(8); // this would be calculated differently if we were dealing with multiple batches
			string batchAmount = getTotalValueOfDebits(14); // this would be calculated differently if we were dealing with multiple batches
			string filler = new string(' ', 56);

			batchControlRow = logicalRecordTypeID + batchPaymentType + batchRecordCount + batchAmount + filler + Environment.NewLine;

			logger.LogInformation("Exiting GenerateBatchControlRecord");
			return batchControlRow;
		}

		private async Task<string> GenerateFileControlRecord()
		{
			logger.LogInformation("Entering GenerateFileControlRecord");

			string fileControlRow = "";

			string logicalRecordId = "Z";
			string totalValueOfDebits = getTotalValueOfDebits(14);
			string totalNumberOfDebits = getTotalNumberOfDebits(5);
			string totalValueOfCredits = getTotalValueOfCredits(14);
			string totalNumberOfCredits = getTotalNumberOfCredits(5);
			string filler = new string(' ', 41);

			fileControlRow = logicalRecordId + totalValueOfDebits + totalNumberOfDebits + totalValueOfCredits +
			                 totalNumberOfCredits + filler + Environment.NewLine;

			logger.LogInformation("Exiting GenerateFileControlRecord");
			return fileControlRow;
		}


		private string getTotalValueOfDebits(int desiredLength)
		{
			decimal totalDebits = Debits.Sum(d => d.RecurringAmount.Value);
			return FormatCurrencyValue(totalDebits, desiredLength);
		}
		private string getTotalNumberOfDebits(int desiredLength)
		{
			return GetSpecificLengthString(Debits.Count.ToString(), desiredLength, 0, '0', false);
		}

		private string getTotalValueOfCredits(int desiredLength)
		{
			decimal totalCredits = Credits.Sum(c => c.RecurringAmount.Value);
			return FormatCurrencyValue(totalCredits, desiredLength);
		}
		private string getTotalNumberOfCredits(int desiredLength)
		{
			return GetSpecificLengthString(Credits.Count.ToString(), desiredLength, 0, '0', false);
		}


	}
}
