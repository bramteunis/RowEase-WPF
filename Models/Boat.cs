using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Models
{
    public class Boat
    {
        public int id { get; set; }
        public string boatName { get; private set; }
        public int amountOfRowers { get; private set; }
        public string boatType { get; private set; }
        public int buildYear { get; set; }
        public bool status { get; private set; }
        public Certificate certificate { get; set; }
        public bool steer { get; private set; }
        public string boatImage { get; set; }
        public string statusFormat { get; set; }
        public string? maintenanceWidth { get; set; }
        public BitmapImage? maintenanceImage { get; set; }
        public string? pricePerHour { get; set; }

        public Boat(int id, string boatName, int amountOfRowers, string boatType, string buildYear, bool status, Certificate certificate, bool steer, string pricePerHour)
        {
            this.id = id;
            this.boatName = boatName;
            this.amountOfRowers = amountOfRowers;
            this.boatType = boatType;
            this.buildYear = Convert.ToInt32(buildYear);
            this.status = status;
            this.certificate = certificate;
            this.steer = steer;
            statusFormat = status ? "In onderhoud" : "Beschikbaar";
            maintenanceImage = status ? new BitmapImage(new Uri("Resources/switch-off.png", UriKind.Relative)) : new BitmapImage(new Uri("Resources/switch-on.png", UriKind.Relative));
            switch (boatType)
            {
                case "1x":
                    boatImage = "Resources/Onex-Icon.png"; break;
                case "C2x" or "2x":
                    boatImage = "Resources/Twox-Icon.png"; break;
                case "2+":
                    boatImage = "Resources/Two+-Icon.png"; break;
                case "4+" or "C4+":
                    boatImage = "Resources/Four+Icon.png"; break;
                case "4x":
                    boatImage = "Resources/FourxIcon.png"; break;
                case "8+":
                    boatImage = "Resources/EightIcon.png"; break;
                default:
                    boatImage = "Resources/Rower.png"; break;
            }
            this.pricePerHour = pricePerHour;
            maintenanceWidth = "0";
        }
    }
}
