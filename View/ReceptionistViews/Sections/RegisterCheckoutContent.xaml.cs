using Hotel_C4ta.Model;
using Hotel_C4ta.Utils;
using Microsoft.Data.SqlClient;
using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Hotel_C4ta.View.ReceptionistViews.Sections
{
<<<<<<< HEAD
    public partial class RegisterCheckOutContent : UserControl
    {
        private DatabaseConnection _db = new DatabaseConnection();
        private int _selectedBookingId = 0;
        private double _selectedEstimatedPrice = 0;

=======
    /// <summary>
    /// Lógica de interacción para RegisterCheck_outContent.xaml
    /// </summary>
    public partial class RegisterCheckOutContent : UserControl
    {
>>>>>>> 89905a425d42465c321a2ff71423c49327930442
        public RegisterCheckOutContent()
        {
            InitializeComponent();
            if (GlobalFontSettings.FontResolver == null)
            {
                GlobalFontSettings.FontResolver = new CustomFontResolver();
            }

            LoadCheckedInBookings();
        }

        private void LoadCheckedInBookings()
        {
            var bookings = new List<BookingModel>();

            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;

                try
                {
                    string sql = @"SELECT Id, StartDate, EndDate, Status_, EstimatedPrice, DniClient, IdRecepcionist, RoomNumber
                                   FROM Booking
                                   WHERE Status_ = 'CheckedIn'";

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
                                _Status = reader.GetString(3),
                                _EstimatedPrice = (double)reader.GetDecimal(4),
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
                _selectedEstimatedPrice = booking._EstimatedPrice;

                TxtAmount.Text = booking._EstimatedPrice.ToString("F2");
                LblBookingInfo.Text = $"Reserva #{booking._Id} - Habitación {booking._RoomNumber}";
            }
        }

        private void BtnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBookingId == 0)
            {
                MessageBox.Show("Selecciona una reserva para hacer Check-Out.");
                return;
            }

            if (string.IsNullOrEmpty(CmbMethod.Text))
            {
                MessageBox.Show("Selecciona un método de pago.");
                return;
            }

            using (var conn = _db.OpenConnection())
            {
                if (conn == null)
                {
                    MessageBox.Show("Error: No se pudo conectar a la base de datos.");
                    return;
                }

                SqlTransaction transaction = null;

                try
                {
                    transaction = conn.BeginTransaction();

                    // 1. Obtener número de habitación
                    int roomNumber = 0;
                    string sqlGetRoom = "SELECT RoomNumber FROM Booking WHERE Id = @id";
                    using (var cmd = new SqlCommand(sqlGetRoom, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id", _selectedBookingId);
                        var result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Error: No se encontró la reserva seleccionada.");
                            transaction?.Rollback();
                            return;
                        }
                        roomNumber = (int)result;
                    }

                    // 2. Actualizar reserva a CheckedOut
                    string sqlBooking = @"UPDATE Booking
                                  SET Status_ = 'CheckedOut'
                                  WHERE Id = @id";
                    using (var cmd = new SqlCommand(sqlBooking, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id", _selectedBookingId);
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Cambiar habitación a Disponible
                    string sqlRoom = @"UPDATE Room
                               SET Status_ = 'Disponible'
                               WHERE Number = @roomNumber";
                    using (var cmd = new SqlCommand(sqlRoom, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@roomNumber", roomNumber);
                        cmd.ExecuteNonQuery();
                    }

                    // 4. Insertar factura (Bill)
                    int billNumber = 0;
                    string sqlBill = @"INSERT INTO Bill (IssueDate, Total, IdBooking)
                               OUTPUT INSERTED.Number
                               VALUES (GETDATE(), @total, @idBooking)";
                    using (var cmd = new SqlCommand(sqlBill, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@total", Convert.ToDecimal(TxtAmount.Text));
                        cmd.Parameters.AddWithValue("@idBooking", _selectedBookingId);
                        var billResult = cmd.ExecuteScalar();
                        if (billResult == null)
                        {
                            MessageBox.Show("Error: No se pudo crear la factura.");
                            transaction?.Rollback();
                            return;
                        }
                        billNumber = (int)billResult;
                    }

                    // 5. Insertar pago
                    string sqlPayment = @"INSERT INTO Payment (Dates, Amount, PaymentMethod, BillNumber)
                                  VALUES (GETDATE(), @amount, @method, @billNumber)";
                    using (var cmd = new SqlCommand(sqlPayment, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@amount", Convert.ToDecimal(TxtAmount.Text));
                        cmd.Parameters.AddWithValue("@method", CmbMethod.Text);
                        cmd.Parameters.AddWithValue("@billNumber", billNumber);
                        cmd.ExecuteNonQuery();
                    }

                    // 6. Recuperar datos para la factura
                    string clientName = "", clientDni = "", paymentMethod = CmbMethod.Text;
                    string roomNumStr = "";
                    DateTime startDate = DateTime.Now, endDate = DateTime.Now;
                    decimal total = Convert.ToDecimal(TxtAmount.Text);

                    string sqlInvoiceData = @"
                SELECT c.Names, c.Dni, r.Number, b.StartDate, b.EndDate
                FROM Booking b
                JOIN Client c ON b.DniClient = c.Dni
                JOIN Room r ON b.RoomNumber = r.Number
                WHERE b.Id = @id";
                    using (var cmd = new SqlCommand(sqlInvoiceData, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id", _selectedBookingId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientName = reader.IsDBNull(0) ? "Cliente desconocido" : reader.GetString(0);
                                clientDni = reader.IsDBNull(1) ? "Sin DNI" : reader.GetString(1);
                                roomNumStr = reader.IsDBNull(2) ? "0" : reader.GetInt32(2).ToString();
                                startDate = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3);
                                endDate = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4);
                            }
                            else
                            {
                                MessageBox.Show("Error: No se pudieron recuperar los datos de la factura.");
                                transaction?.Rollback();
                                return;
                            }
                        }
                    }

                    // 7. Generar factura en PDF (bytes en memoria)
                    byte[] pdfBytes = BillGenerator.GenerateInvoice(
                        clientName,
                        clientDni,
                        roomNumStr,
                        startDate,
                        endDate,
                        total,
                        paymentMethod
                    );

                    // 8. Guardar el PDF en la tabla Bill
                    string sqlUpdateBill = @"UPDATE Bill SET PdfFile = @pdf WHERE Number = @billNumber";
                    using (var cmd = new SqlCommand(sqlUpdateBill, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@pdf", pdfBytes);
                        cmd.Parameters.AddWithValue("@billNumber", billNumber);
                        cmd.ExecuteNonQuery();
                    }

                    // 9. Intentar abrir el PDF
                    try
                    {
                        BillGenerator.OpenInvoice(pdfBytes, billNumber);
                    }
                    catch
                    {
                        MessageBox.Show("Factura generada y guardada en BD, pero no se pudo abrir el PDF.");
                    }

                    transaction.Commit();
                    MessageBox.Show("✅ Check-Out completado, factura generada y guardada en la base de datos.");
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    MessageBox.Show($"Error en Check-Out: {ex.Message}\n\nDetalles: {ex.StackTrace}");
                }
                finally
                {
                    _db.CloseConnection();
                }
            }

            LoadCheckedInBookings();
            _selectedBookingId = 0;
            LblBookingInfo.Text = "(ninguna)";
            TxtAmount.Text = "";
            CmbMethod.SelectedIndex = -1;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            BookingsGrid.SelectedItem = null;
            _selectedBookingId = 0;
            LblBookingInfo.Text = "(ninguna)";
            TxtAmount.Text = "";
            CmbMethod.SelectedIndex = -1;
        }
    }
}
