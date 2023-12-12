using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Models;
using System.Windows.Media.Imaging;

namespace Database
{
    public class BoatSQL
    {
        private readonly ExecuteSQL _connection;

        public BoatSQL()
        {
            _connection = new ExecuteSQL();
        }

        public List<Boat> GetAllAvailableBoats(User? user)
        {
            UserSQL userSQL = new();

            List<Boat> boats = new();

            SqlCommand command = new("SELECT * FROM boats");
            using SqlDataReader reader = _connection.ExecuteStatement(command);

            while (reader.Read())
            {
                Boat boat = new Boat(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), Convert.ToBoolean(reader.GetInt16(5)), userSQL.GetCertificateById(reader.GetInt32(6)), Convert.ToBoolean(reader.GetInt16(7)), reader.GetString(8));

                if (user != null)
                {
                    if (user.roleId == 1 || user.roleId == 2 || user.roleId == 5)
                    {
                        if (boat.certificate.id > user.certificate.id)
                        {
                            continue;
                        }
                    }
                    if (boat.status && !user.role.changeBlocked)
                    {
                        continue;
                    }
                    if (user.role.changeBlocked)
                    {
                        boat.maintenanceWidth = "50";
                    }
                }

                boats.Add(boat);
            }

            return boats;
        }

        public void ChangeBoatStatus(int boatId, int status)
        {
            SqlCommand command = new("UPDATE boats SET Blocked = @Status WHERE Id = @BoatId;");
            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@BoatId", boatId);
            using SqlDataReader reader = _connection.ExecuteStatement(command);
        }
    }
}
