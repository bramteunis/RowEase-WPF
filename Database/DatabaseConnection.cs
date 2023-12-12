using System.Data.SqlClient;
using System.Data;

namespace Database
{
    internal class DatabaseConnection
    {
        //Save the connection string
        private readonly string _connectionString;

        public DatabaseConnection()
        {
            _connectionString = CreateConnectionString();
        }

        private string CreateConnectionString()
        {
            //Create a connection string
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "116.202.3.163";
            builder.UserID = "SA";
            builder.Password = "idF1#ZAxVJ669#PLRgtN";
            builder.InitialCatalog = "roeivereniging";

            return builder.ConnectionString;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        internal SqlConnection OpenConnection()
        {
            SqlConnection connection = CreateConnection();
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }
    }
}