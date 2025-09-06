using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Controller
{
    public static class DBContext
    {
        private static SqlConnection? _connection;

        public static SqlConnection OpenConnection()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["HotelC4TA"].ConnectionString;
            _connection = new SqlConnection(connectionString);

            try
            {
                _connection.Open();
                return _connection;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
                return null!;
            }
        }

        public static void CloseConnection()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open) _connection.Close();
        }
    }
}
