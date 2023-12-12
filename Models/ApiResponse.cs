using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ApiResponse
    {
        public int queryCost { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string resolvedAddress { get; set; }
        public string address { get; set; }
        public string timezone { get; set; }
        public double tzoffset { get; set; }
        public string description { get; set; }
        public List<WeatherDay> days { get; set; }
        public List<WeatherAlert> alerts { get; set; }
        public Dictionary<string, WeatherStation> stations { get; set; }
        public WeatherConditions currentConditions { get; set; }
    }
}
