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
    public class AdministratorController
    {
        public AdministratorModel _administratorModel;

        public AdministratorModel GetAdministrators(int id)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "SELECT * FROM Administrator WHERE ID=@id";
                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new AdministratorModel
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
        public void CreateAdministrator(AdministratorModel Admin)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "INSERT INTO Administrator (FullName,Email,PasswordHashed,Rol) VALUES (@name,@Email,@Pass,@rol)";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", Admin.FullName);
                cmd.Parameters.AddWithValue("@Email", Admin.Email);
                cmd.Parameters.AddWithValue("@Pass", Admin.PasswordHashed);
                cmd.Parameters.AddWithValue("@rol", Admin.Rol);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating Admin User: " + ex.Message);
            }
        }

        public void UpdateAdministrator(AdministratorModel Admin)
        {
            using var conn = DBContext.OpenConnection();
            try
            {
                string sql = "UPDATE Administrator SET FullName=@name, Email=@Email, PasswordHashed=@Pass, Rol=@rol WHERE ID=@id";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", Admin.ID);
                cmd.Parameters.AddWithValue("@name", Admin.FullName);
                cmd.Parameters.AddWithValue("@Email", Admin.Email);
                cmd.Parameters.AddWithValue("@Pass", Admin.PasswordHashed);
                cmd.Parameters.AddWithValue("@rol", Admin.Rol);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating Admin User: " + ex.Message);
            }
        }

        public void DeleteAdministrator(int id)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "DELETE FROM Administrator WHERE ID=@id";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting Admin User:" + ex.Message);
            }
        }
    }
}