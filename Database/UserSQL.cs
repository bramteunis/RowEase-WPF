using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Models;

namespace Database
{
    public class UserSQL
    {
        private readonly ExecuteSQL _connection;

        public UserSQL()
        {
            _connection = new ExecuteSQL();
        }

        public List<Certificate> GetAllCertificates()
        {
            SqlCommand command = new("SELECT * FROM certificates");

            List<Certificate> certificates = new();

            using SqlDataReader reader = _connection.ExecuteStatement(command);
            while (reader.Read())
            {
                int certificateId = reader.GetInt32(0);
                string certificateName = reader.GetString(1);
                certificates.Add(new Certificate(certificateId, certificateName));
            }
            return certificates;
        }

        public Certificate GetCertificateById(int id)
        {
            SqlCommand command = new("SELECT * FROM certificates WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = _connection.ExecuteStatement(command);
            if (reader.Read())
            {
                int certificateId = reader.GetInt32(0);
                string certificateName = reader.GetString(1);
                return new Certificate(certificateId, certificateName);
            }
            return new Certificate(0, "none");
        }

        public User? ValidateUser(string email, string password)
        {
            SqlCommand command = new("SELECT * FROM users WHERE Email = @Email AND Password = @Password");
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password);

            using SqlDataReader reader = _connection.ExecuteStatement(command);
            if (reader.Read())
            {
                int certificate = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                string memberSince = reader.IsDBNull(11) ? "Not a member" : reader.GetString(11);
                return new User(reader!.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), GetRoleById(reader.GetInt32(4))!, GetCertificateById(certificate), reader.GetString(7), reader.GetString(8), reader.GetString(10), memberSince);
            }
            return null;
        }

        private Role? GetRoleById(int roleId)
        {
            SqlCommand command = new("SELECT * FROM roles WHERE Id = @Id");
            command.Parameters.AddWithValue("@Id", roleId);
            using SqlDataReader reader = _connection.ExecuteStatement(command);
            if (reader.Read())
            {
                return new Role(reader!.GetInt32(0), reader.GetString(1), Convert.ToBoolean(reader.GetInt32(2)), reader.GetInt32(3), Convert.ToBoolean(reader.GetInt32(4)), reader.GetInt32(5));
            }
            return null;
        }

