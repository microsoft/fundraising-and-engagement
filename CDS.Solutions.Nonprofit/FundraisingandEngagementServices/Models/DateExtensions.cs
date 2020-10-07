using System;
using FundraisingandEngagement.Models.Enums;

namespace FundraisingandEngagement.Models
{
	public static class DateExtensions
	{
		public static DateTime GetMonthlyRecurranceDate(this FrequencyStart frequencyStart, DateTime nextDonationDate, int instance = 1)
		{
			switch (frequencyStart)
			{
				case FrequencyStart.CurrentDay:

					var lastDateOfCurrentMonth = GetLastDateOfMonth(nextDonationDate);
					var lastDateofNextmonth = GetLastDateOfNextMonth(nextDonationDate);

					if (nextDonationDate.Date.Day > lastDateofNextmonth.Date.Day)
					{
						if (lastDateOfCurrentMonth.Date.Day > lastDateofNextmonth.Date.Day)
							nextDonationDate = lastDateofNextmonth;
						else
							nextDonationDate = nextDonationDate.AddMonths(1);
					}
					else
					{
						nextDonationDate = nextDonationDate.AddMonths(1);
					}

					break;

				case FrequencyStart.FirstOfMonth:
					nextDonationDate = GetFirstDateOfNextMonth(nextDonationDate);
					break;

				case FrequencyStart.FifteenthOfMonth:
					nextDonationDate = GetFifteenthDateOfNextMonth(nextDonationDate);
					break;
			}

			if (instance > 1)
			{
				nextDonationDate = nextDonationDate.AddMonths(instance - 1);
			}

			return nextDonationDate;
		}

		private static DateTime GetLastDateOfMonth(DateTime value)
		{
			value = value.AddMonths(1);
			var LastDateOfMonth = value.AddDays(-(value.Day));

			return LastDateOfMonth;
		}

		private static DateTime GetLastDateOfNextMonth(DateTime value)
		{
			value = value.AddMonths(2);
			var LastDateOfMonth = value.AddDays(-(value.Day));

			return LastDateOfMonth;
		}

		private static DateTime GetFirstDateOfNextMonth(DateTime value)
		{
			return value.AddDays(-value.Day + 1).AddMonths(1);
		}

		private static DateTime GetFifteenthDateOfNextMonth(DateTime value)
		{
			return value.AddDays(-value.Day + 15).AddMonths(1);
		}
	}
}
