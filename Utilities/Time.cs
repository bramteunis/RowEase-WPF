using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Time
    {
        public static DateTime CalculateEndTime(DateTime startTime, string timeSpan)
        {
            int totalSeconds = CalculateSecondsInTimespan(timeSpan);

            DateTime endDate = startTime.AddSeconds(totalSeconds);
            return endDate;
        }

        public static int CalculateSecondsInTimespan(string timeSpan)
        {
            int totalSeconds = 0;

            if (timeSpan!.Contains(",5"))
            {
                totalSeconds += 1800;
                totalSeconds += Convert.ToInt32(timeSpan.Split(",")[0]) * 3600;
            }
            else if (timeSpan!.Contains(".5"))
            {
                totalSeconds += 1800;
                totalSeconds += Convert.ToInt32(timeSpan.Split(".")[0]) * 3600;
            }
            else
            {
                totalSeconds += Convert.ToInt32(timeSpan.Split(" Uur")[0]) * 3600;
            }

            return totalSeconds;
        }
    }
}
