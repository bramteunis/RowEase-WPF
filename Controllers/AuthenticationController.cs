using Utilities;
using Database;
using Models;
using Mail;
using System.Xml;

namespace Controllers
{
    public class AuthenticationController
    {
        public static User? loggedInUser { get; set; }

        public bool AuthenticateUser(string email, string password)
        {
            string encryptedPassword = Secure.EncryptString(password);
            UserSQL userSQL = new UserSQL();
            User? user = userSQL.ValidateUser(email, encryptedPassword);
            if(user == null) return false;
            loggedInUser = user;
            return true;
        }

        public bool DoesUserExists(string email)
        {
            UserSQL userSQL = new UserSQL();
            return userSQL.GetUserByEmail(email) != null;
        }

        public bool CreateUser(string name, string email, int roleId, string password, string residence, string address, int certificate)
        {
            string encryptedPassword = Secure.EncryptString(password);
            string randomString = GenRandom.GenerateRandomString();
            UserSQL userSQL = new UserSQL();
            bool isCreated = userSQL.AddNewUser(name, email, roleId, encryptedPassword, residence, address, certificate, randomString);
            if(isCreated)
            {
                SendVerificationCode(email);
            }
            return isCreated;
        }

        public bool IsVerificationCorrect(string email, string code)
        {
            UserSQL userSQL = new UserSQL();
            return userSQL.IsUserCodeCorrect(email, code);
        }

        public void AddCreationDate(string email)
        {
            UserSQL userSQL = new UserSQL();
            userSQL.UpdateUserByEmail(email, "AccountCreated", DateTime.Now.ToString("MMM dd yyyy HH:mm:ss"));
        }

        public void LoginUserByEmail(string email)
        {
            UserSQL userSQL = new UserSQL();
            loggedInUser = userSQL.GetUserByEmail(email); 
        }

        public bool SendVerificationCode(string email)
        {
            UserSQL userSQL = new UserSQL();
            string code = userSQL.GetVerificationCodeByEmail(email);
            if(code != "")
            {
                MailAction mailAction = new MailAction();
                if(!mailAction.SendMail(email, "Verificatie code voor account", "Verificatie code voor je RBS account: " + code + "<br>Verzoek is verstuurd op: " + DateTime.Now))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public void SendResetResetPasswordCode(string email)
        {
            UserSQL userSQL = new();
            User? possibleUser = userSQL.GetUserByEmail(email);
            if(possibleUser != null)
            {
                userSQL.UpdateUserByEmail(email, "RememberToken", GenRandom.GenerateRandomString());
                SendVerificationCode(email);
            }
        }

        public void ChangeUserPassword(string email, string password)
        {
            UserSQL userSQL = new();
            string encryptedPassword = Secure.EncryptString(password);
            userSQL.UpdateUserByEmail(email, "Password", encryptedPassword);
        }
    }
}