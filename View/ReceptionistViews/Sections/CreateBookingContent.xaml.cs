using Hotel_C4ta.Controller;
using Hotel_C4ta.Model;
using Hotel_C4ta.Utils;
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
        private readonly ClientController _clientController 
            = new ClientController();

        private readonly ReceptionistController _receptionistController
            = new ReceptionistController();

        private readonly RoomController _roomController 
            = new RoomController();

        private readonly BookingController _bookingController
            = new BookingController();

        public CreateBookingContent(int receptionistID)
        {
            InitializeComponent();
            var clients = _clientController.GetAllClients();
            CmbClient.ItemsSource = clients;

            var receptionist = _receptionistController.GetReceptionist(receptionistID);
            CmbReceptionist.Items.Add(receptionist);

            var rooms = _roomController.GetAllAvailableRooms();
            CmbRoom.ItemsSource = rooms;
        }

        private void DateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DpStartDate.SelectedDate == null || DpEndDate.SelectedDate == null || CmbRoom.SelectedItem == null)
                return;

            var start = DpStartDate.SelectedDate.Value;
            var end = DpEndDate.SelectedDate.Value;

            var EstimatedPrice = CalculateBookingPrice.Calculate(
                                (RoomModel)CmbRoom.SelectedItem, start, end);

            TxtEstimatedPrice.Text = EstimatedPrice.ToString("0.00");
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CmbClient.SelectedItem == null || CmbReceptionist.SelectedItem == null || CmbRoom.SelectedItem == null ||
                DpStartDate.SelectedDate == null || DpEndDate.SelectedDate == null)
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            var start = DpStartDate.SelectedDate.Value;
            var end = DpEndDate.SelectedDate.Value;
            var status = TxtStatus.Text;
            var price = decimal.Parse(TxtEstimatedPrice.Text);
            var client = (ClientModel)CmbClient.SelectedItem;
            var recep = (ReceptionistModel)CmbReceptionist.SelectedItem;
            var room = (RoomModel)CmbRoom.SelectedItem;
            
            var createdBooking = _bookingController.RegisterBooking(start, end, status, price, client.DNI, recep.ID, room.RoomID);

            if (createdBooking)
            {
                _roomController.UpdateStatusRoom(room.RoomID, "Occupied");

                MessageBox.Show($"Booking created successfully. {room.RoomID} room status changed to Occupied");
                ClearForm();
            }
            else
            {
                MessageBox.Show("Error saving booking.");
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
            TxtStatus.Text = "Pending";
        }
    }
}
