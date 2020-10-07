// the code has a lot of warning messages, possble possible issues
#pragma warning disable CS8629 // Nullable value type may be null.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning disable CS8602 // Dereference of a possibly null reference.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Models.Entities;
using Microsoft.Extensions.Logging;

namespace FundraisingandEngagement
{
	public class AbaFileReport : BankRunFileReport
	{
		private List<PaymentSchedule> Debits { get; set; }
		private List<PaymentSchedule> Credits { get; set; }
		

		public AbaFileReport(BankRun bankRun, PaymentProcessor paymentProcessor, PaymentMethod paymentMethod, PaymentContext dataContext, ILogger logger) : base(bankRun, paymentProcessor, paymentMethod, dataContext, logger)
		{
			Debits = new List<PaymentSchedule>();
			Credits = new List<PaymentSchedule>();
			fileExtension = "aba";

		}



		public override async Task GenerateFileReport()
		{
			logger.LogInformation("Generating ABA Report");

			string reportContents;

			string headerRecord = await GenerateHeaderRecord();
			string creditAndDebitRecords = await GenerateCreditAndDebitRecords();
			string fileTotalRecord = await GenerateFileTotalRecord();

			reportContents = headerRecord + creditAndDebitRecords + fileTotalRecord;
			ReportContents = reportContents;
		}

		private async Task<string> GenerateHeaderRecord()
		{
			logger.LogInformation("Entering GenerateHeaderRecord");

			string headerRow = "";

			string recordType = "0";
			string filler1 = new string(' ', 17);
			string reelSequenceNumber = "01";
			string financialInstitutionName = GetSpecificLengthString(PaymentMethod.BankName,3);
			string filler2 = new string(' ', 7);
			string nameOfUser = GetSpecificLengthString(PaymentProcessor.AbaUserName, 26);
			string numberOfUser = GetSpecificLengthString(PaymentProcessor.AbaUserNumber, 6, 0, '0', false);
			string descriptionOfEntries = GetSpecificLengthString("DONATIONS", 12);
			string dateToBeProcessed = FormatDateValueToDDMMYY(BankRun.DateToBeProcessed);
			string filler3 = new string(' ', 40);

			headerRow = recordType + filler1 + reelSequenceNumber + financialInstitutionName + filler2 + nameOfUser +
			            numberOfUser + descriptionOfEntries + dateToBeProcessed + filler3 + Environment.NewLine;

			logger.LogInformation("Exiting GenerateHeaderRecord");
			return headerRow;
		}

		private async Task<string> GenerateCreditAndDebitRecords()
		{
			logger.LogInformation("Entering GenerateCreditAndDebitRecords");

			StringBuilder creditAndDebitRecords = new StringBuilder();
			List<PaymentSchedule> paymentSchedules = GetPaymentSchedulesFromBankRun(BankRun);
			foreach (PaymentSchedule curPaymentSchedule in paymentSchedules)
			{
				PaymentMethod curPaymentMethod = GetPaymentMethodFromPaymentSchedule(curPaymentSchedule);
				string recordType = "1";
				string bsbNumber = formatBSBNumber(curPaymentMethod.BankActRtNumber);
				string accountNumber = formatBankAccountNumber(curPaymentMethod.BankActNumber, curPaymentMethod.BankName);
				string indicator = " ";
				string transactionCode = "13"; // 13 = debit - only dealing with Debits for now
				string amount = FormatCurrencyValue(curPaymentSchedule.RecurringAmount, 10);
				string titleOfAccount = GetSpecificLengthString(curPaymentMethod.NameAsItAppearsOnTheAccount, 32, 0, ' ', true);
				string lodgementReference = GetSpecificLengthString(curPaymentSchedule.Name, 18, 0, ' ', true);
				string traceBSBNumber = formatBSBNumber(PaymentMethod.BankActRtNumber);
				string traceAccountNumber = formatBankAccountNumber(PaymentMethod.BankActNumber, PaymentMethod.BankName);
				string nameOfRemitter = GetSpecificLengthString(PaymentProcessor.AbaRemitterName, 16);
				string amountOfWithholdingTax = FormatCurrencyValue(0, 8);

				string currentRecord = recordType + bsbNumber + accountNumber + indicator + transactionCode + amount +
				                       titleOfAccount + lodgementReference + traceBSBNumber + traceAccountNumber +
				                       nameOfRemitter + amountOfWithholdingTax;
				
				Debits.Add(curPaymentSchedule);
				creditAndDebitRecords.AppendLine(currentRecord);
			}

			// NAB Connect requires self-balanced files. That is the last transaction within the file must be
			// a settling transaction to your NAB Account for the total file value.
			string sbRecordType = "1";
			string sbBsbNumber = formatBSBNumber(PaymentMethod.BankActRtNumber);
			string sbAccountNumber = formatBankAccountNumber(PaymentMethod.BankActNumber, PaymentMethod.BankName);
			string sbIndicator = " ";
			string sbTransactionCode = "50"; // 50 = credit- this is "balancing out" all the debits from above
			string sbAmount = getTotalValueOfDebits(10);
			string sbTitleOfAccount = GetSpecificLengthString(PaymentMethod.NameAsItAppearsOnTheAccount, 32, 0, ' ', true);
			string sbLodgementReference = GetSpecificLengthString(PaymentProcessor.AbaUserNumber, 18, 0, ' ', true);
			string sbTraceBSBNumber = formatBSBNumber(PaymentMethod.BankActRtNumber);
			string sbTraceAccountNumber = formatBankAccountNumber(PaymentMethod.BankActNumber, PaymentMethod.BankName);
			string sbNameOfRemitter = GetSpecificLengthString(PaymentProcessor.AbaRemitterName, 16);
			string sbAmountOfWithholdingTax = FormatCurrencyValue(0, 8);

			string sbRecord = sbRecordType + sbBsbNumber + sbAccountNumber + sbIndicator + sbTransactionCode + sbAmount +
			                       sbTitleOfAccount + sbLodgementReference + sbTraceBSBNumber + sbTraceAccountNumber +
			                       sbNameOfRemitter + sbAmountOfWithholdingTax;


			creditAndDebitRecords.AppendLine(sbRecord);
			logger.LogInformation("Exiting GenerateCreditAndDebitRecords");
			return creditAndDebitRecords.ToString();
		}

