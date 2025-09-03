using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Hotel_C4ta.Model
{
    class DatabaseConnection
    {
        private SqlConnection? _Connection;

        public SqlConnection OpenConnection()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["HotelC4ta"].ConnectionString;


            _Connection = new SqlConnection(connectionString);
          

            try
            {
                _Connection.Open();
                return _Connection;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
                return null!;
            }
        }

        public void CloseConnection()
        {
            if (_Connection != null && _Connection.State == System.Data.ConnectionState.Open) _Connection.Close();
        }
    }
}
