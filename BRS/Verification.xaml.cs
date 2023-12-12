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
    /// Interaction logic for Verification.xaml
    /// </summary>
    public partial class Verification : Window
    {
        private string email { get; set; }
        private string type { get; set; }

        public Verification(string email, string type)
        {
            this.email = email;
            this.type = type;
            InitializeComponent();
        }

        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            string code = textBoxVerification.Text;
            AuthenticationController controller = new AuthenticationController();
            if (string.IsNullOrWhiteSpace(code) || !controller.IsVerificationCorrect(email, code))
            {
                errorMessage.Text = "Vul een geldige verificatie code in.";
                textBoxVerification.Focus();
                return;
            }

            if (type == "login")
            {
                controller.AddCreationDate(email);
                controller.LoginUserByEmail(email);
            }

            if(type == "passreset")
            {
                PasswordResetForm passwordResetForm = new (email);
                passwordResetForm.Show();
            }
            this.Close();
        }
    }
}
