using Innovative.SolarCalculator;

namespace Sunrise
{
    public class SunriseActions
    {
        public static List<string> GetAllSunHoursInADay(DateTime day)
        {
            SolarTimes sunTimes = new SolarTimes(day, 52.4, 6.0);
            List<string> hoursList = new List<string>();
            DateTime dateSunrise = Convert.ToDateTime(sunTimes.Sunrise);
            DateTime dateSunset = Convert.ToDateTime(sunTimes.Sunset);
            var hours = (dateSunset - dateSunrise).TotalHours - 1;
            DateTime RoundUp(DateTime dt, TimeSpan d)
            {
                return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
            }
            for (int i = 0; i < hours * 2; i++)
            {
                DateTime date = (DateTime)dateSunrise.AddMinutes(i * 30);
                if (date > DateTime.Now)
                {
                    var rounded = RoundUp(date, TimeSpan.FromMinutes(15));
                    hoursList.Add(rounded.ToString("HH:mm"));
                }
            }
            return hoursList;
        }
    }
}