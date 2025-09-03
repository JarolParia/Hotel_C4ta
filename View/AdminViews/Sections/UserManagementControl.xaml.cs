using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hotel_C4ta.View.AdminViews
{
    /// <summary>
    /// Lógica de interacción para UserManagementControl.xaml
    /// </summary>
    public partial class UserManagementControl : UserControl
    {
        public UserManagementControl()
        {
            InitializeComponent();
            LoadUsers();
        }
        private void LoadUsers()
        {
            var users = new List<dynamic>(); // usamos dinámico para juntar admins y recepcionistas
            var db = new DatabaseConnection();

            using (var conn = db.OpenConnection())
            {
                try
                {
                    // Cargar administradores
                    string adminQuery = "SELECT Id, Names, Rol FROM Administrator";
                    using (var cmd = new SqlCommand(adminQuery, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new
                            {
                                _Id = reader.GetInt32(0),
                                _Name = reader.GetString(1),
                                ExtraInfo = reader.GetString(2) // Rol
                            });
                        }
                    }

                    // Cargar recepcionistas
                    string recepQuery = "SELECT Id, Names, Code FROM Recepcionist";
                    using (var cmd = new SqlCommand(recepQuery, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new
                            {
                                _Id = reader.GetInt32(0),
                                _Name = reader.GetString(1),
                                ExtraInfo = reader.GetString(2) // Código
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error al cargar usuarios: " + ex.Message);
                }
                finally
                {
                    db.CloseConnection();
                }
            }

            UsersGrid.ItemsSource = users;
        }

    }
}
