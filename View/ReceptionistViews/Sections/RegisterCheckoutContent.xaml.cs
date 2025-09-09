using Hotel_C4ta.Controller;
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
    public partial class RegisterCheckOutContent : UserControl
    {
        private readonly BookingController _bookingController = new();
        private readonly RoomController _roomController = new();
        private readonly BillController _billController = new();
        private readonly PaymentController _paymentController = new();
        private readonly ClientController _clientController = new();

        private int _selectedBookingID = 0;
        private decimal _selectedEstimatedPrice = 0;
        private int _selectedRoomID = 0;

        public RegisterCheckOutContent()
        {
            InitializeComponent();
            GlobalFontSettings.FontResolver ??= new CustomFontResolver();

            LoadCheckedInBookings();
            
        }

        private void LoadCheckedInBookings()
        {
            var bookings = _bookingController.GetCheckedInBookings();
            BookingsGrid.ItemsSource = bookings;
        }

        private void BookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookingsGrid.SelectedItem is BookingModel booking)
            {
                _selectedBookingID = booking.BookingID;
                _selectedEstimatedPrice = booking.EstimatedPrice;

                TxtAmount.Text = booking.EstimatedPrice.ToString("F2");
                LblBookingInfo.Text = $"Booking #{booking.BookingID} - Room {booking.RoomID}";

                _selectedRoomID = booking.RoomID;
            }
        }

        private void BtnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBookingID == 0)
            {
                MessageBox.Show("Select a reservation to check out.");
                return;
            }

            if (string.IsNullOrEmpty(CmbMethod.Text))
            {
                MessageBox.Show("Select a payment method.");
                return;
            }

            //Obtener booking
            var booking = _bookingController.GetBooking(_selectedBookingID);           
            //Actualizar booking a CheckedOut
            var updated = _bookingController.ChangeBookingStatus(booking.BookingID, "CheckedOut");
            //Verificar que booking se haya actualizado a CheckedOut
            if (updated)
            {
                MessageBox.Show("✅ Check-out successful.");
                //LoadCheckedInBookings();
                //LblBookingInfo.Text = "(none)";
            }
            else
            {
                MessageBox.Show("⚠️ Status could not be updated.");
            }
            //Obtener número de habitación
            var roomId = booking.RoomID;
            //Obtener habitación
            var room = _roomController.GetRoom(roomId);
            //Actualizar habitación a Disponible
            _roomController.UpdateStatusRoom(room.RoomID, "Available");
            //Capturar el monto total
            var totalAmount = Convert.ToDecimal(TxtAmount.Text);
            //Generar factura y recuperar su id
            var billId = _billController.RegisterBill(totalAmount, booking.BookingID);
            //Recuperar factura
            var bill = _billController.GetBill(billId);
            //Capturar método de pago
            var paymentMethod = CmbMethod.Text;
            //Generar pago y recuperar su id
            var paymentId = _paymentController.RegisterPayment(totalAmount, paymentMethod, billId);
            //Recuperar datos para la factura
            string clientDni = booking.ClientDNI;
            //Recuperar cliente para obtener su nombre
            var client = _clientController.GetClient(clientDni);
            //Recuperar nombre del cliente
            string clientName = client.FullName;
            //Capturar fechas de reserva (Inicio y Fin)
            DateTime startDate = booking.StartDate, endDate = booking.EndDate;
            // Generar factura en PDF 
            byte[] pdfBytes = BillGenerator.GenerateInvoice(
                clientName,
                clientDni,
                roomId,
                startDate,
                endDate,
                totalAmount,
                paymentMethod
            );
            //Guardar el PDF en la tabla Bill
            var pdfSaved = _billController.SavePDF(billId, pdfBytes);
            //Intentar abrir la factura PDF
            if (!pdfSaved)
            {
                return;
            }
            else
            {
                try
                {
                    BillGenerator.OpenInvoice(pdfBytes, billId);
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Bill generated and saved in DB, but PDF could not be opened.", ex.Message);
                }
            }  
            //Limpiar los campos una vez generada y cargada la factura PDF
            LoadCheckedInBookings();
            _selectedBookingID = 0;
            LblBookingInfo.Text = "(none)";
            TxtAmount.Text = "";
            CmbMethod.SelectedIndex = -1;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            BookingsGrid.SelectedItem = null;
            _selectedBookingID = 0;
            LblBookingInfo.Text = "(none)";
            TxtAmount.Text = "";
            CmbMethod.SelectedIndex = -1;
        }
    }
}
