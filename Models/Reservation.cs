using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public double Price { get; set; }
        public string? ReservationImage { get; set; }
        public List<Boat> Boats { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Reservation(int id, int memberId, double price, List<Boat> boats, DateTime startTime, DateTime endTime)
        {
            Id = id;
            MemberId = memberId;
            Price = price;
            Boats = boats;
            StartTime = startTime;
            EndTime = endTime;
            ReservationImage = Boats[0].boatImage;
        }
    }
}
