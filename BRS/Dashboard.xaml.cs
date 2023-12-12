using Controllers;
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
using Models;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Collections;
using System.Diagnostics.Metrics;

namespace Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private RotateTransform rotateTransform = new RotateTransform();
        private RotateTransform rotateTransform2 = new RotateTransform();
        private RotateTransform rotateTransform3 = new RotateTransform();

        public Dashboard()
        {
            InitializeComponent();

            DashboardController dashboardController = new();

            // Creates timers for all the weather advice cards
            dashboardController.CreateTimers(1, WeatherInfoDay1, WeatherCardDay1, rotateTransform, OpacityProperty);
            dashboardController.CreateTimers(2, WeatherInfoDay2, WeatherCardDay2, rotateTransform2, OpacityProperty);
            dashboardController.CreateTimers(3, WeatherInfoDay3, WeatherCardDay3, rotateTransform3, OpacityProperty);

            User user = AuthenticationController.loggedInUser!;

            welcomeUser.Text = "Welkom gebruiker: " + user.firstName + " " + user.lastName;
            if (!user.role!.isAdmin)
            {
                ManageUsers.Visibility = Visibility.Hidden;
                CloseDay.Visibility = Visibility.Hidden;
                ExportData.Visibility = Visibility.Hidden;
            }

            load_WeatherData();

            this.DataContext = new UserReservations(dashboardController.GetReservations(user));
            
        }

        private void OpenMakeReservation_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MakeReservation makeReservation = new();
            makeReservation.Show();
            this.Close();
        }

        private void BecomeMember_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (AuthenticationController.loggedInUser!.roleId == 1)
            {
                UserController userController = new();
                userController.RequestMembership(AuthenticationController.loggedInUser);
                MessageBox.Show("De aanvraag tot lid te worden is verstuurd!");
            }
            else if (AuthenticationController.loggedInUser!.roleId == 5)
            {
                MessageBox.Show("Jouw aanvraag is al verstuurd, wacht op een beheerder voor goedkeuring!");
            }
            else
            {
                MessageBox.Show("Jouw rol laat niet toe om deze actie uit te voeren.");
            }
        }

        private void ManageUsers_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ManageUsersDialog manageUsersDialog = new();
            manageUsersDialog.ShowDialog();
        }

        private void CloseDay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClosingDayDialog closingDayDialog = new();
            closingDayDialog.ShowDialog();
        }

        private void ExportData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ExportDialog exportDialog = new();
            exportDialog.ShowDialog();
        }

        private async Task load_WeatherData()
        {
            DashboardController dashboardController = new();
            TextBlock[] temperaturesArray = { TemperatureWeatherDay1, TemperatureWeatherDay2, TemperatureWeatherDay3 };
            Image[] rainArray = { RainGifDay1, RainGifDay2, RainGifDay3 };
            TextBlock[] windArray = { WindWeatherDay1, WindWeatherDay2, WindWeatherDay3 };
            TextBlock[] humidityArray = { HumidityWeatherDay1, HumidityWeatherDay2, HumidityWeatherDay3 };
            TextBlock[] visibilityArray = { VisionWeatherDay1, VisionWeatherDay2, VisionWeatherDay3 };
            TextBlock[] summaryWeather = { SummaryWeatherDay1, SummaryWeatherDay2, SummaryWeatherDay3 };



            WeatherCard[] weatherCards = await dashboardController.loadWeatherData();

            int counter = 0;
            foreach (WeatherCard forecast in weatherCards)
            {
                temperaturesArray[counter].Text = forecast.Temperature;
                rainArray[counter].Visibility = forecast.rainVisible;
                windArray[counter].Text = forecast.Wind;
                humidityArray[counter].Text = forecast.Humidity;
                visibilityArray[counter].Text = forecast.Visibility;
                summaryWeather[counter].Text = forecast.Summary;

                counter++;
            }
        }
        private void boatListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Reservation clickedReservation = boatListBox.SelectedItem as Reservation;
            MessageBox.Show("Datum: " + clickedReservation?.StartTime + "\n Boot: " + clickedReservation?.Id + "\n EndTime: " + clickedReservation?.EndTime, "Info", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        private void RemoveReservation_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DashboardController dashboardController = new();
            dashboardController.removeReservation(sender);
        }
    }
}
