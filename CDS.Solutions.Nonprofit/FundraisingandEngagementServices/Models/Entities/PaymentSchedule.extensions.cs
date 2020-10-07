using System;
using FundraisingandEngagement.Models.Enums;

namespace FundraisingandEngagement.Models.Entities
{
	public static class PaymentScheduleExtensions
	{
		public static DateTime GetNextDonationDate(this PaymentSchedule payment)
		{
			if (payment.NextPaymentDate == null)
				throw new NullReferenceException($"{nameof(PaymentSchedule.NextPaymentDate)} can not be null.");

			var instance = payment.FrequencyInterval ?? 1;
			var frequency = payment.Frequency ?? FrequencyType.Monthly;
			var frequencyStart = payment.FrequencyStartCode ?? FrequencyStart.CurrentDay;

			var nextDonationDate = payment.NextPaymentDate.Value;

			switch (frequency)
			{
				case FrequencyType.Daily:
					nextDonationDate = nextDonationDate.AddDays(1);
					break;

				case FrequencyType.Weekly:
					nextDonationDate = nextDonationDate.AddDays(7);
					break;

				case FrequencyType.Monthly:
					nextDonationDate = frequencyStart.GetMonthlyRecurranceDate(nextDonationDate, instance);
					break;

				case FrequencyType.Annually:
					nextDonationDate = nextDonationDate.AddYears(instance);
					break;
			}

			// We need to add the local time zone offset as Dynamics converts it to the users local time. So 12am will appear as the previous day otherwise:
			// We only do it on these records as going forward this issue has been fixed by not dropping the time info.
			if (nextDonationDate.Hour == 0 && nextDonationDate.Minute == 0 && nextDonationDate.Second == 0 && nextDonationDate.Millisecond == 0)
			{
				var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
				nextDonationDate = nextDonationDate.AddHours(Math.Abs(offset.Hours));
			}

			return nextDonationDate;
		}
	}
}
