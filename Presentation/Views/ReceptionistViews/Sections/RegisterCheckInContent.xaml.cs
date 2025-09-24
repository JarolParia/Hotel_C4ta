using Hotel_C4ta.Application.Services;
using Hotel_C4ta.Domain.Entities;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Hotel_C4ta.Presentation.Views.ReceptionistViews.Sections
{
    /// <summary>
    /// Lógica de interacción para RegisterCheckInContent.xaml
    /// </summary>
    public partial class RegisterCheckInContent : UserControl
    {
        private readonly ServiceManager _serviceManager;
        private int _selectedBookingId = 0;

        public RegisterCheckInContent(ServiceManager serviceManager)
        {
            InitializeComponent();
            _serviceManager = serviceManager;
            LoadPendingBookings();
        }

        public void LoadPendingBookings()
        {
            try
            {
                var bookings = _serviceManager.BookingService.GetPendingAndConfirmedBookings();
                BookingsGrid.ItemsSource = bookings;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bookings: {ex.Message}");
            }
        }

        private void BookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookingsGrid.SelectedItem is Booking booking)
            {
                _selectedBookingId = booking.BookingID;
                LblBookingInfo.Text = $"Booking #{booking.BookingID} - Client: {booking.ClientDNI}, Room: {booking.RoomID}";


            }
            else
            {
                _selectedBookingId = 0;
                LblBookingInfo.Text = "(none selected)";
            }
        }

        private void BtnCheckIn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBookingId == 0)
            {
                MessageBox.Show("⚠️ Please select a reservation first.");
                return;
            }

            // Confirmación antes del check-in
            var result = MessageBox.Show(
                $"Are you sure you want to check in Booking #{_selectedBookingId}?",
                "Confirm Check-In",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                bool updated = _serviceManager.BookingService.ChangeBookingStatus(_selectedBookingId, "CheckedIn");

                if (updated)
                {
                    MessageBox.Show("✅ Check-in completed successfully!");
                    LoadPendingBookings(); // Refresh the grid
                    ClearSelection();
                }
                else
                {
                    MessageBox.Show("❌ Status could not be updated. Please try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error during check-in: {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
        }

        private void ClearSelection()
        {
            BookingsGrid.SelectedItem = null;
            _selectedBookingId = 0;
            LblBookingInfo.Text = "(none selected)";

        }


        public void RefreshBookings()
        {
            LoadPendingBookings();
            ClearSelection();
        }


    }
}