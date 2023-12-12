using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;

namespace Utilities
{
    public class PDF
    {
        public static PdfDocument CreateExportDocument(DateTime? startTime, DateTime endTime, List<Boat> boats, string documentName, bool showReservations, List<ExportItem> exportItems)
        {
            PdfDocument document = new PdfDocument();
            XFont font20 = new XFont("Verdana", 20, XFontStyle.Bold);
            XFont font16 = new XFont("Verdana", 16, XFontStyle.Bold);
            XFont font14 = new XFont("Verdana", 14, XFontStyle.Bold);
            XFont font13 = new XFont("Verdana", 13, XFontStyle.Regular);
            XFont font11 = new XFont("Verdana", 11, XFontStyle.Regular);

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            gfx.DrawString("RBS Exportatie", font20, XBrushes.Black,
            new XRect(50, 50, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Wat is het doel van dit document?", font16, XBrushes.Black,
            new XRect(50, 100, page.Width, page.Height), XStringFormats.TopLeft);
            XTextFormatter tf = new XTextFormatter(gfx);
            tf.DrawString("RBS is een reserveringsapplicatie voor roeiverenigingen. De informatie welke in dit systeem te vinden zijn kan geexporteerd worden. Dit document is er als resultaat van de exportatie. Bij elke exportering kunnen er verschillende opties worden aangegeven om de geëxporteerde data zo goed mogelijk weer te geven. De huidige opties welke voor deze exportatie actief zijn, zijn hieronder te vinden.", font11, XBrushes.Black,
            new XRect(50, 130, page.Width - 100, page.Height), XStringFormats.TopLeft);

            gfx.DrawString("Exportatie Informatie", font16, XBrushes.Black,
            new XRect(50, 220, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Exportatie datum: ", font14, XBrushes.Black,
            new XRect(50, 260, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), font13, XBrushes.Black,
            new XRect(250, 261, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Exportatie naam: ", font14, XBrushes.Black,
            new XRect(50, 285, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(documentName, font13, XBrushes.Black,
            new XRect(250, 286, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Toon reserveringen: ", font14, XBrushes.Black,
            new XRect(50, 310, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(showReservations ? "Ja" : "Nee", font13, XBrushes.Black,
            new XRect(250, 311, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Aantal resultaten: ", font14, XBrushes.Black,
            new XRect(50, 335, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(boats.Count.ToString(), font13, XBrushes.Black,
            new XRect(250, 336, page.Width, page.Height), XStringFormats.TopLeft);
            if (startTime != null)
            {
                gfx.DrawString("Export Periode: ", font14, XBrushes.Black,
                new XRect(50, 360, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString($"{startTime?.ToString("dd/MM/yyyy")} - {endTime.ToString("dd/MM/yyyy")}", font13, XBrushes.Black,
                new XRect(250, 361, page.Width, page.Height), XStringFormats.TopLeft);
            }
            foreach (ExportItem export in exportItems)
            {
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);

                gfx.DrawString(export.boat.boatName, font20, XBrushes.Black,
                new XRect(50, 50, page.Width, page.Height), XStringFormats.TopLeft);
                XImage xImage = XImage.FromFile(export.boat.boatImage);
                gfx.DrawImage(xImage, new XRect(page.Width - 150, 25, 100, 100));
                gfx.DrawString("Boot informatie", font16, XBrushes.Black,
                new XRect(50, 200, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString("Boot type: ", font14, XBrushes.Black,
                new XRect(50, 240, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(export.boat.boatType, font13, XBrushes.Black,
                new XRect(250, 241, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString("Aantal Roeiers: ", font14, XBrushes.Black,
                new XRect(50, 265, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(export.boat.amountOfRowers.ToString(), font13, XBrushes.Black,
                new XRect(250, 266, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString("Bouwjaar: ", font14, XBrushes.Black,
                new XRect(50, 290, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(export.boat.buildYear.ToString(), font13, XBrushes.Black,
                new XRect(250, 291, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString("Status: ", font14, XBrushes.Black,
                new XRect(50, 315, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(export.boat.statusFormat, font13, XBrushes.Black,
                new XRect(250, 316, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString("Certificaat: ", font14, XBrushes.Black,
                new XRect(50, 340, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(export.boat.certificate.name, font13, XBrushes.Black,
                new XRect(250, 341, page.Width, page.Height), XStringFormats.TopLeft);
                if (showReservations)
                {
                    gfx.DrawString("Reserveringen", font16, XBrushes.Black,
                    new XRect(50, 400, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("Startdatum", font14, XBrushes.Black,
                    new XRect(50, 425, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("Einddatum", font14, XBrushes.Black,
                    new XRect(250, 425, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("Naam", font14, XBrushes.Black,
                    new XRect(450, 425, page.Width, page.Height), XStringFormats.TopLeft);

                    for (int i = 0; i < export.users.Count; i++)
                    {
                        gfx.DrawString(export.startDates[i].ToString(), font13, XBrushes.Black,
                        new XRect(50, 450 + (20 * i), page.Width, page.Height), XStringFormats.TopLeft);
                        gfx.DrawString(export.endDates[i].ToString(), font13, XBrushes.Black,
                        new XRect(250, 450 + (20 * i), page.Width, page.Height), XStringFormats.TopLeft);
                        gfx.DrawString(export.users[i].firstName!.ToString() + " " + export.users[i].lastName!.ToString(), font13, XBrushes.Black,
                        new XRect(450, 450 + (20 * i), page.Width, page.Height), XStringFormats.TopLeft);
                    }
                }
            }
            return document;
        }

        public static void SavePDF(PdfDocument document, string saveName, bool openAfterSave)
        {
            document.Save(saveName + ".pdf");
            if(openAfterSave)
            {
                Process.Start("explorer", saveName + ".pdf");
            }
        }
    }
}
