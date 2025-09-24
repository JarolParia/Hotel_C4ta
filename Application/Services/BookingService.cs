using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Application.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public Booking? GetBooking(int bookingId)
        {
            return _bookingRepository.GetBooking(bookingId);
        }

        public IEnumerable<Booking> GetPendingBookings()
        {
            return _bookingRepository.GetPendingBookings();
        }

        public IEnumerable<Booking> GetCheckedInBookings()
        {
            return _bookingRepository.GetCheckedInBookings();
        }

        public bool RegisterBooking(DateTime start, DateTime end, string status, decimal price, string dni, int recid, int roomid)
        {
            if (end < start)
                throw new ArgumentException("End date cannot be before start date.");

            return _bookingRepository.RegisterBooking(start, end, status, price, dni, recid, roomid);
        }

        public bool UpdateBooking(int bookingId, DateTime start, DateTime end, decimal price, string status)
        {
            return _bookingRepository.UpdateBooking(bookingId, start, end, price, status);
        }

        public bool CancelBooking(int bookingId)
        {
            return _bookingRepository.SoftDeleteBooking(bookingId);
        }

        public bool ChangeBookingStatus(int bookingId, string newStatus)
        {
            return _bookingRepository.ChangeBookingStatus(bookingId, newStatus);
        }
        public IEnumerable<Booking> GetPendingAndConfirmedBookings()
        {
            return _bookingRepository.GetBookingsByStatuses("Pending", "Confirmed");
        }
    }
}
