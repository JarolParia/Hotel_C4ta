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
    /// Repository class for managing bookings in the database.
    /// Handles CRUD operations and queries related to the Booking table.
    public class BookingRepository : IBookingRepository
    {
        private readonly DBContext _context; /// Database context for opening connections.

        public BookingRepository(DBContext context)
        {
            _context = context;
        }

        /// Retrieves a single booking by its unique ID.
        /// Returns null if no booking is found.
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

        /// Retrieves all bookings with "Pending" status.
        public IEnumerable<Booking> GetPendingBookings()
        {
            return GetBookingsByStatus("Pending");
        }

        /// Retrieves all bookings with "CheckedIn" status.
        public IEnumerable<Booking> GetCheckedInBookings()
        {
            return GetBookingsByStatus("CheckedIn");
        }

        /// Inserts a new booking into the database.
        /// Returns true if at least one row was inserted.
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

        /// Updates an existing booking with new details (dates, price, status).
        /// Returns true if at least one row was updated.
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

        /// Performs a "soft delete" by marking a booking as "Cancelled" instead of physically removing it from the database.
        public bool SoftDeleteBooking(int bookingId)
        {
            using var conn = _context.OpenConnection();
            string sql = @"UPDATE Booking SET BookingStatus = 'Cancelled' WHERE BookingID = @id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", bookingId);

            return cmd.ExecuteNonQuery() > 0;
        }

        /// Performs a "soft delete" by marking a booking as "Cancelled" instead of physically removing it from the database.
        public bool ChangeBookingStatus(int bookingId, string newStatus)
        {
            using var conn = _context.OpenConnection();
            string sql = @"UPDATE Booking SET BookingStatus = @status WHERE BookingID = @id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@status", newStatus);
            cmd.Parameters.AddWithValue("@id", bookingId);

            return cmd.ExecuteNonQuery() > 0;
        }

        // Helper method to convert a SqlDataReader row into a Booking object.
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

        /// Retrieves bookings filtered by a specific status
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

        /// Retrieves bookings that match any of the provided statuses.
        public IEnumerable<Booking> GetBookingsByStatuses(params string[] statuses)
        {
            var bookings = new List<Booking>();

            using var conn = _context.OpenConnection();

            // Creamos parámetros dinámicos: @s0, @s1, @s2...
            var parameters = statuses.Select((s, i) => $"@s{i}").ToArray();
            string sql = $"SELECT * FROM Booking WHERE BookingStatus IN ({string.Join(",", parameters)})";

            using var cmd = new SqlCommand(sql, conn);

            for (int i = 0; i < statuses.Length; i++)
            {
                cmd.Parameters.AddWithValue($"@s{i}", statuses[i]);
            }

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                bookings.Add(MapBooking(reader));
            }

            return bookings;
        }
    }
}
