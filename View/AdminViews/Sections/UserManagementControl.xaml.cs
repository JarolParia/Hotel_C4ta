using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Hotel_C4ta.View.AdminViews.Sections
{
    public partial class UserManagementControl : UserControl
    {
        public UserManagementControl()
        {
            InitializeComponent();
            CargarUsuarios();
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            var form = new LoggerUsersSection();
            form.UsuarioGuardado += () =>
            {
                CargarUsuarios();
                SectionRegistrar.Content = null; // Limpia después de guardar
            };

            SectionRegistrar.Content = form;
        }

        private void CargarUsuarios()
        {
            var usuarios = new List<UsuarioViewModel>();
            var db = new DatabaseConnection();

            using (var conn = db.OpenConnection())
            {
                try
                {
                    // Cargar administradores
                    string queryAdmins = "SELECT Id, Names, Rol AS ExtraInfo FROM Administrator";
                    using (var cmd = new SqlCommand(queryAdmins, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new UsuarioViewModel
                            {
                                _Id = reader.GetInt32(0),
                                _Name = reader.GetString(1),
                                ExtraInfo = reader.GetString(2)
                            });
                        }
                    }

                    // Cargar recepcionistas
                    string queryRecep = "SELECT Id, Names, Code AS ExtraInfo FROM Recepcionist";
                    using (var cmd = new SqlCommand(queryRecep, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new UsuarioViewModel
                            {
                                _Id = reader.GetInt32(0),
                                _Name = reader.GetString(1),
                                ExtraInfo = reader.GetString(2)
                            });
                        }
                    }

                    UsersGrid.ItemsSource = usuarios;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar usuarios: " + ex.Message);
                }
                finally
                {
                    db.CloseConnection();
                }
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int userId)
            {
                var formEditar = new LoggerUsersSection(userId);
                formEditar.UsuarioGuardado += CargarUsuarios; // refrescar cuando guarde
                SectionRegistrar.Content = formEditar;
            }
        }


        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button)?.Tag?.ToString();
            if (MessageBox.Show("¿Eliminar este usuario?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var db = new DatabaseConnection();
                using (var conn = db.OpenConnection())
                {
                    try
                    {
                        string query = $"DELETE FROM Administrator WHERE Id=@Id; DELETE FROM Recepcionist WHERE Id=@Id;";
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.ExecuteNonQuery();
                        }

                        CargarUsuarios();
                        MessageBox.Show("Usuario eliminado.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar usuario: " + ex.Message);
                    }
                    finally
                    {
                        db.CloseConnection();
                    }
                }
            }
        }
    }

    // Modelo de usuario para mostrar en el DataGrid
    public class UsuarioViewModel
    {
        public int _Id { get; set; }
        public string _Name { get; set; }
        public string ExtraInfo { get; set; }
    }
}

