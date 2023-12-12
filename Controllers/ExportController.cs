using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Controllers;
using Utilities;
using Database;
using PdfSharp;
using PdfSharp.Pdf;

namespace Controllers
{
    public class ExportController
    { 
        public List<ExportItem> PrepairExportItems(DateTime? start, DateTime end, List<Boat> boats, bool showReservations, string saveName, bool openAfterSave)
        {
            ReservationSQL reservationSQL = new();
            UserSQL userSQL = new();

            List<ExportItem> exportItems = new();

            if (boats.Count > 0)
            {
                foreach(Boat boat in boats)
                {
                    List<DateTime> reservationDateStart = new();
                    List<DateTime> reservationDateEnd = new();
                    List<User> reservationUsers = new();
                    if (showReservations)
                    {
                        List<Reservation> reservationsWithBoat = reservationSQL.GetAllBoatReservations(boat);
                        foreach(Reservation reservation in reservationsWithBoat)
                        {
                            if ((start != null && start < reservation.StartTime && end > reservation.EndTime) || start == null)
                            {
                                User? user = userSQL.GetUserById(reservation.MemberId);
                                if (user != null)
                                {
                                    reservationDateStart.Add(reservation.StartTime);
                                    reservationUsers.Add(user);
                                    reservationDateEnd.Add(reservation.EndTime);
                                }
                            }
                        }
                    }
                    exportItems.Add(new ExportItem(boat, reservationDateStart, reservationDateEnd, reservationUsers));
                }
            }

            return exportItems;
        }

        public void CreateAndSavePDF(DateTime? start, DateTime end, List<Boat> boats, bool showReservations, string saveName, List<ExportItem> exportItems, bool openAfterSave)
        {
            PdfDocument document = PDF.CreateExportDocument(start, end, boats, saveName, showReservations, exportItems);
            PDF.SavePDF(document, saveName, openAfterSave);
        }
    }
}
