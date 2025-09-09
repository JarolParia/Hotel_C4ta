using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hotel_C4ta.Controller
{
    public class BookingController
    {
        public BookingModel _bookingModel;

        public List<BookingModel> GetPendingBookings()
        {
            var bookings = new List<BookingModel>();
            using (var conn = DBContext.OpenConnection())
            {
                if (conn == null) return bookings;
                try
                {
                    string sql = @"SELECT BookingID, StartDate, EndDate, BookingStatus, EstimatedPrice, ClientDNI, ReceptionistID, RoomID
                                   FROM Booking
                                   WHERE BookingStatus = 'Pending'";
                    using (var cmd = new SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bookings.Add(new BookingModel
                            {
                                BookingID = reader.GetInt32(0),
                                StartDate = reader.GetDateTime(1),
                                EndDate = reader.GetDateTime(2),
                                BookingStatus = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                EstimatedPrice = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                                ClientDNI = reader.GetString(5),
                                ReceptionistID = reader.GetInt32(6),
                                RoomID = reader.GetInt32(7)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading bookings: " + ex.Message);
                }
            }
            return bookings;
        }


        public bool RegisterBooking(DateTime start, DateTime end, string status, decimal price, string dni, int recid, int roomid)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = @"INSERT INTO Booking (StartDate, EndDate, BookingStatus, EstimatedPrice, ClientDNI, ReceptionistID, RoomID)
                                   VALUES (@sd, @ed, @st, @price, @dni, @recid, @roomid)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@sd", start);
                    cmd.Parameters.AddWithValue("@ed", end);
                    cmd.Parameters.AddWithValue("@st", status);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@dni", dni);
                    cmd.Parameters.AddWithValue("@recid", recid);
                    cmd.Parameters.AddWithValue("@roomid", roomid);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving booking: " + ex.Message);
            }
            return false;
        }

        public bool UpdateBooking(int bookingId, DateTime start, DateTime end, decimal price, string status)
        {
            using var conn = DBContext.OpenConnection();
            if (conn == null) return false;

            try
            {
                string sql = @"UPDATE Booking
                       SET StartDate = @sd,
                           EndDate = @ed,
                           EstimatedPrice = @price,
                           BookingStatus = @status
                       WHERE BookingID = @id";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@sd", start);
                    cmd.Parameters.AddWithValue("@ed", end);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@id", bookingId);
                    cmd.Parameters.AddWithValue("@status", status);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0; // true si se actualizó algo
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating booking: " + ex.Message);
                return false;
            }
        }

        public bool SoftDeleteBooking(int bookingId)
        {
            using var conn = DBContext.OpenConnection();
            if (conn == null) return false;

            try
            {
                string sql = @"UPDATE Booking 
                       SET BookingStatus = 'Cancelled'
                       WHERE BookingID = @id";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", bookingId);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0; // true if updated
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cancelling booking: " + ex.Message);
                return false;
            }
        }

        public bool ChangeBookingStatus(int bookingId, string newStatus)
        {
            using var conn = DBContext.OpenConnection();
            if (conn == null) return false;

            try
            {
                string sql = @"UPDATE Booking
                       SET BookingStatus = @status
                       WHERE BookingID = @id";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@id", bookingId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating booking status: " + ex.Message);
                return false;
            }


        }
    }
}
