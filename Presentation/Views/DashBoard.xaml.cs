using Hotel_C4ta.Application.Services;
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

namespace Hotel_C4ta.Presentation.Views
{
    /// This UserControl displays general statistics and recent data for the hotel system.
    public partial class DashBoard : UserControl
    {
        private readonly ServiceManager _services;
        
        /// Initializes the UI and immediately loads the dashboard data.
        public DashBoard(ServiceManager serviceManager)
        {
            InitializeComponent();
            _services = serviceManager;
            LoadData(); /// Load data as soon as the dashboard is created
        }

        /// Loads statistics and recent activity into the dashboard
        private void LoadData()
        {
            /// Count total clients
            int totalClientes = _services.ClientService.GetAllClients().Count;

            /// Count total rooms
            int totalHabitaciones = _services.RoomService.GetAllRooms().Count;

            /// Count active bookings (checked in)
            int reservasActivas = _services.BookingService.GetCheckedInBookings().Count(b => b.BookingStatus == "CheckedIn");

            /// Count pending bookings
            int reservasPendientes = _services.BookingService.GetPendingBookings().Count(b => b.BookingStatus == "Pending");

            /// Count occupied rooms
            int habitacionesOcupadas = _services.RoomService.GetAllRooms().Count(r => r.RoomStatus == "Occupied");

            /// Calculate available rooms
            int habitacionesDisponibles = totalHabitaciones - habitacionesOcupadas;

            /// Calculate total income (sum of all bills)
            decimal ingresosTotales = _services.BillService.GetAllBills().Sum(b => b.TotalAmount);

            /// Get the 5 most recent payments
            var pagosRecientes = _services.PaymentService.GetAllPayments()
                .OrderByDescending(p => p.PaymentDate) /// Order by most recent
                .Take(5) /// Only take top 5
                .Select(p => new
                {
                    PaymentDate = p.PaymentDate.ToShortDateString(),
                    Amount = p.Amount.ToString("C"),
                    PaymentMethod = p.PaymentMethod
                }).ToList();

            ClientsText.Text = totalClientes.ToString();
            RoomsText.Text = totalHabitaciones.ToString();
            Active_BookingsText.Text = reservasActivas.ToString();
            Pending_BookingsText.Text = reservasPendientes.ToString();
            OccupiedRoomsText.Text = habitacionesOcupadas.ToString();
            AvailibleRoomsText.Text = $"Disponibles: {habitacionesDisponibles}";
            Total_incomeText.Text = ingresosTotales.ToString("C");

            LastPaysGrid.ItemsSource = pagosRecientes;
        }
    }
}
