using System;
using FundraisingandEngagement.Models.Enums;
using FundraisingandEngagement.Models;
using NUnit.Framework;

namespace FundraisingandEngagement.Tests.Models
{
    [TestFixture]
    public class DateExtensions
    {
        [TestCase("2016-01-01", FrequencyStart.CurrentDay, "2016-02-01")]
        [TestCase("2016-01-02", FrequencyStart.CurrentDay, "2016-02-02")]
        [TestCase("2016-03-31", FrequencyStart.CurrentDay, "2016-04-30", TestName = "Long month to short month")]
        [TestCase("2016-04-30", FrequencyStart.CurrentDay, "2016-05-30", TestName = "Short month to long month")]
        [TestCase("2016-01-29", FrequencyStart.CurrentDay, "2016-02-29", TestName = "Leap year")]
        [TestCase("2017-01-29", FrequencyStart.CurrentDay, "2017-02-28", TestName = "Not Leap year")]
        [TestCase("2017-01-29", FrequencyStart.FirstOfMonth, "2017-02-01")]
        [TestCase("2016-01-01", FrequencyStart.FirstOfMonth, "2016-02-01")]
        [TestCase("2016-01-15", FrequencyStart.FifteenthOfMonth, "2016-02-15")]
        [TestCase("2016-01-01", FrequencyStart.FifteenthOfMonth, "2016-02-15")]
        [TestCase("2016-01-18", FrequencyStart.FifteenthOfMonth, "2016-02-15")]
        [TestCase("2016-12-29", FrequencyStart.CurrentDay, "2017-01-29")]
        [TestCase("2016-12-29", FrequencyStart.FirstOfMonth, "2017-01-01")]
        [TestCase("2016-12-29", FrequencyStart.FifteenthOfMonth, "2017-01-15")]
        [Test]
        public void TestGetMonthlyRecurranceDate_AddOneMonth_DateEquals(string nowString, FrequencyStart start, string expected)
        {
            var now = DateTime.Parse(nowString);
            var next = start.GetMonthlyRecurranceDate(now, 1);

            Assert.AreEqual(DateTime.Parse(expected), next);
        }

        [Test]
        public void TestGetMonthlyRecurranceDate_AddMoreMonths_DateEquals()
        {
            var now = DateTime.Parse("2016-01-31");
            var next = FrequencyStart.CurrentDay.GetMonthlyRecurranceDate(now, 2);

            Assert.AreEqual(DateTime.Parse("2016-03-29"), next);
        }
    }
}
