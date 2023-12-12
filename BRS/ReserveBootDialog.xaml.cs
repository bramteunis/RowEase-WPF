using Controllers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sunrise;
using Utilities;

namespace Views
{
    /// <summary>
    /// Interaction logic for ReserveBootDialog.xaml
    /// </summary>
    public partial class ReserveBootDialog : Window
    {
        public Boat SelectedBoat { get; set; }
        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }
        private List<Boat> AddedBoatList = new List<Boat>();
        private double TotalPrice = 0;

        public ReserveBootDialog(Boat boat)
        {
            InitializeComponent();

            boatNameWPF.Content = "Bootnaam: " + boat.boatName;
            boatStatusWPF.Content = $"Status: {boat.statusFormat}";
            boatSteerWPF.Content = $"Stuur: {(boat.steer ? "Nodig" : "Niet Nodig")}";
            boatYearWPF.Content = "Bouwjaar: " + boat.buildYear;
            boatCertificateWPF.Content = "Certificaat: " + boat.certificate.name;

            dateBox.FirstDayOfWeek = DayOfWeek.Monday;
            dateBox.DisplayDateStart = DateTime.Today;
            SelectedBoat = boat;
            dateBox.DisplayDateEnd = DateTime.Today.AddDays(AuthenticationController.loggedInUser!.role.daysBefore);

            BoatController boatController = new();
            List<Boat> boats = boatController.GetAllBoats();
            foreach(Boat boatItem in boats)
            {
                addBoatsCombobox.Items.Add(boatItem.boatName);
            }
        }

        private void DateBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> lightHours = SunriseActions.GetAllSunHoursInADay((DateTime)dateBox.SelectedDate!);
            timeBox.ItemsSource = lightHours;

            durationBox.Items.Clear();
            for (double Hours = 0.5; Hours <= 2; Hours += 0.5)
            {
                durationBox.Items.Add(Hours + " Uur");
            }

