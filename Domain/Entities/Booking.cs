using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Entities
{
    public class Booking
    {
        public int BookingID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BookingStatus { get; set; }
        public decimal EstimatedPrice { get; set; }
        public string ClientDNI { get; set; }
        public int ReceptionistID { get; set; }
        public int RoomID { get; set; }
    }
}
