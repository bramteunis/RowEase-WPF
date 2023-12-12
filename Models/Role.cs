using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Role
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool isAdmin { get; set; }
        public int daysBefore { get; set; }
        public bool changeBlocked { get; set; }
        public int maxReserve { get; set; }

        public Role(int id, string name, bool isAdmin, int daysBefore, bool changeBlocked, int maxReserve)
        {
            this.id = id;
            this.name = name;
            this.isAdmin = isAdmin;
            this.daysBefore = daysBefore;
            this.changeBlocked = changeBlocked;
            this.maxReserve = maxReserve;
        }
    }
}
