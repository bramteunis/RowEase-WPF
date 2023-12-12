using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Database;

namespace Controllers
{
    public class BoatController
    {
        public List<Boat> GetAllBoats()
        {
            BoatSQL boatSQL = new();
            return boatSQL.GetAllAvailableBoats(AuthenticationController.loggedInUser!);
        }

        public void ToggleBoatStatus(Boat boat)
        {
            BoatSQL boatSQL = new();
            boatSQL.ChangeBoatStatus(boat.id, Convert.ToInt32(!boat.status));
        }
    }
}
