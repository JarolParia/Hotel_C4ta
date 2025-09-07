using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hotel_C4ta.Controller
{
    public class ReceptionistController
    {
        public ReceptionistModel _receptionistModel;
        public ReceptionistModel GetReceptionist(int id)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "SELECT * FROM Receptionist WHERE ID=@id";
                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new ReceptionistModel
                        {
                            ID = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Email = reader.GetString(2),
                            PasswordHashed = reader.GetString(3),
                            Rol = reader.GetString(4),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading receptionist: " + ex.Message);
            }
            return null;
        }
    }
}
