using Hotel_C4ta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Repositories
{
    /// Defines the contract for creating, updating, retrieving, and managing bookings in the system.
    public interface IBookingRepository
    {
        Booking? GetBooking(int bookingId);  /// Retrieves a specific booking by its unique identifier.
        IEnumerable<Booking> GetPendingBookings(); /// Retrieves all bookings that are pending (not yet checked-in).
        IEnumerable<Booking> GetCheckedInBookings(); /// Retrieves all bookings that are currently checked-in.
        bool RegisterBooking(DateTime start, DateTime end, string status, decimal price, string dni, int recid, int roomid); /// Registers (creates) a new booking in the system.
        bool UpdateBooking(int bookingId, DateTime start, DateTime end, decimal price, string status); /// Updates an existing booking with new details.
        bool SoftDeleteBooking(int bookingId); /// Performs a soft delete of a booking. The booking is marked as inactive rather than being permanently removed.
        bool ChangeBookingStatus(int bookingId, string newStatus); /// Changes the status of a booking 
        IEnumerable<Booking> GetBookingsByStatuses(params string[] statuses); /// Retrieves bookings that match one or more statuses.
    }
}
