using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Application.Services
{
    public class ServiceManager
    {
        public UserService UserService { get; }
       public RoomService RoomService { get; }

        public ClientService ClientService { get; }

        public BillService BillService { get; }

        public PaymentService PaymentService { get; }

        public BookingService BookingService { get; }
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
