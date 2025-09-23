using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using Hotel_C4ta.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DBContext _context;

        public BookingRepository(DBContext context)
        {
            _context = context;
        }

        public Booking? GetBooking(int bookingId)
        {
            using var conn = _context.OpenConnection();
            string sql = "SELECT * FROM Booking WHERE BookingID=@bookingId";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@bookingId", bookingId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapBooking(reader);
            }
            return null;
        }

        public IEnumerable<Booking> GetPendingBookings()
        {
            return GetBookingsByStatus("Pending");
        }

        public IEnumerable<Booking> GetCheckedInBookings()
        {
            return GetBookingsByStatus("CheckedIn");
        }

        public bool RegisterBooking(DateTime start, DateTime end, string status, decimal price, string dni, int recid, int roomid)
        {
            using var conn = _context.OpenConnection();
            string sql = @"INSERT INTO Booking (StartDate, EndDate, BookingStatus, EstimatedPrice, ClientDNI, ReceptionistID, RoomID)
                          VALUES (@sd, @ed, @st, @price, @dni, @recid, @roomid)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sd", start);
            cmd.Parameters.AddWithValue("@ed", end);
            cmd.Parameters.AddWithValue("@st", status);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@dni", dni);
            cmd.Parameters.AddWithValue("@recid", recid);
            cmd.Parameters.AddWithValue("@roomid", roomid);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool UpdateBooking(int bookingId, DateTime start, DateTime end, decimal price, string status)
        {
            using var conn = _context.OpenConnection();
            string sql = @"UPDATE Booking
                          SET StartDate = @sd,
                              EndDate = @ed,
                              EstimatedPrice = @price,
                              BookingStatus = @status
                          WHERE BookingID = @id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sd", start);
            cmd.Parameters.AddWithValue("@ed", end);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@id", bookingId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool SoftDeleteBooking(int bookingId)
        {
            using var conn = _context.OpenConnection();
            string sql = @"UPDATE Booking SET BookingStatus = 'Cancelled' WHERE BookingID = @id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", bookingId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool ChangeBookingStatus(int bookingId, string newStatus)
        {
            using var conn = _context.OpenConnection();
            string sql = @"UPDATE Booking SET BookingStatus = @status WHERE BookingID = @id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@status", newStatus);
            cmd.Parameters.AddWithValue("@id", bookingId);

            return cmd.ExecuteNonQuery() > 0;
        }

        // 🔹 Helper
        private Booking MapBooking(SqlDataReader reader)
        {
            return new Booking
            {
                BookingID = reader.GetInt32(0),
                StartDate = reader.GetDateTime(1),
                EndDate = reader.GetDateTime(2),
                BookingStatus = reader.IsDBNull(3) ? "" : reader.GetString(3),
                EstimatedPrice = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                ClientDNI = reader.GetString(5),
                ReceptionistID = reader.GetInt32(6),
                RoomID = reader.GetInt32(7)
            };
        }

        private IEnumerable<Booking> GetBookingsByStatus(string status)
        {
            var bookings = new List<Booking>();
            using var conn = _context.OpenConnection();
            string sql = @"SELECT * FROM Booking WHERE BookingStatus = @status";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@status", status);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                bookings.Add(MapBooking(reader));
            }
            return bookings;
        }
    }
}