		private async Task<string> GenerateFileTotalRecord()
		{
			logger.LogInformation("Entering GenerateFileTotalRecord");

			string fileTotalRow = "";

			string recordType = "7";
			//string bsbNumber = formatBSBNumber(PaymentMethod.BankActRtNumber);
			string bsbNumber = "999-999";
			string filler1 = new string(' ', 12);
			string fileNetTotalAmount = FormatCurrencyValue(0, 10); // Should be all zeros in a self-balanced file (which this is)
			string fileCreditTotalAmount = getTotalValueOfDebits(10); // Should equal to debit total if self - balanced
			string fileDebitTotalAmount = getTotalValueOfDebits(10); // Should equal to credit total if self - balanced
			string filler2 = new string(' ', 24);
			string countOfDebitsOrCredits =
				GetSpecificLengthString((Debits.Count + 1).ToString(), 6, 0, '0', false); // Add one to the count of Debits so that the self-balancing record is included.
			string filler3 = new string(' ', 40);

			fileTotalRow = recordType + bsbNumber + filler1 + fileNetTotalAmount + fileCreditTotalAmount + fileDebitTotalAmount +
			               filler2 + countOfDebitsOrCredits + filler3;

			logger.LogInformation("Exiting GenerateFileTotalRecord");
			return fileTotalRow;
		}


		// format for a BSB Numer = XXX-XXX (where X is a digit)
		private string formatBSBNumber(string rtNumber)
		{
			if (rtNumber == null)
			{
				// if this happens, we'll probably get an error submitting the file,
				// but the file processor should let the user know why
				return new string(' ', 7);
			}

			if (Regex.IsMatch(rtNumber, "\\d{3}-\\d{3}"))
			{
				return rtNumber;
			}

			if (rtNumber.Length == 6)
			{
				return rtNumber.Insert(3, "-");
			}

			// if we get this far, we'll probably get an error submitting the file,
			// but the file processor should let the user know why
			return GetSpecificLengthString(rtNumber, 7, 3);
		}

		// NAB accounts must be numeric, zero filled.
		// Non-Nab accounts can be alphanumeric, right justified, blank filled.
		// Must not be all blanks or zeros.
		// Must not contain hyphens.
		// TODO: Check the formatting on this. It depends on the type of account
		private string formatBankAccountNumber(string origBankAccountNumber, string financialInstitutionName)
		{
			if (origBankAccountNumber == null)
			{
				// if this happens, we'll probably get an error submitting the file,
				// but the file processor should let the user know why
				return new string(' ', 9);
			}

			if (financialInstitutionName != null && financialInstitutionName.ToLower() == "nab")
			{
				return GetSpecificLengthString(origBankAccountNumber, 9, 0, '0', false);
			}
			else
			{
				return GetSpecificLengthString(origBankAccountNumber.Replace("-", " "), 9, 0, ' ', false);
			}
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
