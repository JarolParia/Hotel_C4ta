using Hotel_C4ta.Application.Services;
using Hotel_C4ta.Infrastructure.Data;
using Hotel_C4ta.Infrastructure.Repositories;
using Hotel_C4ta.Presentation.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Hotel_C4ta
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["HotelC4TA"].ConnectionString;
                var dbConfig = new DatabaseConfig { ConnectionString = connString };

                var dbContext = new DBContext(dbConfig);

                if (!dbContext.TestConnection())
                {
                    MessageBox.Show("No se pudo conectar a la base de datos", "Error de Conexión",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    Shutdown();
                    return;
                }

                var userRepository = new UserRepository(dbContext);
                var roomRepository = new RoomRepository(dbContext);
                var userService = new UserService(userRepository);
                var roomService = new RoomService(roomRepository);
                var clientRepository = new ClientRepository(dbContext);
                var clientService = new ClientService(clientRepository);
                var billRepository = new BillRepository(dbContext);
                var billService = new BillService(billRepository);
                var paymentRepository = new PaymentRepository(dbContext);
                var paymentService = new PaymentService(paymentRepository);
                var bookingRepository = new BookingRepository(dbContext);
                var bookingService = new BookingService(bookingRepository);



                var services = new ServiceManager(userService, roomService, clientService, billService, paymentService, bookingService);

                var mainWindow = new MainWindow(services);
                mainWindow.Show();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar la aplicación: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
    }

}
