using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ExportItem
    {
        public Boat boat { get; set; }
        public List<DateTime> startDates { get; set; }
        public List<DateTime> endDates { get; set; }
        public List<User> users { get; set; }
        public ExportItem(Boat boat, List<DateTime> startDates, List<DateTime> endDates, List<User> users)
        {
            this.boat = boat;
            this.startDates = startDates;
            this.endDates = endDates;
            this.users = users;
        }
    }
}
