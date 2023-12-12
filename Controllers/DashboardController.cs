using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Database;
using Forecast;
using Models;

namespace Controllers
{
    public class DashboardController
    {
        private DispatcherTimer timer;
        private DoubleAnimation OpacityAnimationToHidden = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(1)));
        private DoubleAnimation OpacityAnimationToVisible = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(1)));
        private DoubleAnimation doubleAnimation = new DoubleAnimation(360, 0, new Duration(TimeSpan.FromSeconds(1)));

        public async Task<WeatherCard[]> loadWeatherData() { 
            GetForecast getForecast = new();
            int counter = 0;
            var weatherData = await getForecast.LoadWeather();

            WeatherCard[] weatherCards = new WeatherCard[3];
            foreach (WeatherDay forecast in weatherData.days) {
                WeatherCard weatherCard = new();
                
                weatherCard.Temperature = forecast.temp.ToString() + "°C";
                if (forecast.precipprob > 0.5)
                {
                    weatherCard.rainVisible = Visibility.Visible;
                }
                else {
                    weatherCard.rainVisible = Visibility.Hidden;
                }
                weatherCard.Wind = forecast.windspeed.ToString() + " km/h";
                weatherCard.Humidity = forecast.humidity.ToString() + "%";
                weatherCard.Visibility = forecast.visibility.ToString() + " km";
                if (forecast.visibility > 80)
                {
                    weatherCard.Summary = "Goed zicht";
                }
                else if (forecast.visibility > 50)
                {
                    weatherCard.Summary = "Matig zicht";
                }
                else if (forecast.visibility > 20)
                {
                    weatherCard.Summary = "Slecht zicht";
                }
                else
                {
                    weatherCard.Summary = "Zeer slecht zicht";
                }

                weatherCards.Append(weatherCard);

                counter++;
            }

            return weatherCards;
        }

        public void CreateTimers(int number, Border mouseListener, Border weatherAdvice, RotateTransform rotateTransform, DependencyProperty OpacityProperty)
        {
            // Limits a animation to 1 time per hover of a card
            doubleAnimation.RepeatBehavior = new RepeatBehavior(1);
            OpacityAnimationToVisible.RepeatBehavior = new RepeatBehavior(1);
            
            // Create a timer with a 0.3 second interval.
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            mouseListener.MouseEnter += (sender, e) =>
            {
                try { timer.Tick -= timer_Tick; }
                catch { }
                
                timer.Tick += timer_Tick;
                
                timer.Start();
            };
            mouseListener.MouseLeave += (sender, e) =>
            {
                timer.Stop();
                mouseListener.BeginAnimation(OpacityProperty, OpacityAnimationToVisible);
                weatherAdvice.BeginAnimation(OpacityProperty, OpacityAnimationToHidden);
            };
            mouseListener.RenderTransform = rotateTransform;
            mouseListener.RenderTransformOrigin = new Point(0.5, 0.5);

            void timer_Tick(object sender, EventArgs e)
            {
                // Evenhandler used to Rotate weathercard
                timer.Stop();
                rotateTransform.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
                mouseListener.BeginAnimation(OpacityProperty, OpacityAnimationToHidden);
                weatherAdvice.BeginAnimation(OpacityProperty, OpacityAnimationToVisible);
            }
        }

        public List<Reservation> GetReservations(User user) {
            ReservationSQL reservation = new ReservationSQL();

            return reservation.GetAllReservations(user);
        }

        public void removeReservation(object sender) {
            ReservationSQL reservationSQL = new();
            Reservation ReservationToRemove = (Reservation)(sender as Image).DataContext;
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Weet je zeker dat je de reservering van " + ReservationToRemove.StartTime + " wil verwijderen?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                reservationSQL.RemoveReservationLine(ReservationToRemove.Id);
            }
        }
    }
}
