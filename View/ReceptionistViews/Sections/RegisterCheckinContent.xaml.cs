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
using Hotel_C4ta.Controller;


namespace Hotel_C4ta.View.ReceptionistViews.Sections
{
    /// <summary>
    /// Lógica de interacción para RegisterCheck_inContent.xaml
    /// </summary>
    public partial class RegisterCheckInContent : UserControl
    {
        private int _selectedBookingId = 0;
        private BookingController _BookingCtrl = new BookingController();

        public RegisterCheckInContent()
        {
            InitializeComponent();
            LoadPendingsBookings();
        }

        public void LoadPendingsBookings()
        {
            var pendings = _BookingCtrl.GetPendingBookings();
            BookingsGrid.ItemsSource = pendings;
        }


        private void BookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookingsGrid.SelectedItem is BookingModel booking)
            {
                _selectedBookingId = booking.BookingID;
                LblBookingInfo.Text = $"Booking #{booking.BookingID} - Client: {booking.ClientDNI}, Room: {booking.RoomID}";
            }
            else
            {
                _selectedBookingId = 0;
                LblBookingInfo.Text = "(none)";
            }
        }


        private void BtnCheckIn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBookingId == 0)
            {
                MessageBox.Show("Select a reservation first.");
                return;
            }

            bool updated = _BookingCtrl.ChangeBookingStatus(_selectedBookingId, "CheckedIn");

            if (updated)
            {
                MessageBox.Show("✅ Check-in successful.");
                LoadPendingsBookings();
                LblBookingInfo.Text = "(none)";
                _selectedBookingId = 0;
            }
            else
            {
                MessageBox.Show("⚠️ Status could not be updated.");
            }
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            BookingsGrid.SelectedItem = null;
            _selectedBookingId = 0;
            LblBookingInfo.Text = "(none)";
        }

    }
}