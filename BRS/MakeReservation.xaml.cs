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

namespace Views
{
    /// <summary>
    /// Interaction logic for MakeReservation.xaml
    /// </summary>
    public partial class MakeReservation : Window
    {
        public MakeReservation()
        {
            InitializeComponent();

            User user = AuthenticationController.loggedInUser!;
            welcomeUser.Text = "Welkom gebruiker: " + user.firstName + " " + user.lastName;

            BoatController boatController = new();
            boatListBox.ItemsSource = boatController.GetAllBoats();
        }

        private void BoatListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Boat? clickedBoat = boatListBox.SelectedItem as Boat;
            if (clickedBoat != null)
            {
                ReserveBootDialog beserveBootDialog = new(clickedBoat);
                beserveBootDialog.ShowDialog();
            }
        }

        private void BoatListSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? comboBox = (sender as ComboBox);
            if(comboBox != null)
            {
                ComboBoxItem? selectedItem = comboBox.SelectedItem as ComboBoxItem;
                if(selectedItem != null)
                {
                    string? sortText = selectedItem.Content as string;
                    if (boatListBox != null)
                    {
                        BoatController boatController = new();
                        List<Boat> boats = boatController.GetAllBoats();
                        switch (sortText)
                        {
                            case "Naam":
                                boatListBox.ItemsSource = boats.OrderBy(x => x.boatName);
                                break;
                            case "Bouwjaar":
                                boatListBox.ItemsSource = boats.OrderBy(x => x.buildYear);
                                break;
                            case "Stuur":
                                boatListBox.ItemsSource = boats.OrderBy(x => x.steer);
                                break;
                            case "Certificaat":
                                boatListBox.ItemsSource = boats.OrderBy(x => x.certificate);
                                break;
                            default:
                                boatListBox.ItemsSource = boats;
                                break;
                        }
                    }
                }
            }
        }

        private void BoatListFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? comboBox = (sender as ComboBox);
            if (comboBox != null)
            {
                ComboBoxItem? selectedItem = comboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    string? filterText = selectedItem.Content as string;
                    BoatController boatController = new();
                    List<Boat> boats = boatController.GetAllBoats();
                    if (boatListFilterExact != null)
                    {
                        boatListFilterExact.Visibility = Visibility.Visible;
                        switch (filterText)
                        {
                            case "Bouwjaar":
                                boatListFilterExact.ItemsSource = boats.OrderBy(x => x.buildYear).Select(x => x.buildYear).Distinct().ToList();
                                break;
                            case "Type":
                                boatListFilterExact.ItemsSource = boats.OrderBy(x => x.boatType).Select(x => x.boatType).Distinct().ToList();
                                break;
                            case "Certificaat":
                                boatListFilterExact.ItemsSource = boats.OrderBy(x => x.certificate.name).Select(x => x.certificate.name).Distinct().ToList();
                                break;
                            default:
                                boatListFilterExact.ItemsSource = "";
                                boatListFilterExact.Visibility = Visibility.Hidden;
                                break;
                        }
                        boatListSort.SelectedIndex = 0;
                    }
                }
            }
        }

        private void BoatListFilterExact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? comboBox = (sender as ComboBox);
            if (comboBox != null)
            {
                string? filterExactText = comboBox.SelectedItem as string;

                if(filterExactText == null)
                {
                    if (comboBox!.SelectedItem != null)
                    {
                        filterExactText = comboBox!.SelectedItem as string;
                    }
                }

                ComboBox? filterBox = (boatListFilter as ComboBox);
                ComboBoxItem? selectedFilter = (filterBox!.SelectedItem as ComboBoxItem);
                string? filter = selectedFilter!.Content as string;

                BoatController boatController = new();
                List<Boat> boats = boatController.GetAllBoats();

                switch(filter)
                {
                    case "Bouwjaar":
                        boatListBox.ItemsSource = boats.Where(x => x.buildYear == Convert.ToInt32(filterExactText)).ToList();
                        break;
                    case "Type":
                        boatListBox.ItemsSource = boats.Where(x => x.boatType.ToString() == filterExactText).ToList();
                        break;
                    case "Certificaat":
                        boatListBox.ItemsSource = boats.Where(x => x.certificate.name == filterExactText).ToList();
                        break;
                    default:
                        boatListBox.ItemsSource = boats;
                        break;
                }
            }
        }

        private void BoatMaintenance_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Image maintenanceButton = (Image)sender;
            int boatId = Convert.ToInt32(maintenanceButton.Tag);
            BoatController boatController = new();
            List<Boat> boats = boatController.GetAllBoats();
            Boat? targetBoat = boats.Where(x => x.id == boatId).FirstOrDefault();
            if(targetBoat != null)
            {
                boatController.ToggleBoatStatus(targetBoat);
                boats = boatController.GetAllBoats();
                boatListBox.ItemsSource = boats;
            }
        }

        private void BackImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Dashboard dashboard = new();
            dashboard.Show();
            this.Close();
        }

        private void BecomeMember_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(AuthenticationController.loggedInUser!.roleId == 1)
            {
                UserController userController = new();
                userController.RequestMembership(AuthenticationController.loggedInUser);
                MessageBox.Show("De aanvraag tot lid te worden is verstuurd!");
            } else if(AuthenticationController.loggedInUser!.roleId == 5)
            {
                MessageBox.Show("Jouw aanvraag is al verstuurd, wacht op een beheerder voor goedkeuring!");
            } else
            {
                MessageBox.Show("Jouw rol laat niet toe om deze actie uit te voeren.");
            }
        }
    }
}
