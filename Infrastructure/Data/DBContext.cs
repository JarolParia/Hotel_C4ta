using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Infrastructure.Data
{
    public class DBContext
    {
        private readonly string _connectionString;

        public DBContext(DatabaseConfig config)
        {
            _connectionString = config?.ConnectionString ??
                throw new ArgumentNullException("Connection string no puede ser null");
        }

        public SqlConnection OpenConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        // Método para probar la conexión
        public bool TestConnection()
        {
            try
            {
                using var connection = OpenConnection();
                return connection.State == System.Data.ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }
    }
}
