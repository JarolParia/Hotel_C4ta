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
    /// <summary>
    /// Lógica de interacción para DashBoard.xaml
    /// </summary>
    public partial class DashBoard : UserControl
    {
        private readonly ServiceManager _services;
        public DashBoard(ServiceManager serviceManager)
        {
            InitializeComponent();
            _services = serviceManager;
            LoadData();
        }
        private void LoadData()
        {
            int totalClientes = _services.ClientService.GetAllClients().Count;
            int totalHabitaciones = _services.RoomService.GetAllRooms().Count;
            int reservasActivas = _services.BookingService.GetCheckedInBookings().Count(b => b.BookingStatus == "CheckedIn");
            int reservasPendientes = _services.BookingService.GetPendingBookings().Count(b => b.BookingStatus == "Pending");
            int habitacionesOcupadas = _services.RoomService.GetAllRooms().Count(r => r.RoomStatus == "Occupied");
            int habitacionesDisponibles = totalHabitaciones - habitacionesOcupadas;
            decimal ingresosTotales = _services.BillService.GetAllBills().Sum(b => b.TotalAmount);

            var pagosRecientes = _services.PaymentService.GetAllPayments()
                .OrderByDescending(p => p.PaymentDate)
                .Take(5)
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
