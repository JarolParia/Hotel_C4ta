using Hotel_C4ta.Application.Services;
using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace Hotel_C4ta.Presentation.Views.ReceptionistViews.Sections
{
    public partial class Search_UpdateBookingContent : UserControl
    {
        private readonly ServiceManager _serviceManager;
        private int selectedBookingId = -1;
        private DateTime _originalStartDate;
        private DateTime _originalEndDate;

        public Search_UpdateBookingContent(ServiceManager serviceManager)
        {
            InitializeComponent();
            _serviceManager = serviceManager;
            LoadBookings();
        }

        private void LoadBookings()
        {
            var bookings = _serviceManager.BookingService.GetPendingBookings();
            BookingsGrid.ItemsSource = bookings;
        }

        private void BookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookingsGrid.SelectedItem is Booking selectedBooking)
            {
                selectedBookingId = selectedBooking.BookingID;

                // Guardar fechas originales
                _originalStartDate = selectedBooking.StartDate;
                _originalEndDate = selectedBooking.EndDate;

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

        private void DateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DpStartDate.SelectedDate == null || DpEndDate.SelectedDate == null)
                return;

            var newStart = DpStartDate.SelectedDate.Value;
            var newEnd = DpEndDate.SelectedDate.Value;

            // Validaciones con las fechas originales
            if (newStart < _originalStartDate)
            {
                MessageBox.Show("La nueva fecha de inicio no puede ser menor a la original.");
                DpStartDate.SelectedDate = _originalStartDate;
                return;
            }

            if (newEnd < _originalEndDate)
            {
                MessageBox.Show("La nueva fecha de fin no puede ser menor a la original.");
                DpEndDate.SelectedDate = _originalEndDate;
                return;
            }

            if (newEnd <= newStart)
            {
                MessageBox.Show("La fecha de fin debe ser mayor que la de inicio.");
                return;
            }

            // Recalcular el precio estimado
            if (int.TryParse(TxtRoom.Text, out int roomId))
            {
                try
                {
                    var room = _serviceManager.RoomService.GetRoom(roomId);
                    if (room != null)
                    {
                        var estimatedPrice = CalculateBookingPrice.Calculate(room, newStart, newEnd);
                        TxtEstimatedPrice.Text = estimatedPrice.ToString("F2");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error calculating price: {ex.Message}");
                }
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

            try
            {
                bool updated = _serviceManager.BookingService.UpdateBooking(selectedBookingId, start, end, price, status);

                if (updated)
                {
                    MessageBox.Show("✅ Booking updated successfully!");
                    LoadBookings(); // refresh the grid
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("❌ Failed to update booking.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error updating booking: {ex.Message}");
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
                "Are you sure you want to cancel this booking?",
                "Confirm Cancellation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    bool cancelled = _serviceManager.BookingService.CancelBooking(selectedBookingId);

                    if (cancelled)
                    {
                        MessageBox.Show("✅ Booking cancelled successfully!");
                        LoadBookings(); // Refresh grid
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("❌ Unable to cancel the booking.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Error cancelling booking: {ex.Message}");
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            selectedBookingId = -1;
            TxtClient.Text = "";
            TxtRoom.Text = "";
            DpStartDate.SelectedDate = null;
            DpEndDate.SelectedDate = null;
            CmbStatus.SelectedIndex = -1;
            TxtEstimatedPrice.Text = "";
            BookingsGrid.SelectedItem = null;
        }


    }
}