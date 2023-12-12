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
    /// Interaction logic for PasswordResetForm.xaml
    /// </summary>
    public partial class PasswordResetForm : Window
    {
        private string email { get; set; }
        public PasswordResetForm(string email)
        {
            this.email = email;
            InitializeComponent();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            string password1 = passwordResetBox.Password;
            string password2 = passwordResetBoxConfirm.Password;

            ResetPasswordController resetPasswordController = new();
            string errormessage = resetPasswordController.ResetPassword(email, password1, password2);
            if (errormessage != "")
            {
                errorMessage.Text = errormessage;
                passwordResetBox.Focus();
                return;
            }
            Login login = new();
            login.Show();
            this.Close();
        }
    }
}
