using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Price
    {
        public static string CalculateRentPrice(string timeSpan, string pricePerHour, bool sale)
        {
            double price = Convert.ToDouble(pricePerHour);
            double totalHours = Time.CalculateSecondsInTimespan(timeSpan) / (double)3600;

            //kijken als de user een lid is en dus korting krijgt
            if (sale)
            {
                return string.Format("{0:0.00}", Math.Round(price * totalHours / 100 * 75, 2));
            }
            else
            {
                return string.Format("{0:0.00}", Math.Round(price * totalHours, 2));
            }
        }
    }
}
