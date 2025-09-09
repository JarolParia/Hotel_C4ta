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
        public void CreateReceptionist(ReceptionistModel Recep)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "INSERT INTO Receptionist (FullName,Email,PasswordHashed,Rol) VALUES (@name,@Email,@Pass,@rol)";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", Recep.FullName);
                cmd.Parameters.AddWithValue("@Email", Recep.Email);
                cmd.Parameters.AddWithValue("@Pass", Recep.PasswordHashed);
                cmd.Parameters.AddWithValue("@rol", Recep.Rol);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating Receptionist User: " + ex.Message);
            }
        }


        public void UpdateReceptionist(ReceptionistModel Recep)
        {
            using var conn = DBContext.OpenConnection();
            try
            {
                string sql = "UPDATE Receptionist SET FullName=@name, Email=@Email, PasswordHashed=@Pass, Rol=@rol WHERE ID=@id";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", Recep.ID);
                cmd.Parameters.AddWithValue("@name", Recep.FullName);
                cmd.Parameters.AddWithValue("@Email", Recep.Email);
                cmd.Parameters.AddWithValue("@Pass", Recep.PasswordHashed);
                cmd.Parameters.AddWithValue("@rol", Recep.Rol);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating Receptionist User: " + ex.Message);
            }
        }

        public void DeleteReceptionist(int id)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "DELETE FROM Receptionist WHERE ID=@id";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting Receptionist User:" + ex.Message);
            }
        }


    }
}