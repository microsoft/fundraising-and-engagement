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
	public class ScotiaBankFileReport :BankRunFileReport
	{
		private List<PaymentSchedule> Debits { get; set; }
		private List<PaymentSchedule> Credits { get; set; }

		public ScotiaBankFileReport(BankRun bankRun, PaymentProcessor paymentProcessor, PaymentMethod paymentMethod, PaymentContext dataContext, ILogger logger) : base(bankRun, paymentProcessor, paymentMethod, dataContext, logger)
		{
			Debits = new List<PaymentSchedule>();
			Credits = new List<PaymentSchedule>();
		}


		public override async Task GenerateFileReport()
		{
			logger.LogInformation("Generating Scotiabank Report");

			string reportContents;

			string headerRecord = await GenerateHeaderRecord();
			string customerInformationRecord = await GenerateCustomerInformationRecord();
			string creditAndDebitRecords = await GenerateCreditAndDebitRecords();
			string trailerRecord = await GenerateTrailerRecord();

			reportContents = headerRecord + customerInformationRecord + creditAndDebitRecords + trailerRecord;
			ReportContents = reportContents;
		}


		private async Task<string> GenerateHeaderRecord()
		{
			logger.LogInformation("Entering GenerateHeaderRecord");

			string headerRow="";

			string recordType = "A";
			string recordCount = "000000001";

			string customerNumber = GetSpecificLengthString(PaymentProcessor.ScotiabankCustomerNumber, 10);

			// This should be 0000 for each Test file
			// Prod files shuld have unique values (0001 through 9999)
			// can we use crm autonumber or some sql db equivalent?
			string fileCreationNumber =
				GetSpecificLengthString(BankRun.FileCreationNumber.ToString(), 4, 0, '0', false);
			string fileCreationDate = FormatDateValueToJulianFormat(DateTimeNow);

			string scotiaBankDataCentre = "00220";
			string serviceIdentifier = "D";
			string filler1 = new string(' ', 6);
			string sdVer = new string(' ', 11);
			string filler2 = new string(' ', 52);

			headerRow = recordType + recordCount + customerNumber + fileCreationNumber + fileCreationDate +
			            scotiaBankDataCentre + serviceIdentifier + filler1 + sdVer + filler2 + Environment.NewLine;
			
			logger.LogInformation("Exiting GenerateHeaderRecord");
			return headerRow;
		}

		private async Task<string> GenerateCustomerInformationRecord()
		{
			logger.LogInformation("Entering GenerateCustomerInformationRecord"); 
			string customerInformationRecord="";

			string recordType = "Y";
			string originatorShortName = GetSpecificLengthString(PaymentProcessor.OriginatorShortName, 15);
			string originatorLongName = GetSpecificLengthString(PaymentProcessor.OriginatorLongName, 30);
			string returnInstitutionCode = GetSpecificLengthString(PaymentMethod.BankActRtNumber, 3);
			string returnBranchTransitNumber = GetSpecificLengthString(PaymentMethod.BankActRtNumber, 5, 3);
			string returnAccountNumber = GetSpecificLengthString(PaymentMethod.BankActNumber, 12);
			string filler = new string(' ', 39);

			customerInformationRecord = recordType + originatorShortName + originatorLongName + returnInstitutionCode +
			                            returnBranchTransitNumber + returnAccountNumber + filler + Environment.NewLine;
			logger.LogInformation("Exiting GenerateCustomerInformationRecord");
			return customerInformationRecord;
		}

		// For now we are only dealing with Debit records where the Transaction Type is 480 (Donation)
		private async Task<string> GenerateCreditAndDebitRecords()
		{
			logger.LogInformation("Entering GenerateCreditAndDebitRecords");
			
			StringBuilder creditAndDebitRecords = new StringBuilder();
			List<PaymentSchedule> paymentSchedules = GetPaymentSchedulesFromBankRun(BankRun);
			foreach (PaymentSchedule curPaymentSchedule in paymentSchedules)
			{
				PaymentMethod curPaymentMethod = GetPaymentMethodFromPaymentSchedule(curPaymentSchedule);
				string recordType = "D";
				string transactionType = "480"; // Donation - for now, we are only dealing with this Transaction Type
				string amount = FormatCurrencyValue(curPaymentSchedule.RecurringAmount, 10);
				string dueDate = FormatDateValueToJulianFormat(BankRun.DateToBeProcessed);
				string filler = " ";
				string institutionCode = GetSpecificLengthString(curPaymentMethod.BankActRtNumber, 3);
				string transitNumber = GetSpecificLengthString(curPaymentMethod.BankActRtNumber, 5, 3);
				string accountNumber = GetSpecificLengthString(curPaymentMethod.BankActNumber, 12);
				string recipientsName = GetSpecificLengthString(curPaymentSchedule.LastName + ", " + curPaymentSchedule.FirstName, 30);
				string originatorsCrossReferenceNumber = GetSpecificLengthString(curPaymentSchedule.Name, 19);
				string customerSundryInformation = GetSpecificLengthString(originatorsCrossReferenceNumber, 15);
				
				string currentRecord = recordType + transactionType + amount + dueDate + filler + institutionCode +
				                       transitNumber + accountNumber + recipientsName +
				                       originatorsCrossReferenceNumber + customerSundryInformation;
				Debits.Add(curPaymentSchedule);
				creditAndDebitRecords.AppendLine(currentRecord);
			}

			logger.LogInformation("Exiting GenerateCreditAndDebitRecords");
			return creditAndDebitRecords.ToString();
		}

		private async Task<string> GenerateTrailerRecord()
		{
			logger.LogInformation("Entering GenerateTrailerRecord");

			string trailerRecord = "";

			string recordType = "Z";
			string filler1 = new string(' ', 9);
			string customerNumber = GetSpecificLengthString(PaymentProcessor.ScotiabankCustomerNumber, 10);
			string fileCreationNumber = GetSpecificLengthString(BankRun.FileCreationNumber.ToString(), 4, 0, '0', false);
			string totalValueOfDebits = getTotalValueOfDebits();
			string totalNumberOfDebits = getTotalNumberOfDebits();
			string totalValueOfCredits = getTotalValueOfCredits();
			string totalNumberOfCredits = getTotalNumberOfCredits();
			string filler2 = new string(' ', 37);

			trailerRecord = recordType + filler1 + customerNumber + fileCreationNumber + totalValueOfDebits +
			                totalNumberOfDebits + totalValueOfCredits + totalNumberOfCredits + filler2 + Environment.NewLine;

			logger.LogInformation("Entering GenerateTrailerRecord");
			return trailerRecord;
		}

		private string getTotalValueOfDebits()
		{
			decimal totalDebits = Debits.Sum(d => d.RecurringAmount.Value);
			return FormatCurrencyValue(totalDebits, 14);
		}
		private string getTotalNumberOfDebits()
		{
			return GetSpecificLengthString(Debits.Count.ToString(),8, 0, '0', false);
		}

		private string getTotalValueOfCredits()
		{
			decimal totalCredits = Credits.Sum(c => c.RecurringAmount.Value);
			return FormatCurrencyValue(totalCredits, 14);
		}
		private string getTotalNumberOfCredits()
		{
			return GetSpecificLengthString(Credits.Count.ToString(), 8, 0, '0', false);
		}



	}
}
