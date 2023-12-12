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
using Utilities;

namespace Views
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        private List<Certificate> certificates;
        public Register()
        {
            InitializeComponent();
            UserController userController = new UserController();
            certificates = userController.GetAllAvailableCertificates();
            certificateComboBox.ItemsSource = certificates.Select(x => x.name);
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string name = textBoxName.Text;
            string address = textBoxAddress.Text;
            string residence = textBoxResidence.Text;
            string email = textBoxEmail.Text;
            string password = passwordBox.Password;
            string passwordConfirm = passwordBoxConfirm.Password;
            Certificate? certificate = certificates.Find(certificate => certificate.name.Equals(certificateComboBox.SelectedItem.ToString()));
            if (String.IsNullOrWhiteSpace(name))
            {
                errorMessage.Text = "Vul een naam in.";
                textBoxName.Focus();
            }
            else if (!name.Contains(" "))
            {
                errorMessage.Text = "Vul uw volledige naam in met een spatie er tussen.";
                textBoxName.Focus();
            }
            else if (String.IsNullOrWhiteSpace(address))
            {
                errorMessage.Text = "Vul een address in.";
                textBoxAddress.Focus();
            }
            else if (String.IsNullOrWhiteSpace(residence))
            {
                errorMessage.Text = "Vul een woonplaats in.";
                textBoxResidence.Focus();
            }
            else if (String.IsNullOrWhiteSpace(email))
            {
                errorMessage.Text = "Vul een email in.";
                textBoxEmail.Focus();
            }
            else if (!Validate.IsValidEmail(email))
            {
                errorMessage.Text = "Vul een geldig email in.";
                textBoxEmail.Select(0, email.Length);
                textBoxEmail.Focus();
            }
            else if (String.IsNullOrWhiteSpace(password))
            {
                errorMessage.Text = "Vul een geldig wachtwoord in.";
                passwordBox.Focus();
            }
            else if (password != passwordConfirm)
            {
                errorMessage.Text = "Vul het zelfde wachtwoord in.";
                passwordBoxConfirm.Focus();
            }
            else if (certificate == null)
            {
                errorMessage.Text = "Het geselecteerde certificaat is niet gevonden.";
                certificateComboBox.Focus();
            }
            else
            {
                errorMessage.Text = " ";
                AuthenticationController controller = new AuthenticationController();
                if(certificate.id > 0)
                {
                    int roleId = 1;
                    if((bool)checkBoxMember.IsChecked!)
                    {
                        roleId = 5;
                    }
                    if(!controller.CreateUser(name, email, roleId, password, residence, address, certificate.id))
                    {
                        errorMessage.Text = "Er is een fout bij het aanmaken van het account, contacteer een beheerder!";
                        return;
                    }
                    MessageBox.Show("Succes");
                    Verification verification = new(email, "login");
                    verification.Show();
                    this.Close();
                }
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new();
            login.Show();
            this.Close();
        }
    }
}
