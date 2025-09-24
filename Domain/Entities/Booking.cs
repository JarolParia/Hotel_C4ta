using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Entities
{
    /// Represents a hotel booking (reservation) made by a client.
    public class Booking
    {
        public int BookingID { get; set; } /// Unique identifier of the booking.
        public DateTime StartDate { get; set; } /// Start date of the booking (check-in date).
        public DateTime EndDate { get; set; } /// End date of the booking (check-out date).
        public string BookingStatus { get; set; } /// Current status of the booking (e.g., Pending, Confirmed, Cancelled).
        public decimal EstimatedPrice { get; set; } /// Estimated total price of the booking.
        public string ClientDNI { get; set; } /// DNI of the client associated with the booking.
        public int ReceptionistID { get; set; }  /// Identifier of the receptionist who created the booking.
        public int RoomID { get; set; } /// Identifier of the reserved room.
    }
}
