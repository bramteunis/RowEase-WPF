using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserReservations
    {
        public UserReservations(List<Reservation> data) {
            this.Reservations = data;
        }

        public IList<Reservation> Reservations { get; set; }
    }
}
