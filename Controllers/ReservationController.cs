using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using Models;
using Utilities;
using Mail;

namespace Controllers
{
    public class ReservationController
    {
        public bool IsThereAReservationYet(Boat boat, DateTime start, DateTime end)
        {
            ReservationSQL reservationSQL = new();
            List<Reservation> reservations = reservationSQL.GetAllReservations(null);
            foreach(Reservation reservation in reservations)
            {
                if (reservation.Boats.Any(x => x.boatName == boat.boatName)) { 
                    if(end.Ticks >= reservation.StartTime.Ticks && end.Ticks <= reservation.EndTime.Ticks)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<Reservation> GetUserReservations(User user)
        {
            ReservationSQL reservationSQL = new();
            List<Reservation> reservations = reservationSQL.GetAllReservations(user);
            return reservations;
        }
        public List<Reservation> GetAllReservations()
        {
            ReservationSQL reservationSQL = new();
            List<Reservation> reservations = reservationSQL.GetAllReservations(null);
            return reservations;
        }


        public List<Reservation> GetAllReservationsInDate(DateTime date)
        {
            ReservationSQL reservationSQL = new();
            List<Reservation> reservations = reservationSQL.GetAllReservations(null);
            List<Reservation> inDate = new();
            DateTime nextDay = date.AddDays(1);
            if(reservations.Count > 0)
            {
                foreach(Reservation reservation in reservations)
                {
                    if (date.Ticks <= reservation.StartTime.Ticks && nextDay.Ticks >= reservation.EndTime.Ticks)
                    {
                        inDate.Add(reservation);
                    }
                }
            }
            return inDate;
        }

        public void AddReservation(User user, List<Boat> boats, double price, DateTime start, DateTime end)
        {
            ReservationSQL reservationSQL = new();
            int reservationId = reservationSQL.AddReservationAndGetId(user, price, start, end);
            foreach (Boat boat in boats)
            {
                reservationSQL.AddReservationLine(reservationId, boat.id);
            }
        }

        public List<DateTime> GetBlackedOutDays()
        {
            ReservationSQL reservationSQL = new();
            List<DateTime> dateTimes = reservationSQL.GetBlackedOutDays();
            return dateTimes;
        }

        public void BlackoutNewDateTime(DateTime date)
        {
            ReservationSQL reservationSQL = new();
            reservationSQL.AddBlackoutDateTime(date);
            List <Reservation> cancelledReservations = GetAllReservationsInDate(date);
            foreach (Reservation reservation in cancelledReservations)
            {
                SendCancellationMail(reservation);
                RemoveReservation(reservation.Id);
            }
        }

        public void SendCancellationMail(Reservation reservation)
        {
            MailAction mailAction = new();
            UserSQL userSQL = new();
            User? user = userSQL.GetUserById(reservation.MemberId);
            if (user != null)
            {
                mailAction.SendMail(user.email!, "Reservering geannuleerd", $"Beste {user.firstName} {user.lastName},\n\nUw reservering voor {reservation.StartTime} tot {reservation.EndTime} is geannuleerd.\n\nMet vriendelijke groet,\n\nRBS");
            }
        }

        public void RemoveReservation(int reservationId)
        {
            ReservationSQL reservationSQL = new();
            reservationSQL.RemoveReservationLine(reservationId);
            reservationSQL.RemoveReservation(reservationId);
        }
    }
}
