using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Controller
{
    public class AuthController : DBContext
    {
        public string? HandleLogin(string username, string password) {
            using (SqlConnection conn = new(_ConnectionString))
            {

                string query = @"
                SELECT 'Admin' AS UserType
                FROM Administrator
                WHERE Names = @user AND Password_ = @pass
                UNION
                SELECT 'Recepcionist'
                FROM Recepcionist
                WHERE Names = @user AND Password_ = @pass
            ";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", password);

                object? result = cmd.ExecuteScalar();

                return result?.ToString();
            }
        
        }
    }
}
