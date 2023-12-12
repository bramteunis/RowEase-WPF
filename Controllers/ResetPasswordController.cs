using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class ResetPasswordController
    {

        public string ResetPassword(string email, string password, string passwordConfirm)
        {
            if (String.IsNullOrWhiteSpace(password) || String.IsNullOrWhiteSpace(passwordConfirm))
            {
                return "De wachtwoord velden mogen niet leeg zijn.";
            }
            else if (password != passwordConfirm)
            {
                return "De wachtwoorden komen niet overeen.";
                
            }
            AuthenticationController authenticationController = new();
            authenticationController.ChangeUserPassword(email, password);
            return "";
        }
    }
}
