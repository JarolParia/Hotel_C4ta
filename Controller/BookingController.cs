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
    }
}
