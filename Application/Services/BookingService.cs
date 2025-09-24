using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Application.Services
{
    /// Service layer for managing bookings.
    /// This class contains business logic and delegates data access to the repository layer.
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        /// Gets a specific booking by its ID.
        public Booking? GetBooking(int bookingId)
        {
            return _bookingRepository.GetBooking(bookingId);
        }

        /// Retrieves all bookings that are currently pending.
        public IEnumerable<Booking> GetPendingBookings()
        {
            return _bookingRepository.GetPendingBookings();
        }

        /// Retrieves all bookings that are currently checked in.
        public IEnumerable<Booking> GetCheckedInBookings()
        {
            return _bookingRepository.GetCheckedInBookings();
        }

        /// Registers a new booking with validation for correct date range.
        public bool RegisterBooking(DateTime start, DateTime end, string status, decimal price, string dni, int recid, int roomid)
        {
            if (end < start)
                throw new ArgumentException("End date cannot be before start date.");

            return _bookingRepository.RegisterBooking(start, end, status, price, dni, recid, roomid);
        }

        /// Updates the details of an existing booking.
        public bool UpdateBooking(int bookingId, DateTime start, DateTime end, decimal price, string status)
        {
            return _bookingRepository.UpdateBooking(bookingId, start, end, price, status);
        }

        public bool CancelBooking(int bookingId)
        {
            return _bookingRepository.SoftDeleteBooking(bookingId);
        }

        /// Changes the status of a booking 
        public bool ChangeBookingStatus(int bookingId, string newStatus)
        {
            return _bookingRepository.ChangeBookingStatus(bookingId, newStatus);
        }

        /// Retrieves only the bookings that are either Pending or Confirmed.
        public IEnumerable<Booking> GetPendingAndConfirmedBookings()
        {
            return _bookingRepository.GetBookingsByStatuses("Pending", "Confirmed");
        }
    }
}
