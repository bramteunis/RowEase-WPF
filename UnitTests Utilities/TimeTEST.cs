using Utilities;

namespace UnitTests_Utilities
{
    [TestClass]
    public class TimeTEST
    {
        [TestMethod]
        public void CalculateSecondsInTimespanTEST1WithDot()
        {
            // Arrange
            string[] times = { "0.5 Uur", "1 Uur", "1.5 Uur", "2 Uur" };
            int[] expectedSeconds = { 1800, 3600, 5400, 7200 };
            int actual;

            for (int i = 0; i < times.Length; i++)
            {
                // Act
                actual = Time.CalculateSecondsInTimespan(times[i]);

                // Assert
                Assert.AreEqual(expectedSeconds[i], actual, $"Failed for time span: {times[i]}");
            }
        }
        [TestMethod]
        public void CalculateSecondsInTimespanTEST1WithComma()
        {
            // Arrange
            string[] times = { "0,5 Uur", "1 Uur", "1,5 Uur", "2 Uur" };
            int[] expectedSeconds = { 1800, 3600, 5400, 7200 };
            int actual;

            for (int i = 0; i < times.Length; i++)
            {
                // Act
                actual = Time.CalculateSecondsInTimespan(times[i]);

                // Assert
                Assert.AreEqual(expectedSeconds[i], actual, $"Failed for time span: {times[i]}");
            }
        }
        [TestMethod]
        public void CalculateEndTimeTEST()
        {
            // Arrange
            DateTime startDateTime = DateTime.Parse("27-10-2023 09:00:00");
            string[] timeSpans = { "0,5 Uur", "1 Uur", "1,5 Uur", "2 Uur" };
            DateTime[] expectedEndTimes = {
                DateTime.Parse("27-10-2023 09:30:00"),
                DateTime.Parse("27-10-2023 10:00:00"),
                DateTime.Parse("27-10-2023 10:30:00"),
                DateTime.Parse("27-10-2023 11:00:00")
            };
            DateTime actual;

            for (int i = 0; i < timeSpans.Length; i++)
            {
                // Act
                actual = Time.CalculateEndTime(startDateTime, timeSpans[i]);

                // Assert
                Assert.AreEqual(expectedEndTimes[i], actual, $"Failed for time span: {timeSpans[i]}");
            }
        }
    }
}