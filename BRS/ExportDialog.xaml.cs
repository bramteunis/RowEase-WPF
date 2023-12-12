using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using Controllers;
using Utilities;

namespace Views
{
    /// <summary>
    /// Interaction logic for ExportDialog.xaml
    /// </summary>
    public partial class ExportDialog : Window
    {
        private BitmapImage toggleOff = new BitmapImage(new Uri("Resources/switch-off.png", UriKind.Relative));
        private BitmapImage toggleOn = new BitmapImage(new Uri("Resources/switch-on.png", UriKind.Relative));

        private List<Boat> exportList;
        private string sortingOption = "Geen sortering";
        private DateTime? selectedStartDate;
        private DateTime selectedEndDate = DateTime.Now;

        public ExportDialog()
        {

            InitializeComponent();
            NameInput.Text = DateTime.Now.ToShortDateString();
            BoatController boatController = new();
            exportList = boatController.GetAllBoats();
            UpdateView();
        }

        private void ToggleSwitch(object? sender, RoutedEventArgs e)
        {
            //Wanneer er op de toggle image wordt gedrukt
            if (sender is Image)
            {
                //Verander de foto en tag van de toggle en update deze informatie in het overzicht (sidebar)
                Image image = (Image)sender;
                if (image.Tag.ToString() == "0") { image.Source = toggleOn; image.Tag = "1"; UpdateView(); return; };
                if (image.Tag.ToString() == "1") { image.Source = toggleOff; image.Tag = "0"; UpdateView(); return; };
            }
        }

        //Methode om de sortering daadwerkelijk uit te voeren
        private void UpdateSorting(string option)
        {
            //Verander de sortering op basis van het geselecteerde element
            switch (option)
            {
                case "Naam":
                    exportList = exportList.OrderBy(boat => boat.boatName).ToList();
                    sortingOption = "Naam";
                    break;
                case "Bouwjaar":
                    exportList = exportList.OrderBy(boat => boat.buildYear).ToList();
                    sortingOption = "Bouwjaar";
                    break;
                case "Roeiers":
                    exportList = exportList.OrderBy(boat => boat.amountOfRowers).ToList();
                    sortingOption = "Roeiers";
                    break;
                case "Certificaat":
                    exportList = exportList.OrderBy(boat => boat.certificate).ToList();
                    sortingOption = "Certificaat";
                    break;
                default:
                    exportList = exportList.OrderBy(boat => boat.id).ToList();
                    sortingOption = "Geen sortering";
                    break;
            }
        }

        //Methode om de sortering combobox op te vaangen
        private void ChangeSorting(object? sender, RoutedEventArgs e)
        {
            if (sender is ComboBox)
            {
                //Krijg de text van het gelecteerde element
                ComboBox comboBox = (ComboBox)sender;
                ComboBoxItem? selectedItem = (comboBox.SelectedItem as ComboBoxItem);
                string? itemText = selectedItem!.Content as string;
                if (itemText != null)
                {
                    //Verander de sortering
                    UpdateSorting(itemText);
                }
            }
            //Update dee weergave (sidebar)
            UpdateView();
        }

        //Methode om de eerste datepicker op te vangen
        private void ChangedStartDate(object? sender, RoutedEventArgs e)
        {
            if (sender is DatePicker)
            {
                //Krijg de datum
                DatePicker datePicker = (DatePicker)sender;
                DateTime? date = datePicker.SelectedDate;
                if (date != null)
                {
                    //Laat de optie zien om een 2e periode in te vullen en sla de startdatum op
                    selectedStartDate = (DateTime)date;
                    endDatePickerXAML.Visibility = Visibility.Visible;
                    UpdateView();
                }
            }
        }

        //Methode om de tweede datepicker op te vangen
        private void ChangedEndDate(object? sender, RoutedEventArgs e)
        {
            if (sender is DatePicker)
            {
                //Krijg de datum
                DatePicker datePicker = (DatePicker)sender;
                DateTime? date = datePicker.SelectedDate;
                if (date != null)
                {
                    //Kijk of de datum voor de startdatum ligt
                    if ((DateTime)date < selectedStartDate)
                    {
                        MessageBox.Show("De einddatum kan niet voor de startdatum liggen.");
                        datePicker.SelectedDate = null;
                    }
                    else
                    {
                        //Sla de einddatum op
                        selectedEndDate = ((DateTime)date);
                    }
                    UpdateView();
                }
            }
        }

        //Methode om de checkbox op te vangen
        private void CheckBoxChecked(object sender, RoutedEventArgs e)
        {
            //zet de sender als checkBox voor gebruik datacontext van de class
            CheckBox? checkBox = sender as CheckBox;
            Boat? checkBoxData = checkBox!.DataContext as Boat;
            if (checkBoxData != null)
            {
                //als checkbox gechecked is voeg toe aan certificates lijst en anders verwijder uit lijst
                if ((bool)checkBox!.IsChecked!)
                {
                    exportList.Add(checkBoxData);
                }
                else if (!(bool)checkBox.IsChecked)
                {
                    exportList.Remove(checkBoxData);
                }
            }

            UpdateView();
        }

