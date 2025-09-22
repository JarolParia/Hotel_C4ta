using Hotel_C4ta.Controller;
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

namespace Hotel_C4ta.View
{
    /// <summary>
    /// Lógica de interacción para Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {

        private readonly ClientController _clientController = new ClientController();
        private readonly RoomController _roomController = new RoomController();
        private readonly BookingController _bookingController = new BookingController();
        private readonly BillController _billController = new BillController();
        private readonly PaymentController _paymentController = new PaymentController();
        public Dashboard()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Clientes y habitaciones
            int totalClientes = _clientController.GetAllClients().Count;
            int totalHabitaciones = _roomController.GetAllRooms().Count;
            int reservasActivas = _bookingController.GetCheckedInBookings().Count(b => b.BookingStatus == "CheckedIn");

            // Reservas pendientes
            int reservasPendientes = _bookingController.GetPendingBookings().Count(b => b.BookingStatus == "Pending");

            // Habitaciones ocupadas vs disponibles
            int habitacionesOcupadas = _roomController.GetAllRooms().Count(r => r.RoomStatus == "Occupied");
            int habitacionesDisponibles = totalHabitaciones - habitacionesOcupadas;

            // Ingresos Totales
            decimal ingresosTotales = _billController.GetAllBills().Sum(b => b.TotalAmount);

            // Pagos recientes (TOP 5 ordenados por fecha descendente)
            var pagosRecientes = _paymentController.GetAllPayments()
                                    .OrderByDescending(p => p.PaymentDate)
                                    .Take(5)
                                    .Select(p => new
                                    {
                                        PaymentDate = p.PaymentDate.ToShortDateString(),
                                        Amount = p.Amount.ToString("C"),
                                        PaymentMethod = p.PaymentMethod
                                    }).ToList();

            // Asignar a UI
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
