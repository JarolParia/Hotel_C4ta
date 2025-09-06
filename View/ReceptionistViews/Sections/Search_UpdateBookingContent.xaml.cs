using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Lógica de interacción para Search_UpdateBookingContent.xaml
    /// </summary>
    public partial class Search_UpdateBookingContent : UserControl
    {
        private DatabaseConnection db = new DatabaseConnection();
        private int selectedBookingId = -1;
        public Search_UpdateBookingContent()
        {
            InitializeComponent();
            LoadBookings();
            LoadClients();
            LoadRooms();
        }


        // 🔹 Cargar reservas en el DataGrid
        private void LoadBookings()
        {
            try
            {
                using (SqlConnection conn = db.OpenConnection())
                {
                    string query = "SELECT * FROM Booking WHERE Status_ NOT IN ('CheckedIn', 'CheckedOut')";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    BookingsGrid.ItemsSource = dt.DefaultView;
                }
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar reservas: " + ex.Message);
            }
        }

        // 🔹 Cargar clientes en ComboBox
        private void LoadClients()
        {
            try
            {
                using (SqlConnection conn = db.OpenConnection())
                {
                    string query = "SELECT Dni FROM Client";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CmbClient.Items.Add(reader["Dni"].ToString());
                    }
                }
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
            }
        }

        // 🔹 Cargar habitaciones en ComboBox
        private void LoadRooms()
        {
            try
            {
                using (SqlConnection conn = db.OpenConnection())
                {
                    string query = "SELECT Number FROM Room WHERE Status_ = 'Disponible'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CmbRoom.Items.Add(reader["Number"].ToString());
                    }
                }
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar habitaciones: " + ex.Message);
            }
        }

        // 🔹 Seleccionar una reserva en el DataGrid
        private void BookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookingsGrid.SelectedItem is DataRowView row)
            {
                selectedBookingId = Convert.ToInt32(row["Id"]);

                CmbClient.SelectedItem = row["DniClient"].ToString();
                CmbRoom.SelectedItem = row["RoomNumber"].ToString();
                TxtReceptionist.Text = row["IdRecepcionist"].ToString();
                DpStartDate.SelectedDate = Convert.ToDateTime(row["StartDate"]);
                DpEndDate.SelectedDate = Convert.ToDateTime(row["EndDate"]);
                CmbStatus.Text = row["Status_"].ToString();
                TxtEstimatedPrice.Text = row["EstimatedPrice"].ToString();
            }
        }

        // 🔹 Actualizar reserva
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBookingId == -1)
            {
                MessageBox.Show("Seleccione una reserva primero.");
                return;
            }

            try
            {
                using (SqlConnection conn = db.OpenConnection())
                {
                    string query = @"UPDATE Booking 
                                     SET DniClient = @DniClient,
                                         RoomNumber = @RoomNumber,
                                         StartDate = @StartDate,
                                         EndDate = @EndDate,
                                         Status_ = @Status_,
                                         EstimatedPrice = @EstimatedPrice
                                     WHERE Id = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@DniClient", CmbClient.SelectedItem?.ToString());
                    cmd.Parameters.AddWithValue("@RoomNumber", CmbRoom.SelectedItem?.ToString());
                    cmd.Parameters.AddWithValue("@StartDate", DpStartDate.SelectedDate);
                    cmd.Parameters.AddWithValue("@EndDate", DpEndDate.SelectedDate);
                    cmd.Parameters.AddWithValue("@Status_", CmbStatus.Text);
                    cmd.Parameters.AddWithValue("@EstimatedPrice", TxtEstimatedPrice.Text);
                    cmd.Parameters.AddWithValue("@Id", selectedBookingId);

                    cmd.ExecuteNonQuery();
                }
                db.CloseConnection();

                MessageBox.Show("Reserva actualizada correctamente.");
                LoadBookings();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar reserva: " + ex.Message);
            }
        }

        // 🔹 Eliminar reserva
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBookingId == -1)
            {
                MessageBox.Show("Seleccione una reserva primero.");
                return;
            }

            if (MessageBox.Show("¿Está seguro de eliminar la reserva?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = db.OpenConnection())
                    {
                        string query = "DELETE FROM Booking WHERE Id = @Id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Id", selectedBookingId);
                        cmd.ExecuteNonQuery();
                    }
                    db.CloseConnection();

                    MessageBox.Show("Reserva eliminada correctamente.");
                    LoadBookings();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar reserva: " + ex.Message);
                }
            }
        }

        // 🔹 Cancelar y limpiar formulario
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            selectedBookingId = -1;
            CmbClient.SelectedIndex = -1;
            CmbRoom.SelectedIndex = -1;
            TxtReceptionist.Text = "";
            DpStartDate.SelectedDate = null;
            DpEndDate.SelectedDate = null;
            CmbStatus.SelectedIndex = -1;
            TxtEstimatedPrice.Text = "";
        }
    }
}
