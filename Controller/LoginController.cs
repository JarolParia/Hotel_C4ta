using Hotel_C4ta.Model;
//using Hotel_C4ta.View.AdminViews;
using Hotel_C4ta.View.ReceptionistViews;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hotel_C4ta.Controller
{
    public class LoginController
    {
        public (int id, string fullname, string rol) HandleLogin(string email, string password) {
            using var conn = DBContext.OpenConnection();

            string query = @"
                SELECT ID, FullName, Rol 
                FROM Administrator 
                WHERE Email = @email AND PasswordHashed = @pass
                UNION
                SELECT ID, FullName, Rol    
                FROM Receptionist 
                WHERE Email = @email AND PasswordHashed = @pass";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@pass", password);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                int id = reader.GetInt32(0);
                string fullname = reader.GetString(1);
                string rol = reader.GetString(2);
                return (id, fullname, rol);
            }
            else
            {
                throw new Exception("User not found or incorrect credentials.");
            }
        }
    }
}
