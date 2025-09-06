using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Hotel_C4ta.View.AdminViews.Sections
{
    public partial class LoggerUsersSection : UserControl
    {
        private int? userId = null; // 👈 null = modo crear, número = modo editar

        public event Action UsuarioGuardado;

        public LoggerUsersSection()
        {
            InitializeComponent();
        }

        // 👇 Constructor para edición
        public LoggerUsersSection(int id) : this()
        {
            userId = id;
            CargarUsuario(id);
        }

        private void CargarUsuario(int id)
        {
            var db = new DatabaseConnection();
            using (var conn = db.OpenConnection())
            {
                try
                {
                    // Buscar en admin
                    string query = "SELECT Names, Rol AS ExtraInfo, Password_ FROM Administrator WHERE Id=@Id " +
                                   "UNION " +
                                   "SELECT Names, Code AS ExtraInfo, Password_ FROM Recepcionist WHERE Id=@Id";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TxtNombre.Text = reader.GetString(0);
                                TxtRolCodigo.Text = reader.GetString(1);
                                Pwd.Password = reader.GetString(2);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar usuario: " + ex.Message);
                }
                finally
                {
                    db.CloseConnection();
                }
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            var nombre = TxtNombre.Text.Trim();
            var rolCodigo = TxtRolCodigo.Text.Trim();
            var password = Pwd.Password.Trim();

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(rolCodigo) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor llena todos los campos.");
                return;
            }

            var db = new DatabaseConnection();
            using (var conn = db.OpenConnection())
            {
                try
                {
                    string query;

                    if (userId == null) // 👈 Crear
                    {
                        if (rolCodigo.ToLower().Contains("admin"))
                            query = "INSERT INTO Administrator (Names, Rol, Password_) VALUES (@Names, @Rol, @Password_)";
                        else
                            query = "INSERT INTO Recepcionist (Names, Code, Password_) VALUES (@Names, @Code, @Password_)";
                    }
                    else // 👈 Editar
                    {
                        if (rolCodigo.ToLower().Contains("admin"))
                            query = "UPDATE Administrator SET Names=@Names, Rol=@Rol, Password_=@Password_ WHERE Id=@Id";
                        else
                            query = "UPDATE Recepcionist SET Names=@Names, Code=@Code, Password_=@Password_ WHERE Id=@Id";
                    }

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Names", nombre);
                        if (rolCodigo.ToLower().Contains("admin"))
                            cmd.Parameters.AddWithValue("@Rol", rolCodigo);
                        else
                            cmd.Parameters.AddWithValue("@Code", rolCodigo);

                        cmd.Parameters.AddWithValue("@Password_", password);

                        if (userId != null)
                            cmd.Parameters.AddWithValue("@Id", userId);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show(userId == null ? "Usuario creado exitosamente." : "Usuario actualizado exitosamente.");

                    UsuarioGuardado?.Invoke(); // 👈 Notificar a la tabla
                    var parent = this.Parent as ContentControl;
                    if (parent != null)
                    {
                        parent.Content = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar usuario: " + ex.Message);
                }
                finally
                {
                    db.CloseConnection();
                }
            }
        }
    }
}
