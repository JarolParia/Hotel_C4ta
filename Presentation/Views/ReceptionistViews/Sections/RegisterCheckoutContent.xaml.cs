using Hotel_C4ta.Application.Services;
using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Utils;
using PdfSharp.Fonts;
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

namespace Hotel_C4ta.Presentation.Views.ReceptionistViews.Sections
{
    /// <summary>
    /// Lógica de interacción para RegisterCheckoutContent.xaml
    /// </summary>
    public partial class RegisterCheckoutContent : UserControl
    {
        private readonly ServiceManager _serviceManager;
        private int _selectedBookingID = 0;
        private decimal _selectedEstimatedPrice = 0;
        private int _selectedRoomID = 0;
        public RegisterCheckoutContent(ServiceManager serviceManager)
        {
            InitializeComponent();
            GlobalFontSettings.FontResolver ??= new CustomFontResolver();
            _serviceManager = serviceManager;

            LoadCheckedInBookings();
        }

        private void LoadCheckedInBookings()
        {

            var bookings = _serviceManager.BookingService.GetCheckedInBookings();
            BookingsGrid.ItemsSource = bookings;
        }

        private void BookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookingsGrid.SelectedItem is Booking booking)
            {
                _selectedBookingID = booking.BookingID;
                _selectedEstimatedPrice = booking.EstimatedPrice;
                _selectedRoomID = booking.RoomID;

                TxtAmount.Text = booking.EstimatedPrice.ToString("F2");
                LblBookingInfo.Text = $"Booking #{booking.BookingID} - Room {booking.RoomID}";
            }
        }

        private void BtnCheckOut_Click(object sender, EventArgs e)
        {
            if (_selectedBookingID == 0)
            {
                MessageBox.Show("Please select a booking to check out.", "No Booking Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(CmbMethod.Text))
            {
                MessageBox.Show("Please select a payment method.", "No Payment Method", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            try
            {
                var booking = _serviceManager.BookingService.GetBooking(_selectedBookingID);

                bool updateSuccess = _serviceManager.BookingService.ChangeBookingStatus(_selectedBookingID, "CheckedOut");
                if (!updateSuccess)
                {
                    MessageBox.Show("Failed to update booking status.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _serviceManager.RoomService.UpdateStatusRoom(_selectedRoomID, "Available");

                decimal totalAmount = Convert.ToDecimal(_selectedEstimatedPrice);
                int billId = _serviceManager.BillService.RegisterBill(totalAmount, booking.BookingID);

                var bill = _serviceManager.BillService.GetBill(billId);

                string paymentMethod = CmbMethod.Text;
                _serviceManager.PaymentService.RegisterPayment(totalAmount, paymentMethod, billId);

                var client = _serviceManager.ClientService.GetClient(booking.ClientDNI);

                // Generate PDF receipt

                byte[] pdfBytes = BillGenerator.GenerateInvoice(
                    client.FullName,
                    client.DNI,
                    booking.RoomID,
                    booking.StartDate,
                    booking.EndDate,
                    totalAmount,
                    paymentMethod
                    );

                bool pdfSaved = _serviceManager.BillService.SavePDF(billId, pdfBytes);
                if (!pdfSaved)
                {
                    MessageBox.Show("Failed to save PDF receipt.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    BillGenerator.OpenInvoice(pdfBytes, billId);
                }

                MessageBox.Show("Checkout completed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadCheckedInBookings();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during checkout: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            BookingsGrid.SelectedItem = null;
            _selectedBookingID = 0;
            _selectedEstimatedPrice = 0;
            _selectedRoomID = 0;

            LblBookingInfo.Text = "(none)";
            TxtAmount.Text = "";
            CmbMethod.SelectedIndex = -1;
        }
    }
}