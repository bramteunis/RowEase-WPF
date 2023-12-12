using System.Windows;
using System.Windows.Controls;
using Utilities;
using Controllers;

namespace Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

            textBoxEmail.Text = "fabian@serree.me";
            passwordBox.Password = "testie";
        }

        private void Inloggen_Click(object sender, RoutedEventArgs e)
        {
            string email = textBoxEmail.Text;
            string password = passwordBox.Password;

            if(string.IsNullOrWhiteSpace(email) || !Validate.IsValidEmail(email))
            {
                errorMessage.Text = "Vul een geldig emailadres in.";
                textBoxEmail.Focus();
                return;
            }

            if(string.IsNullOrWhiteSpace(password))
            {
                errorMessage.Text = "Vul een wachtwoord in.";
                textBoxEmail.Focus();
                return;
            }

            AuthenticationController controller = new AuthenticationController();
            bool isValidLogin = controller.AuthenticateUser(email, password);
            if(!isValidLogin)
            {
                errorMessage.Text = "Fout bij inlog poging, probeer opnieuw!";
                textBoxEmail.Focus();
                return;
            }

            Dashboard dashboard = new();
            dashboard.Show();
            this.Close();
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Close();
        }

        private void LostPassword_Click(object sender, RoutedEventArgs e)
        {
            PasswordResetRequest passwordResetRequest = new();
            passwordResetRequest.Show();
            this.Close();
        }
    }
}
