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
using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;

<<<<<<< HEAD
    
=======

>>>>>>> 89905a425d42465c321a2ff71423c49327930442

namespace Hotel_C4ta.View.ReceptionistViews.Sections
{

    public partial class Search_UpdateClientContent : UserControl
    {
<<<<<<< HEAD

=======
        private DatabaseConnection _db = new DatabaseConnection();
        private string _selectedDni = null;
>>>>>>> 89905a425d42465c321a2ff71423c49327930442
        public Search_UpdateClientContent()
        {
            InitializeComponent();
            LoadClients();
        }
<<<<<<< HEAD
        
=======
        private void LoadClients()
        {
            var clients = new List<ClientModel>();
            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;

                try
                {
                    string sql = "SELECT Dni, Names, Email, Phone FROM Client";
                    using (var cmd = new SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(new ClientModel
                            {
                                _DNI = reader.GetString(0),
                                _Name = reader.GetString(1),
                                _Email = reader.GetString(2),
                                _Phone = reader.IsDBNull(3) ? "" : reader.GetString(3)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar clientes: " + ex.Message);
                }
                finally
                {
                    _db.CloseConnection();
                }
            }
            ClientsGrid.ItemsSource = clients;
        }

        private void ClientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is ClientModel client)
            {
                _selectedDni = client._DNI;
                TxtDni.Text = client._DNI;
                TxtName.Text = client._Name;
                TxtEmail.Text = client._Email;
                TxtPhone.Text = client._Phone;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedDni))
            {
                MessageBox.Show("Selecciona un cliente.");
                return;
            }

            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;

                try
                {
                    string sql = "UPDATE Client SET Names=@name, Email=@mail, Phone=@phone WHERE Dni=@dni";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@dni", TxtDni.Text.Trim());
                        cmd.Parameters.AddWithValue("@name", TxtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@mail", TxtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", TxtPhone.Text.Trim());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cliente actualizado.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar: " + ex.Message);
                }
                finally
                {
                    _db.CloseConnection();
                }
            }
            LoadClients();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedDni))
            {
                MessageBox.Show("Selecciona un cliente.");
                return;
            }

            var confirm = MessageBox.Show("¿Eliminar este cliente?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) return;

            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;

                try
                {
                    string sql = "DELETE FROM Client WHERE Dni=@dni";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@dni", _selectedDni);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cliente eliminado.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message);
                }
                finally
                {
                    _db.CloseConnection();
                }
            }
            LoadClients();
        }
>>>>>>> 89905a425d42465c321a2ff71423c49327930442

    }
}