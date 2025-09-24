using Hotel_C4ta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Repositories
{
    public interface IBookingRepository
    {
        Booking? GetBooking(int bookingId);
        IEnumerable<Booking> GetPendingBookings();
        IEnumerable<Booking> GetCheckedInBookings();
        bool RegisterBooking(DateTime start, DateTime end, string status, decimal price, string dni, int recid, int roomid);
        bool UpdateBooking(int bookingId, DateTime start, DateTime end, decimal price, string status);
        bool SoftDeleteBooking(int bookingId);
        bool ChangeBookingStatus(int bookingId, string newStatus);
        IEnumerable<Booking> GetBookingsByStatuses(params string[] statuses);
    }
}
