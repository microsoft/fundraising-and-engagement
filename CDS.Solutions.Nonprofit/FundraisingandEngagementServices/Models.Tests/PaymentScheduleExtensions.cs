using System;
using System.Collections.Generic;
using System.Text;
using FundraisingandEngagement.Models.Entities;
using FundraisingandEngagement.Models.Enums;
using NUnit.Framework;

namespace Models.Tests
{
    [TestFixture]
    public class PaymentScheduleExtensions
    {
        [Test]
        [TestCase("2016-01-01", FrequencyType.Days, null, "2016-01-02")]
        [TestCase("2016-01-31", FrequencyType.Days, null, "2016-02-01")]
        [TestCase("2016-12-31", FrequencyType.Days, null, "2017-01-01")]
        [TestCase("2016-01-01", FrequencyType.Months, FrequencyStart.CurrentDay, "2016-02-01")]
        [TestCase("2016-01-31", FrequencyType.Months, FrequencyStart.CurrentDay, "2016-02-29")]
        [TestCase("2016-12-31", FrequencyType.Months, FrequencyStart.CurrentDay, "2017-01-31")]
        [TestCase("2016-01-01", FrequencyType.Weeks, null, "2016-01-08")]
        [TestCase("2016-01-31", FrequencyType.Weeks, null, "2016-02-07")]
        [TestCase("2016-12-31", FrequencyType.Weeks, null, "2017-01-07")]
        [TestCase("2016-01-01", FrequencyType.Years, null, "2017-01-01")]
        [TestCase("2016-01-31", FrequencyType.Years, null, "2017-01-31")]
        [TestCase("2016-12-31", FrequencyType.Years, null, "2017-12-31")]
        [TestCase("2016-02-29", FrequencyType.Years, null, "2017-02-28")]
        public void GetNextDonationDate_AddOne_IsExpected(string nextPayment, FrequencyType type, FrequencyStart start, string expected)
        {
            var schedule = new PaymentSchedule()
            {
                NextPaymentDate = DateTime.Parse(nextPayment),
                FrequencyInterval = 1,
                Frequency = type,
                FrequencyStartCode = start
            };
            var scheduleHash = schedule.GetHashCode();
            var next = schedule.GetNextDonationDate();
            
            // TODO: Look at why this code exist in methods...
           var expectedDate = DateTime.Parse(expected);

           if (expectedDate.Hour == 0 && expectedDate.Minute == 0 && expectedDate.Second == 0 && expectedDate.Millisecond == 0)
            {
                var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                expectedDate = expectedDate.AddHours(Math.Abs(offset.Hours));
            }

            Assert.AreEqual(scheduleHash, schedule.GetHashCode(), "Assert schedule not changed");
            Assert.AreEqual(expectedDate, next, "Assert expected date");
        }
    }
}
