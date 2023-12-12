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
using Controllers;

namespace Views
{
    /// <summary>
    /// Interaction logic for PasswordResetRequest.xaml
    /// </summary>
    public partial class PasswordResetRequest : Window
    {
        public PasswordResetRequest()
        {
            InitializeComponent();
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            string email = textBoxEmail.Text;
            AuthenticationController controller = new AuthenticationController();
            controller.SendResetResetPasswordCode(email);
            Verification verification = new(email, "passreset");
            verification.Show();
            this.Close();
        }
    }
}
