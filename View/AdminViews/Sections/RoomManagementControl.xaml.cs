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

namespace Hotel_C4ta.View.AdminViews.Sections
{
    /// <summary>
    /// Lógica de interacción para RoomManagementContent.xaml
    /// </summary>
    public partial class RoomManagementControl : UserControl
    {
        private DatabaseConnection _db = new DatabaseConnection();
        private int _selectedRoomNumber = 0;
        public RoomManagementControl()
        {
            InitializeComponent(); 
            LoadRooms();
            ClearForm();
        }

        private void LoadRooms()
        {
            var rooms = new List<RoomModel>();

            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;

                try
                {
                    string sql = "SELECT Number, Floors, Status_, Type_, Capacity, BasedPrice FROM Room";
                    using (var cmd = new SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rooms.Add(new RoomModel
                            {
                                _Number = reader.GetInt32(0),
                                _Floor = reader.GetInt32(1),
                                _Status = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                _Type = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                _Capacity = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                _BasePrice = reader.IsDBNull(5) ? 0 : (double)reader.GetDecimal(5)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar habitaciones: " + ex.Message);
                }
                finally
                {
                    _db.CloseConnection();
                }
            }

            RoomsGrid.ItemsSource = rooms;
        }

        private void RoomsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoomsGrid.SelectedItem is RoomModel room)
            {
                _selectedRoomNumber = room._Number;
                TxtNumber.Text = room._Number.ToString();
                TxtFloor.Text = room._Floor.ToString();
                TxtStatus.Text = room._Status;
                TxtType.Text = room._Type;
                TxtCapacity.Text = room._Capacity.ToString();
                TxtBasePrice.Text = room._BasePrice.ToString();
            }
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            _selectedRoomNumber = 0;
            TxtNumber.Text = "";
            TxtFloor.Text = "";
            TxtStatus.Text = "";
            TxtType.Text = "";
            TxtCapacity.Text = "";
            TxtBasePrice.Text = "";
            RoomsGrid.SelectedItem = null;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TxtNumber.Text, out int number) ||
                !int.TryParse(TxtFloor.Text, out int floor) ||
                !int.TryParse(TxtCapacity.Text, out int capacity) ||
                !double.TryParse(TxtBasePrice.Text, out double basePrice))
            {
                MessageBox.Show("Datos inválidos.");
                return;
            }

            string status = TxtStatus.Text.Trim();
            string type = TxtType.Text.Trim();

            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;

                try
                {
                    if (_selectedRoomNumber == 0)
                    {
                        // INSERT
                        string insertSql = @"INSERT INTO Room (Number, Floors, Status_, Type_, Capacity, BasedPrice)
                                             VALUES (@num, @floor, @status, @type, @cap, @price)";
                        using (var cmd = new SqlCommand(insertSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@num", number);
                            cmd.Parameters.AddWithValue("@floor", floor);
                            cmd.Parameters.AddWithValue("@status", (object)status ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@type", (object)type ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@cap", capacity);
                            cmd.Parameters.AddWithValue("@price", basePrice);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Habitación agregada.");
                        }
                    }
                    else
                    {
                        // UPDATE
                        string updateSql = @"UPDATE Room
                                             SET Floors=@floor, Status_=@status, Type_=@type, Capacity=@cap, BasedPrice=@price
                                             WHERE Number=@num";
                        using (var cmd = new SqlCommand(updateSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@num", number);
                            cmd.Parameters.AddWithValue("@floor", floor);
                            cmd.Parameters.AddWithValue("@status", (object)status ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@type", (object)type ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@cap", capacity);
                            cmd.Parameters.AddWithValue("@price", basePrice);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Habitación actualizada.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message);
                }
                finally
                {
                    _db.CloseConnection();
                }
            }

            LoadRooms();
            ClearForm();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoomNumber == 0)
            {
                MessageBox.Show("Selecciona una habitación para eliminar.");
                return;
            }

            var confirm = MessageBox.Show("¿Eliminar la habitación seleccionada?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) return;

            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;

                try
                {
                    string delSql = "DELETE FROM Room WHERE Number = @num";
                    using (var cmd = new SqlCommand(delSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@num", _selectedRoomNumber);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Habitación eliminada.");
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

            LoadRooms();
            ClearForm();
        }
        
    }
}
