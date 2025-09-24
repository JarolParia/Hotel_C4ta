using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Application.Services
{
    /// The ServiceManager acts as a container (or façade) for all the application services.
    /// Instead of creating or injecting each service separately everywhere,
    /// this manager provides a single access point to them.
    public class ServiceManager
    {
        // Properties to access each of the domain-related services
        public UserService UserService { get; }
       public RoomService RoomService { get; }

        public ClientService ClientService { get; }

        public BillService BillService { get; }

        public PaymentService PaymentService { get; }

        public BookingService BookingService { get; }

        // Each service is validated to ensure it is not null, preventing runtime errors.
        public ServiceManager(UserService userService, RoomService roomService, ClientService clientService, BillService billService, PaymentService paymentService, BookingService bookingService)
        {
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
            RoomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
            ClientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            BillService = billService ?? throw new ArgumentNullException(nameof(billService));
            PaymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
            BookingService = bookingService;
        }
    }
}