        public User? GetUserByEmail(string email)
        {
            SqlCommand command = new("SELECT * FROM users WHERE Email = @Email");
            command.Parameters.AddWithValue("@Email", email);
            using SqlDataReader reader = _connection.ExecuteStatement(command);
            if (reader.Read())
            {
                int certificate = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                string memberSince = reader.IsDBNull(11) ? "Not a member" : reader.GetString(11);
                return new User(reader!.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), GetRoleById(reader.GetInt32(4))!, GetCertificateById(certificate), reader.GetString(7), reader.GetString(8), reader.GetString(10), memberSince);
            }
            return null;
        }

        public User? GetUserById(int userid)
        {
            SqlCommand command = new("SELECT * FROM users WHERE Id = @UserId");
            command.Parameters.AddWithValue("@UserId", userid);
            using SqlDataReader reader = _connection.ExecuteStatement(command);
            if (reader.Read())
            {
                int certificate = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                string memberSince = reader.IsDBNull(11) ? "Not a member" : reader.GetString(11);
                return new User(reader!.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), GetRoleById(reader.GetInt32(4))!, GetCertificateById(certificate), reader.GetString(7), reader.GetString(8), reader.GetString(10), memberSince);
            }
            return null;
        }

        public bool AddNewUser(string name, string email, int roleId, string password, string residence, string address, int certificate, string randomString)
        {
            SqlCommand checkUserCommand = new("SELECT 1 FROM users WHERE Email = @Email AND AccountCreated IS NULL;");
            checkUserCommand.Parameters.AddWithValue("@Email", email);
            using SqlDataReader readerSelectNull = _connection.ExecuteStatement(checkUserCommand);

            if (readerSelectNull.Read())
            {
                SqlCommand updateCommand = new("UPDATE users SET FirstName = @FirstName, LastName = @LastName, RoleId = @RoleID, Certificate = @Certificate, Password = @Password, Residence = @Residence, Address = @Address, RememberToken = @RandomString WHERE Email = @Email");
                updateCommand.Parameters.AddWithValue("@FirstName", name.Split(" ", 2)[0]);
                updateCommand.Parameters.AddWithValue("@LastName", name.Split(" ", 2)[1]);
                updateCommand.Parameters.AddWithValue("@RoleID", roleId);
                updateCommand.Parameters.AddWithValue("@Password", password);
                updateCommand.Parameters.AddWithValue("@Residence", residence);
                updateCommand.Parameters.AddWithValue("@Address", address);
                updateCommand.Parameters.AddWithValue("@Certificate", certificate);
                updateCommand.Parameters.AddWithValue("@Email", email);
                updateCommand.Parameters.AddWithValue("@RandomString", randomString);

                using SqlDataReader readerUpdate = _connection.ExecuteStatement(updateCommand);

                return readerUpdate.RecordsAffected > 0;
            }

            User? possibleUser = GetUserByEmail(email);

            if (possibleUser != null)
            {
                return false;
            }

            SqlCommand insertCommand = new("INSERT INTO users (FirstName, LastName, Email, RoleId, Certificate, Password, Residence, Address, RememberToken) VALUES (@FirstName, @LastName, @Email, @RoleID, @Certificate, @Password, @Residence, @Address, @RandomString)");
            insertCommand.Parameters.AddWithValue("@FirstName", name.Split(" ", 2)[0]);
            insertCommand.Parameters.AddWithValue("@LastName", name.Split(" ", 2)[1]);
            insertCommand.Parameters.AddWithValue("@Email", email);
            insertCommand.Parameters.AddWithValue("@RoleID", roleId);
            insertCommand.Parameters.AddWithValue("@Password", password);
            insertCommand.Parameters.AddWithValue("@Residence", residence);
            insertCommand.Parameters.AddWithValue("@Address", address);
            insertCommand.Parameters.AddWithValue("@Certificate", certificate);
            insertCommand.Parameters.AddWithValue("@RandomString", randomString);
            using SqlDataReader reader = _connection.ExecuteStatement(insertCommand);

            return reader.RecordsAffected > 0;
        }

        public bool IsUserCodeCorrect(string email, string code)
        {
            SqlCommand command = new("SELECT * FROM users WHERE Email = @Email AND RememberToken = @Code");
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Code", code);
            using SqlDataReader reader = _connection.ExecuteStatement(command);
            if (reader.Read())
            {
                return true;
            }
            return false;
        }

        public string GetVerificationCodeByEmail(string email)
        {
            SqlCommand command = new("SELECT RememberToken FROM users WHERE Email = @Email");
            command.Parameters.AddWithValue("@Email", email);
            using SqlDataReader reader = _connection.ExecuteStatement(command);
            if (reader.Read())
            {
                return reader.GetString(0);
            }
            return "";
        }

        public void UpdateUserByEmail(string email, string column, string value)
        {
            SqlCommand command = new($"UPDATE users SET {column} = @Value WHERE Email = @Email");
            command.Parameters.AddWithValue("@Value", value);
            command.Parameters.AddWithValue("@Email", email);
            _connection.ExecuteStatement(command);
        }

        public List<User> GetAllRegisteredUsers()
        {
            SqlCommand command = new("SELECT * FROM users;");
            List<User> users = new();
            using SqlDataReader reader = _connection.ExecuteStatement(command);
            while(reader.Read())
            {
                int certificate = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                string memberSince = reader.IsDBNull(11) ? "Not a member" : reader.GetString(11);
                users.Add(new User(reader!.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), GetRoleById(reader.GetInt32(4))!, GetCertificateById(certificate), reader.GetString(7), reader.GetString(8), reader.GetString(10), memberSince));
            }
            return users;
        }
    }
}
