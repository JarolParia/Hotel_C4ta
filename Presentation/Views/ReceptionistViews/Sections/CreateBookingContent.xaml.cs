using Hotel_C4ta.Application.Services;
using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Utils;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Hotel_C4ta.Presentation.Views.ReceptionistViews.Sections
{
    public partial class CreateBookingContent : UserControl
    {
        private readonly ServiceManager _serviceManager;
        private readonly User _loggedUser;

        public CreateBookingContent(ServiceManager serviceManager, User loggedUser)
        {
            InitializeComponent();
            _serviceManager = serviceManager;
            _loggedUser = loggedUser;

            LoadData();
        }

        private void LoadData()
        {
            // 🔹 Load clients
            var clients = _serviceManager.ClientService.GetAllClients();
            CmbClient.ItemsSource = clients;

            // 🔹 Mostrar recepcionista logueado
            TxtReceptionistName.Text = $"{_loggedUser.FullName}";

            // 🔹 Load available rooms
            var rooms = _serviceManager.RoomService.GetAllAvailableRooms();
            CmbRoom.ItemsSource = rooms;
        }

        // Método para calcular el precio
        private void CalculatePrice()
        {
            if (DpStartDate.SelectedDate == null || DpEndDate.SelectedDate == null || CmbRoom.SelectedItem == null)
            {
                TxtEstimatedPrice.Text = "0.00";
                return;
            }

            var start = DpStartDate.SelectedDate.Value;
            var end = DpEndDate.SelectedDate.Value;

            if (end <= start)
            {
                TxtEstimatedPrice.Text = "0.00";
                return;
            }

            var room = (Room)CmbRoom.SelectedItem;

            try
            {
                var estimatedPrice = CalculateBookingPrice.Calculate(room, start, end);
                TxtEstimatedPrice.Text = estimatedPrice.ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating price: {ex.Message}");
                TxtEstimatedPrice.Text = "0.00";
            }
        }

        // Se ejecuta cuando cambian las fechas
        private void DateChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculatePrice();
        }

        // Se ejecuta cuando cambia la habitación
        private void RoomChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculatePrice();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validaciones básicas
            if (CmbClient.SelectedItem == null)
            {
                MessageBox.Show("Please select a client.");
                return;
            }

            if (CmbRoom.SelectedItem == null)
            {
                MessageBox.Show("Please select a room.");
                return;
            }

            if (DpStartDate.SelectedDate == null || DpEndDate.SelectedDate == null)
            {
                MessageBox.Show("Please select both start and end dates.");
                return;
            }

            var start = DpStartDate.SelectedDate.Value;
            var end = DpEndDate.SelectedDate.Value;

            if (end <= start)
            {
                MessageBox.Show("End date must be after start date.");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtEstimatedPrice.Text) || TxtEstimatedPrice.Text == "0.00")
            {
                MessageBox.Show("Price calculation failed. Please try again.");
                return;
            }

            // Obtener datos
            var status = TxtStatus.Text;
            var client = (Client)CmbClient.SelectedItem;
            var room = (Room)CmbRoom.SelectedItem;
            var receptionistId = _loggedUser.ID;

            if (!decimal.TryParse(TxtEstimatedPrice.Text, out decimal price))
            {
                MessageBox.Show("Invalid price format.");
                return;
            }

            try
            {
                var created = _serviceManager.BookingService.RegisterBooking(
                    start, end, status, price, client.DNI, receptionistId, room.RoomID);

                if (created)
                {
                    _serviceManager.RoomService.UpdateStatusRoom(room.RoomID, "Occupied");


                    ClearForm();
                }
                else
                {
                    MessageBox.Show("❌ Error saving booking.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error creating booking: {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            CmbClient.SelectedItem = null;
            CmbRoom.SelectedItem = null;
            DpStartDate.SelectedDate = null;
            DpEndDate.SelectedDate = null;
            TxtEstimatedPrice.Text = "";
            TxtStatus.Text = "Pending";

            // Mantener el recepcionista mostrado
            TxtReceptionistName.Text = $"{_loggedUser.FullName} (ID: {_loggedUser.ID})";
        }
    }
}