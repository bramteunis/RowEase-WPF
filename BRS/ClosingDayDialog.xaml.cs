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

namespace Views
{
    /// <summary>
    /// Interaction logic for ClosingDayDialog.xaml
    /// </summary>
    public partial class ClosingDayDialog : Window
    {
        private DateTime startingDate = DateTime.Now;
        private DateTime endingDate;

        public ClosingDayDialog()
        {
            InitializeComponent();

            dateBox.FirstDayOfWeek = DayOfWeek.Monday;
            dateBox.DisplayDateStart = DateTime.Today;
            dateBoxEnd.FirstDayOfWeek = DayOfWeek.Monday;
            dateBoxEnd.DisplayDateStart = startingDate;
            dateBox.DisplayDateEnd = DateTime.Today.AddDays(365);

            ReservationController reservationController = new();
            List<DateTime> blackoutDates = reservationController.GetBlackedOutDays();
            foreach (DateTime blackoutDate in blackoutDates)
            {
                dateBox.BlackoutDates.Add(new CalendarDateRange(blackoutDate));
                dateBoxEnd.BlackoutDates.Add(new CalendarDateRange(blackoutDate));
            }
        }

        private void BackImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void DateBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            confirmationCheckBox.Visibility = Visibility.Hidden;
            closeDayButton.IsEnabled = false;
            if (sender is DatePicker)
            {
                DatePicker datePicker = (DatePicker)sender;
                DateTime? date = datePicker.SelectedDate;
                if(date != null)
                {
                    startingDate = (DateTime)date;
                    endDateLabel.Visibility = Visibility.Visible;
                    dateBoxEnd.Visibility = Visibility.Visible;
                    endingDate = DateTime.Now;
                }
            }
        }

        private void DateBoxEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker)
            {
                DatePicker datePicker = (DatePicker)sender;
                DateTime? date = datePicker.SelectedDate;
                if (date != null)
                {
                    if ((DateTime)date < startingDate)
                    {
                        MessageBox.Show("De einddatum kan niet voor de startdatum liggen.");
                        datePicker.SelectedDate = null;
                    }
                    else
                    {
                        confirmationCheckBox.Visibility = Visibility.Visible;
                        closeDayButton.IsEnabled = true;
                        endingDate = ((DateTime)date);
                    }
                }
            }
        }

        private void CloseDayButton_Click(object sender, RoutedEventArgs e)
        {
            if(confirmationCheckBox.IsChecked == true)
            {
                ReservationController reservationController = new();
                for (var dt = startingDate; dt <= endingDate; dt = dt.AddDays(1))
                {
                    string date = dt.ToString("yyyy-MM-dd");
                    reservationController.BlackoutNewDateTime(dt);
                }
                MessageBox.Show("Alle reserveringen vanaf " + startingDate.Date.ToString("yyyy-MM-dd") + " tot " + endingDate.Date.ToString("yyyy-MM-dd") + " zijn geannuleerd.");
                this.Close();
            } else
            {
                MessageBox.Show("Vink het vakje aan om door te gaan.");
            }
        }
    }
}