            reservationButton.IsEnabled = false;
            VisibilityDuration(Visibility.Hidden);
            VisibilityPrice(Visibility.Hidden);
            VisibilityTime(Visibility.Visible);
            this.UpdateLayout();
        }


        private void TimeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            reservationButton.IsEnabled = false;
            VisibilityDuration(Visibility.Visible);
            VisibilityMultiple(Visibility.Hidden);
            this.UpdateLayout();
        }


        private void DurationBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string timeSpan = (string)(durationBox as ComboBox).SelectedItem;
            if (timeSpan != null)
            {
                string reservationDate = dateBox.SelectedDate!.Value.ToString("yyyy-MM-dd") + " " + ((timeBox as ComboBox)?.SelectedItem as string) + ":00";
                DateTime reservationTime = DateTime.ParseExact(reservationDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                reservationButton.IsEnabled = true;
                StartTime = reservationTime;
                EndTime = Time.CalculateEndTime(reservationTime, timeSpan!);
                if (AuthenticationController.loggedInUser!.roleId == 3 || AuthenticationController.loggedInUser.role.isAdmin)
                {
                    VisibilityMultiple(Visibility.Visible);
                }
                else
                {
                    VisibilityPrice(Visibility.Visible);
                    boatPriceActual.Content = "€" + Price.CalculateRentPrice(timeSpan, SelectedBoat.pricePerHour!, (AuthenticationController.loggedInUser!.roleId == 3 || AuthenticationController.loggedInUser!.roleId == 4));
                    TotalPrice = Convert.ToDouble(Price.CalculateRentPrice(timeSpan, SelectedBoat.pricePerHour!, (AuthenticationController.loggedInUser!.roleId == 3 || AuthenticationController.loggedInUser!.roleId == 4)));
                }
            }
        }

        private void AddBoatsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? comboBox = (sender as ComboBox);
            if(comboBox!.SelectedItem != null)
            {
                string? boatName = comboBox.SelectedItem as string;
                if(boatName != null)
                {
                    BoatController boatController = new();
                    Boat? SelectedBoat = boatController.GetAllBoats().Where(x => x.boatName == boatName).FirstOrDefault();
                    if(SelectedBoat != null)
                    {
                        ReservationController reservationController = new();
                        addBoatsCombobox.SelectedItem = null;
                        if (reservationController.IsThereAReservationYet(SelectedBoat, StartTime, EndTime))
                        {
                            MessageBox.Show("Er is al een reservering gepland met deze boot op deze tijd!");
                            return;
                        }
                        AddedBoatList.Add(SelectedBoat);
                        addedBoats.ItemsSource = AddedBoatList.Select(x => x.boatName).Distinct().ToList();
                    }
                }
            }
        }

        private void AddedBoats_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string? SelectedBoatName = addedBoats.SelectedItem as string;
            if(SelectedBoatName != null)
            {
                Boat? boatToRemove = AddedBoatList.Where(x => x.boatName == SelectedBoatName).FirstOrDefault();
                if(boatToRemove != null)
                {
                    AddedBoatList.Remove(boatToRemove);
                    addedBoats.ItemsSource = AddedBoatList.Select(x => x.boatName).Distinct().ToList();
                }
            }
        }

        private void AllBoatcheck_Checked(object sender, RoutedEventArgs e)
        {
            BoatController boatController = new();
            ReservationController reservationController = new();
            List<Boat> boats = boatController.GetAllBoats();
            string failedToAdd = "";
            AddedBoatList.Clear();
            foreach(Boat boat in boats)
            {
                if(!reservationController.IsThereAReservationYet(boat, StartTime, EndTime))
                {
                    AddedBoatList.Add(boat);
                } else
                {
                    failedToAdd += boat.boatName + ",";
                }
            }
            if(failedToAdd.Length > 0)
            {
                MessageBox.Show($"Een of meerdere boten konden niet worden toegevoegd: {failedToAdd} kies een ander datum of tijdstip indien deze boten belangrijk zijn.");
            }
            addedBoats.ItemsSource = AddedBoatList.Select(x => x.boatName).Distinct().ToList();
        }

        private void AllBoatcheck_Unchecked(object sender, RoutedEventArgs e)
        {
            AddedBoatList.Clear();
            addedBoats.ItemsSource = "";
        }

        private void ReservationButton_Click(object sender, RoutedEventArgs e)
        {
            if(AddedBoatList.Count == 0 && (AuthenticationController.loggedInUser!.roleId == 3 || AuthenticationController.loggedInUser.role.isAdmin))
            {
                this.Close();
                return;
            }

            ReservationController reservationController = new();

            if(AuthenticationController.loggedInUser!.roleId != 3 && !AuthenticationController.loggedInUser.role.isAdmin)
            {
                if(reservationController.GetUserReservations(AuthenticationController.loggedInUser).Count + 1 > AuthenticationController.loggedInUser.role.maxReserve)
                {
                    MessageBox.Show("U heeft het maximaal aantal reserveringen bereikt");
                    return;
                }

                if (reservationController.IsThereAReservationYet(SelectedBoat, StartTime, EndTime))
                {
                    MessageBox.Show("Er is al een reservering voor deze boot op deze tijd");
                    return;
                }

                AddedBoatList.Clear();
                AddedBoatList.Add(SelectedBoat);
            }

            reservationController.AddReservation(AuthenticationController.loggedInUser, AddedBoatList, TotalPrice, StartTime, EndTime);

            MessageBox.Show("Reservering succesvol: " + StartTime.ToString("MMM dd yyyy HH:mm") + " - " + EndTime.ToString("MMM dd yyyy HH:mm"));
            this.Close();
        }

        private void VisibilityDuration(Visibility visibility)
        {
            //Zet de tijdsboxen op visible, hidden of collapsed
            durationBox.Visibility = visibility;
            durationLabel.Visibility = visibility;
            durationBox.SelectedItem = null;
        }

        private void VisibilityMultiple(Visibility visibility)
        {
            //Zet de optie om meerdere boten te reserveren op visible, hidden of collapsed
            addBoatsCombobox.Visibility = visibility;
            addBoatsLabel.Visibility = visibility;
            allBoatcheck.Visibility = visibility;
            addedBoats.Visibility = visibility;
            addedBoatLabel.Visibility = visibility;
            addedBoatBackground.Visibility = visibility;
        }

        private void VisibilityTime(Visibility visibility)
        {
            //Zet de tijdsboxen op visible, hidden of collapsed
            timeBox.Visibility = visibility;
            timeLabel.Visibility = visibility;
            timeBox.SelectedItem = null;
        }

        private void VisibilityPrice(Visibility visibility)
        {
            //Zet de prijsboxen op visible, hidden of collapsed
            boatPriceActual.Visibility = visibility;
            boatPriceLabel.Visibility = visibility;
        }
    }
}
