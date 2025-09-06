using Hotel_C4ta.Utils;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hotel_C4ta.Controller
{
    public class UserController
    {
        public UserDTO CargarUsuario(int id)
        {
            using var conn = DBContext.OpenConnection();
            try
            {
                // Buscar en admin
                string query = "SELECT ID, FullName, Rol AS ExtraInfo, PasswordHashed FROM Administrator WHERE ID=@id " +
                               "UNION " +
                               "SELECT ID, FullName, Code AS ExtraInfo, PasswordHashed FROM Receptionist WHERE ID=@id";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserDTO
                            {
                                ID = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                RoleCode = reader.GetString(2),
                                PasswordHashed = reader.GetString(3)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuario: " + ex.Message);
            }
            return null;
        }

        public bool GuardarUsuario(int? userId, string nombre, string rolCodigo, string password) // REVIEWREVIEWREVIEWREVIEWREVIEWREVIEWREVIEW
        {
            using var conn = DBContext.OpenConnection();
            try
            {
                string query;

                if (userId == null) // 👈 Crear
                {
                    if (rolCodigo.ToLower().Contains("admin"))
                        query = "INSERT INTO Administrator (FullName, Rol, PasswordHashed) VALUES (@fullname, @rol, @passwordhashed)";
                    else
                        query = "INSERT INTO Receptionist (FullName, Code, PasswordHashed) VALUES (@fullname, @code, @passwordhashed)";
                }
                else // 👈 Editar
                {
                    if (rolCodigo.ToLower().Contains("admin"))
                        query = "UPDATE Administrator SET FullName=@fullname, Rol=@rol, PasswordHashed=@passwordhashed WHERE Id=@id";
                    else
                        query = "UPDATE Receptionist SET FullName=@fullname, Code=@code, PasswordHashed=@passwordhashed WHERE Id=@id";
                }

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@fullname", nombre);
                    if (rolCodigo.ToLower().Contains("admin"))
                        cmd.Parameters.AddWithValue("@rol", rolCodigo);
                    else
                        cmd.Parameters.AddWithValue("@code", rolCodigo);

                    cmd.Parameters.AddWithValue("@passwordhashed", password);

                    if (userId != null)
                        cmd.Parameters.AddWithValue("@id", userId);

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar usuario: " + ex.Message);
            }

            return false;

        }
    }
}
