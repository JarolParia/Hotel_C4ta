using Hotel_C4ta.Controller;
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
        private readonly BookingController _bookingcontroller= new BookingController();
        private int selectedBookingId = -1;
        public Search_UpdateBookingContent()
        {
            InitializeComponent();
            LoadRooms();
        }

        private void LoadRooms()
        {
            var booking = _bookingcontroller.GetPendingBookings();
            BookingsGrid.ItemsSource = booking;
        }

        // 🔹 Cancelar y limpiar formulario
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            selectedBookingId = -1;
            DpStartDate.SelectedDate = null;
            DpEndDate.SelectedDate = null;
            CmbStatus.SelectedIndex = -1;
            TxtEstimatedPrice.Text = "";
        }

        private void BookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookingsGrid.SelectedItem is BookingModel selectedBooking)
            {
                selectedBookingId = selectedBooking.BookingID;

                // Cargar datos en los campos
                TxtClient.Text = selectedBooking.ClientDNI;
                TxtRoom.Text = selectedBooking.RoomID.ToString();
                DpStartDate.SelectedDate = selectedBooking.StartDate;
                DpEndDate.SelectedDate = selectedBooking.EndDate;

                // Seleccionar estado en el ComboBox
                CmbStatus.SelectedItem = CmbStatus.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(i => i.Content.ToString() == selectedBooking.BookingStatus);

                TxtEstimatedPrice.Text = selectedBooking.EstimatedPrice.ToString("F2");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBookingId == -1)
            {
                MessageBox.Show("Please select a booking to update.");
                return;
            }

            if (DpStartDate.SelectedDate == null || DpEndDate.SelectedDate == null)
            {
                MessageBox.Show("Please select valid dates.");
                return;
            }

            DateTime start = DpStartDate.SelectedDate.Value;
            DateTime end = DpEndDate.SelectedDate.Value;

            if (end <= start)
            {
                MessageBox.Show("End date must be greater than start date.");
                return;
            }

            if (!decimal.TryParse(TxtEstimatedPrice.Text, out decimal price))
            {
                MessageBox.Show("Invalid estimated price.");
                return;
            }

            // Get status from ComboBox
            string status = (CmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";

            bool updated = _bookingcontroller.UpdateBooking(selectedBookingId, start, end, price, status);

            if (updated)
            {
                MessageBox.Show("Booking updated successfully.");
                LoadRooms(); // refresh the grid
            }
            else
            {
                MessageBox.Show("Failed to update booking.");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBookingId == -1)
            {
                MessageBox.Show("Please select a booking to delete.");
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this booking?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool cancelled = _bookingcontroller.SoftDeleteBooking(selectedBookingId);

                if (cancelled)
                {
                    MessageBox.Show("Booking deleted successfully.");
                    LoadRooms(); // Refresh grid
                    BtnCancel_Click(null, null); // Clear form
                }
                else
                {
                    MessageBox.Show("Unable to delete the booking.");
                }
            }
        }



    }
}