        //Wanneer er een item geselecteerd wordt in plaats van de checkbox
        private void UnSelector(object sender, RoutedEventArgs e)
        {
            //Annuleer de actie (selecteer niks)
            selectBoatXAML.SelectedIndex = -1;
        }

        //Methode om de huidige aantal boten te krijgen
        private string getBoatCount()
        {
            return exportList.Count.ToString();
        }

        //Methode om de informatie te updaten
        private void UpdateView()
        {
            BoatController boatController = new BoatController();
            //Verander de waardes in de WPF view
            exportationViewRange.Content = $"Exportatie Periode:";
            if (differentPeriodToggleXAML.Tag.ToString() == "1")
            {
                startDatePickerXAML.Visibility = Visibility.Visible;
                if (selectedStartDate == null)
                {
                    exportationViewRangeValue.Content = $"01-01-1970 - {selectedEndDate.ToShortDateString()}";
                }
                else
                {
                    exportationViewRangeValue.Content = $"{((DateTime)selectedStartDate).ToShortDateString()} - {selectedEndDate.ToShortDateString()}";
                }
            }
            else
            {
                startDatePickerXAML.Visibility = Visibility.Hidden;
                endDatePickerXAML.Visibility = Visibility.Hidden;
                exportationViewRangeValue.Content = $"01-01-1970 - {selectedEndDate.ToShortDateString()}";
            }
            exportationViewReservations.Content = showReservationsToggleXAML.Tag.ToString() == "1" ? "Tonen Reserveringen: Ja" : "Tonen Reserveringen: Nee";
            if (showAllBoatsToggleXAML.Tag.ToString() == "1")
            {
                if (selectBoatXAML.Visibility == Visibility.Visible)
                {
                    exportList = boatController.GetAllBoats();
                }
                exportationViewBoats.Content = $"Aantal Boten: {getBoatCount()}";
                selectBoatXAML.SelectedItem = null;
                selectBoatXAML.Visibility = Visibility.Hidden;
            }
            else
            {
                if (selectBoatXAML.Visibility == Visibility.Hidden)
                {
                    exportList.Clear();
                    selectBoatXAML.ItemsSource = boatController.GetAllBoats();
                }
                exportationViewBoats.Content = $"Aantal Boten: {exportList.Count}";
                selectBoatXAML.Visibility = Visibility.Visible;
            }
            exportationViewSorting.Content = $"Gesorteerd op: {sortingOption}";
            exportationViewOpen.Content = openAfterToggleXAML.Tag.ToString() == "1" ? "Openen na exportatie: Ja" : "Openen na exportatie: Nee";
        }

        //Methode om de exporteer knop op te vangen
        private void ExportClick(object sender, RoutedEventArgs e)
        {
            //Kijk of de sender een knop is
            if (sender is Button)
            {
                //Kijk of de toggle van de periode aan staat
                if (differentPeriodToggleXAML.Tag.ToString() == "1")
                {
                    //Kijk of er geen startdatum is
                    if (selectedStartDate == null)
                    {
                        MessageBox.Show("Selecteer een startdatum.");
                        return;
                        //Kijk of de einddatum is eerder dan de startdatum
                    }
                    else if (selectedEndDate < selectedStartDate)
                    {
                        MessageBox.Show("De einddatum moet later dan de startdatum zijn.");
                        return;
                    }
                }
                //Kijk of er boten zijn geselecteerd/gevonden
                if (exportList.Count == 0)
                {
                    MessageBox.Show("Er zijn geen boten om te exporteren.");
                    return;
                }

                //Update de sortering
                UpdateSorting(sortingOption);

                //Kijk of de naam voldoet aan de regex (letters, cijfers, -)
                if(!Validate.ValidCharacters(NameInput.Text)) { 
                    MessageBox.Show("Ongeldige naam, de naam mag enkel letters,cijfers,- bevatten");
                    return;
                }

                //Exporteer naar PDF
                ExportController exportController = new();
                List<ExportItem> exportListPDF = exportController.PrepairExportItems(selectedStartDate, selectedEndDate, exportList, showReservationsToggleXAML.Tag.ToString() == "1", NameInput.Text.ToLower(), openAfterToggleXAML.Tag.ToString() == "1");
                exportController.CreateAndSavePDF(selectedStartDate, selectedEndDate, exportList, showReservationsToggleXAML.Tag.ToString() == "1", NameInput.Text.ToLower(), exportListPDF, openAfterToggleXAML.Tag.ToString() == "1");
                //Als het bestand niet direct wordt geopend, laat een bericht zien met de locatie van het bestand
                if (openAfterToggleXAML.Tag.ToString() == "0")
                {
                    MessageBox.Show("Het bestand is geexporteerd naar: " + Directory.GetCurrentDirectory() + "/" + NameInput.Text.ToLower() + ".pdf");
                }
                this.Hide();
            }
        }
    }
}
