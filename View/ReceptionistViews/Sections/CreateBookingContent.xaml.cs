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
    /// Lógica de interacción para CreateBookingContent.xaml
    /// </summary>
    public partial class CreateBookingContent : UserControl
    {
        private DatabaseConnection _db = new DatabaseConnection();
        public CreateBookingContent()
        {
            InitializeComponent();
            LoadClients();
            LoadReceptionists();
            LoadAvailableRooms();
        }
        private void LoadClients()
        {
            var clients = new List<ClientModel>();
            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;
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
                _db.CloseConnection();
            }
            CmbClient.ItemsSource = clients;
        }

        private void LoadReceptionists()
        {
            var receps = new List<ReceptionistModel>();
            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;
                string sql = "SELECT Id, Names FROM Recepcionist";
                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        receps.Add(new ReceptionistModel
                        {
                            _Id = reader.GetInt32(0), // hereda de UserModel
                            _Name = reader.GetString(1)
                        });
                    }
                }
                _db.CloseConnection();
            }
            CmbReceptionist.ItemsSource = receps;
        }

        private void LoadAvailableRooms()
        {
            var rooms = new List<RoomModel>();
            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;
                // Por ahora carga todas, luego filtramos por fechas seleccionadas
                string sql = "SELECT Number, Floors, Status_, Type_, Capacity, BasedPrice FROM Room WHERE Status_ = 'Disponible'";
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
                            _Capacity = reader.GetInt32(4),
                            _BasePrice = (double)reader.GetDecimal(5)
                        });
                    }
                }
                _db.CloseConnection();
            }
            CmbRoom.ItemsSource = rooms;
        }

        private void DateChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculatePrice();
        }

        private void CalculatePrice()
        {
            if (DpStartDate.SelectedDate == null || DpEndDate.SelectedDate == null || CmbRoom.SelectedItem == null)
                return;

            var start = DpStartDate.SelectedDate.Value;
            var end = DpEndDate.SelectedDate.Value;

            if (end <= start)
            {
                TxtEstimatedPrice.Text = "0";
                return;
            }

            var days = (end - start).Days;
            var room = (RoomModel)CmbRoom.SelectedItem;
            var total = days * room._BasePrice;

            TxtEstimatedPrice.Text = total.ToString("0.00");
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CmbClient.SelectedItem == null || CmbReceptionist.SelectedItem == null || CmbRoom.SelectedItem == null ||
                DpStartDate.SelectedDate == null || DpEndDate.SelectedDate == null)
            {
                MessageBox.Show("Completa todos los campos.");
                return;
            }

            var client = (ClientModel)CmbClient.SelectedItem;
            var recep = (ReceptionistModel)CmbReceptionist.SelectedItem;
            var room = (RoomModel)CmbRoom.SelectedItem;
            var start = DpStartDate.SelectedDate.Value;
            var end = DpEndDate.SelectedDate.Value;
            var price = double.Parse(TxtEstimatedPrice.Text);

            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;
                try
                {
                    string sql = @"INSERT INTO Booking (StartDate, EndDate, Status_, EstimatedPrice, DniClient, IdRecepcionist, RoomNumber)
                                   VALUES (@sd, @ed, @st, @price, @dni, @idrec, @room)";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@sd", start);
                        cmd.Parameters.AddWithValue("@ed", end);
                        cmd.Parameters.AddWithValue("@st", TxtStatus.Text);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@dni", client._DNI);
                        cmd.Parameters.AddWithValue("@idrec", recep._Id);
                        cmd.Parameters.AddWithValue("@room", room._Number);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Reserva creada correctamente.");
                    ClearForm();
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
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            CmbClient.SelectedItem = null;
            CmbReceptionist.SelectedItem = null;
            CmbRoom.SelectedItem = null;
            DpStartDate.SelectedDate = null;
            DpEndDate.SelectedDate = null;
            TxtEstimatedPrice.Text = "";
            TxtStatus.Text = "Pendiente";
        }

    }
}
