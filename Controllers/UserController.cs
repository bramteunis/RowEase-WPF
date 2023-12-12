using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Database;

namespace Controllers
{
    public class UserController
    {
        public void RequestMembership(User user)
        {
            UserSQL userSQL = new UserSQL();
            userSQL.UpdateUserByEmail(user.email!, "RoleId", "5");
            AuthenticationController authenticationController = new();
            authenticationController.LoginUserByEmail(user.email!);
        }

        public void MakeUserMemeber(int userId)
        {
            UserSQL userSQL = new UserSQL();
            User? user = userSQL.GetUserById(userId);
            if (user != null)
            {
                userSQL.UpdateUserByEmail(user.email!, "MemberSince", DateTime.Now.ToString("MMM dd yyyy HH:mm:ss"));
                userSQL.UpdateUserByEmail(user.email!, "RoleId", "2");
            }
        }

        public void DeclineMemberRequest(int userId)
        {
            UserSQL userSQL = new UserSQL();
            User? user = userSQL.GetUserById(userId);
            if (user != null)
            {
                userSQL.UpdateUserByEmail(user.email!, "RoleId", "1");
            }
        }

        public List<User> GetAllUsers()
        {
            UserSQL userSQL = new UserSQL();
            return userSQL.GetAllRegisteredUsers();
        }

        public List<Certificate> GetAllAvailableCertificates()
        {
            UserSQL userSQL = new UserSQL();
            return userSQL.GetAllCertificates();
        }
    }
}
