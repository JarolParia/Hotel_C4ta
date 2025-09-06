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

namespace Hotel_C4ta.View.ReceptionistViews.Sections
{
    /// <summary>
    /// Lógica de interacción para RegisterCheck_inContent.xaml
    /// </summary>
    public partial class RegisterCheckInContent : UserControl
    {
        private DatabaseConnection _db = new DatabaseConnection();
        private int _selectedBookingId = 0;

        public RegisterCheckInContent()
        {
            InitializeComponent();
            LoadPendingBookings();
        }

        private void LoadPendingBookings()
        {
            var bookings = new List<BookingModel>();

            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;

                try
                {
                    string sql = @"SELECT Id, StartDate, EndDate, Status_, EstimatedPrice, DniClient, IdRecepcionist, RoomNumber
                                   FROM Booking
                                   WHERE Status_ = 'Pendiente'";

                    using (var cmd = new SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bookings.Add(new BookingModel
                            {
                                _Id = reader.GetInt32(0),
                                _StartDate = reader.GetDateTime(1),
                                _EndDate = reader.GetDateTime(2),
                                _Status = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                _EstimatedPrice = reader.IsDBNull(4) ? 0 : (double)reader.GetDecimal(4),
                                _DniClient = reader.GetString(5),
                                _IdReceptionist = reader.GetInt32(6),
                                _RoomNumber = reader.GetInt32(7)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar reservas: " + ex.Message);
                }
                finally
                {
                    _db.CloseConnection();
                }
            }

            BookingsGrid.ItemsSource = bookings;
        }

        private void BookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookingsGrid.SelectedItem is BookingModel booking)
            {
                _selectedBookingId = booking._Id;
                LblBookingInfo.Text = $"#{booking._Id} - Cliente: {booking._DniClient}, Habitación: {booking._RoomNumber}";
            }
        }

        private void BtnCheckIn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBookingId == 0)
            {
                MessageBox.Show("Selecciona una reserva para hacer Check-In.");
                return;
            }

            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;

                SqlTransaction transaction = null;

                try
                {
                    transaction = conn.BeginTransaction();

                    // 1. Actualizar el estado de la reserva
                    string sqlBooking = @"UPDATE Booking
                                  SET Status_ = 'CheckedIn'
                                  WHERE Id = @id";

                    using (var cmd = new SqlCommand(sqlBooking, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id", _selectedBookingId);
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Obtener el número de habitación de esa reserva
                    int roomNumber = 0;
                    string sqlGetRoom = "SELECT RoomNumber FROM Booking WHERE Id = @id";

                    using (var cmd = new SqlCommand(sqlGetRoom, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id", _selectedBookingId);
                        roomNumber = (int)cmd.ExecuteScalar();
                    }

                    // 3. Actualizar estado de la habitación a "Ocupada"
                    string sqlRoom = @"UPDATE Room
                               SET Status_ = 'Ocupada'
                               WHERE Number = @roomNumber";

                    using (var cmd = new SqlCommand(sqlRoom, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@roomNumber", roomNumber);
                        cmd.ExecuteNonQuery();
                    }

                    // Confirmar transacción
                    transaction.Commit();

                    MessageBox.Show("✅ Check-In confirmado. La habitación ahora está ocupada.");
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    MessageBox.Show("Error al realizar Check-In: " + ex.Message);
                }
                finally
                {
                    _db.CloseConnection();
                }
            }

            LoadPendingBookings();
            _selectedBookingId = 0;
            LblBookingInfo.Text = "(ninguna)";
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            BookingsGrid.SelectedItem = null;
            _selectedBookingId = 0;
            LblBookingInfo.Text = "(ninguna)";
        }
    }
}
