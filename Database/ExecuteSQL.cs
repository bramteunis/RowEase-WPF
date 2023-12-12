using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    internal class ExecuteSQL
    {
        private readonly DatabaseConnection _connection;

        public ExecuteSQL()
        {
            _connection = new DatabaseConnection();
        }

        public SqlDataReader ExecuteStatement(SqlCommand cmd)
        {
            try
            {
                SqlConnection connection = _connection.OpenConnection();
                cmd.Connection = connection;
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
    }
}
