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
using System.Windows.Input;
using System.Globalization;

namespace Database
{
    public class ReservationSQL
    {
        private readonly ExecuteSQL _connection;

        public ReservationSQL()
        {
            _connection = new ExecuteSQL();
        }

        public List<Reservation> GetAllReservations(User? user)
        {
            SqlCommand command;
            if (user == null)
            {
                command = new("SELECT * FROM reservation;");
            } else
            {
                command = new("SELECT * FROM reservation WHERE UserId = @UserId;");
                command.Parameters.AddWithValue("@UserId", user.id);
            }
            using SqlDataReader reader = _connection.ExecuteStatement(command);
            List<Reservation> reservations = new List<Reservation>();
            while (reader.Read())
            {
                List<Boat> boatReservations = GetBoatListByReservation(reader.GetInt32(0));
                Reservation reservation = new(reader.GetInt32(0), reader.GetInt32(1), reader.GetDouble(2), boatReservations, reader.GetDateTime(3), reader.GetDateTime(4));
                reservations.Add(reservation);
            }
            return reservations;
        }

        private List<Boat> GetBoatListByReservation(int reservationId)
        {
            SqlCommand command = new("SELECT * FROM reservationline WHERE ResId = @ResId;");
            command.Parameters.AddWithValue("@ResId", reservationId);
            BoatSQL boatSQL = new();
            List<Boat> boatList = boatSQL.GetAllAvailableBoats(null);
            List<Boat> boats = new();
            using SqlDataReader reader = _connection.ExecuteStatement(command);
            while (reader.Read())
            {
                foreach(Boat boat in boatList)
                {
                    if (boat.id == reader.GetInt32(2))
                    {
                        boats.Add(boat);
                    }
                }
            }
            return boats;
        }

        public List<Reservation> GetAllBoatReservations(Boat boat)
        {
            SqlCommand command = new("SELECT * FROM reservation WHERE Id IN (SELECT ResId FROM reservationline WHERE BoatId = @BoatId);");
            command.Parameters.AddWithValue("@BoatId", boat.id);
            using SqlDataReader reader = _connection.ExecuteStatement(command);
            List<Reservation> reservations = new List<Reservation>();
            while (reader.Read())
            {
                List<Boat> boatReservations = GetBoatListByReservation(reader.GetInt32(0));
                Reservation reservation = new(reader.GetInt32(0), reader.GetInt32(1), reader.GetDouble(2), boatReservations, reader.GetDateTime(3), reader.GetDateTime(4));
                reservations.Add(reservation);
            }
            return reservations;
        }

        public int AddReservationAndGetId(User user, double price, DateTime start, DateTime end)
        {
            SqlCommand command = new SqlCommand("INSERT INTO reservation (UserId, Price, StartDate, EndDate) VALUES (@UserId, @Price, @StartDate, @EndDate); SELECT SCOPE_IDENTITY();");
            command.Parameters.AddWithValue("@UserId", user.id);
            command.Parameters.AddWithValue("@Price", price);
            command.Parameters.AddWithValue("@StartDate", start);
            command.Parameters.AddWithValue("@EndDate", end);
            using SqlDataReader reader = _connection.ExecuteStatement(command);
            if(reader.Read())
            {
                return Convert.ToInt32(reader[0]);
            }
            return 0;
        }

        public void AddReservationLine(int reservationId, int boatId)
        {
            SqlCommand command = new SqlCommand("INSERT INTO reservationline (ResId, BoatId) VALUES (@ResId, @BoatId);");
            command.Parameters.AddWithValue("@ResId", reservationId);
            command.Parameters.AddWithValue("@BoatId", boatId);
            _connection.ExecuteStatement(command);
        }

        public List<DateTime> GetBlackedOutDays()
        {
            List<DateTime> blackoutDates = new();
            SqlCommand command = new("SELECT * FROM blackout");
            using SqlDataReader reader = _connection.ExecuteStatement(command);
            while(reader.Read())
            {
                blackoutDates.Add(reader.GetDateTime(0));
            }
            return blackoutDates;
        }

        public void AddBlackoutDateTime(DateTime date)
        {
            if(!GetBlackedOutDays().Contains(date.Date))
            {
                SqlCommand command = new("INSERT INTO blackout (Date) VALUES (@Date)");
                command.Parameters.AddWithValue("@Date", date);
                using SqlDataReader reader = _connection.ExecuteStatement(command);
            }
        }

        public void RemoveReservationLine(int reservationId)
        {
            SqlCommand command = new SqlCommand("DELETE FROM reservationline WHERE ResId = @ResId");
            command.Parameters.AddWithValue("@ResId", reservationId);
            _connection.ExecuteStatement(command);
        }

        public void RemoveReservation(int reservationId)
        {
            SqlCommand command = new SqlCommand("DELETE FROM reservation WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", reservationId);
            _connection.ExecuteStatement(command);
        }
    }
}
