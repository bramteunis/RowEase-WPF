using Controllers;
using Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Views
{
    /// <summary>
    /// Interaction logic for ManageUsers.xaml
    /// </summary>
    public partial class ManageUsersDialog : Window
    {
        //Maak een ObservableCollection om de data live te updaten op het scherm
        public static ObservableCollection<Reservation> allReservations = new ObservableCollection<Reservation>();
        public static ObservableCollection<User> listOfAppliedUsers = new ObservableCollection<User>();
        public static ObservableCollection<User> acceptedUsers = new ObservableCollection<User>();
        public static ObservableCollection<User> reservedUsers = new ObservableCollection<User>();
        public static ObservableCollection<User> allUsers = new ObservableCollection<User>();

        public ManageUsersDialog()
        {
            InitializeComponent();
            welcomeUser.Visibility = Visibility.Visible;
            welcomeUser.Text = "Welkom admin: " + AuthenticationController.loggedInUser!.firstName + " " + AuthenticationController.loggedInUser!.lastName;
            UserController userController = new();
            ReservationController reservationController = new();
            allUsers = new ObservableCollection<User>(userController.GetAllUsers());

            allReservations = new ObservableCollection<Reservation>(reservationController.GetAllReservations());
            listOfAppliedUsers = GetAppliedUsers();
            acceptedUsers = GetAllMembers();
            reservedUsers = GetUsersWithReservations();
            boatListBox.ItemsSource = allReservations;
            UserWithReservation.ItemsSource = reservedUsers;
            appliedUsersList.ItemsSource = listOfAppliedUsers;
            newMembers.ItemsSource = acceptedUsers;
        }

        //Laat de data van de geselecteerde data zien
        private void ShowUserData(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                User buttondata = (User)((Button)sender).DataContext;
                MessageBox.Show(buttondata.ToString());
            }
        }

        //Reset de sortering naar normaal
        private void resetSort(object sender, RoutedEventArgs e)
        {
            ReservationController reservationController = new();
            allReservations = new ObservableCollection<Reservation>(reservationController.GetAllReservations());
            boatListBox.ItemsSource = allReservations;
            Reserveringen.Text = "Alle reserveringen";
        }

        //Sorteer per gebruiker
        private void SortUserData(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                User buttondata = (User)((Button)sender).DataContext;
                ReservationController reservationController = new();
                allReservations = new ObservableCollection<Reservation>(reservationController.GetUserReservations(buttondata));
                boatListBox.ItemsSource = allReservations;
                Reserveringen.Text = $"Alle reserveringen van {buttondata.firstName + " " + buttondata.lastName}";
            }
        }

        //Verwijder de reservering van de geselecteerde gebruiker
        private void RemoveReservation(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                ReservationController reservationController = new();
                Reservation buttondata = (Reservation)((Button)sender).DataContext;
                allReservations.Remove(buttondata);
                reservationController.SendCancellationMail(buttondata);
                reservationController.RemoveReservation(buttondata.Id);
                reservedUsers = GetUsersWithReservations();
                UserWithReservation.ItemsSource = reservedUsers;
            }
        }

        //Maak de geselecteerde gebruiker lid van de vereniging
        private void AcceptPendingUser(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                UserController userController = new();
                User buttondata = (User)((Button)sender).DataContext;
                listOfAppliedUsers.Remove(buttondata);
                acceptedUsers.Add(buttondata);
                userController.MakeUserMemeber((int)buttondata.id!);
            }
        }

        //Wijs het verzoek van de geselecteerde gebruiker af om lid te worden van de vereniging
        private void DeclinePendingUser(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                UserController userController = new();
                User buttondata = (User)((Button)sender).DataContext;
                listOfAppliedUsers.Remove(buttondata);
                userController.DeclineMemberRequest((int)buttondata.id!);
            }
        }

        //Krijg alle gebruikers met roleid 5
        private static ObservableCollection<User> GetAppliedUsers()
        {
            return new ObservableCollection<User>(allUsers.Where(x => x.roleId == 5));
        }

        //Krijg alle gebruikers met een reservering
        private ObservableCollection<User> GetUsersWithReservations()
        {
            List<int> userIdWithReservations = new List<int>(allReservations.Select(x => x.MemberId).Distinct());
            return new ObservableCollection<User>(allUsers.Where(x => userIdWithReservations.Contains((int)x.id!)));
        }

        //Krijg alle gebruikers die lid zijn van de vereniging
        private ObservableCollection<User> GetAllMembers()
        {
            return new ObservableCollection<User>(allUsers.Where(x => x.roleId == 2 || x.roleId == 3 || x.roleId == 4));
        }
    }
}
