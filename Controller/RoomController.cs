using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hotel_C4ta.Controller
{
    public class RoomController
    {
        public RoomModel _roomModel;

        public RoomModel GetRoom(int roomId)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "SELECT * FROM Room WHERE RoomID=@roomId";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@roomId", roomId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    return new RoomModel
                    {
                        RoomID = reader.GetInt32(0),
                        RoomFloor = reader.GetInt32(1),
                        RoomStatus = reader.GetString(2),
                        RoomType = reader.GetString(3),
                        Capacity = reader.GetInt32(4),
                        BasePrice = reader.GetDecimal(5)
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading room: " + ex.Message);
            }
            return null;
        }
        public List<RoomModel> GetAllRooms()
        {
            using var conn = DBContext.OpenConnection();
            var rooms = new List<RoomModel>();

            try
            {
                string sql = "SELECT RoomID, RoomFloor, RoomStatus, RoomType, Capacity, BasePrice FROM Room";
                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rooms.Add(new RoomModel
                        {
                            RoomID = reader.GetInt32(0),
                            RoomFloor = reader.GetInt32(1),
                            RoomStatus = reader.GetString(2),
                            RoomType = reader.GetString(3),
                            Capacity = reader.GetInt32(4),
                            BasePrice = reader.GetDecimal(5),
                        });
                    }
                    return rooms;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading rooms: " + ex.Message);
            }
            return null;
        }

        public List<RoomModel> GetAllAvailableRooms()
        {
            using var conn = DBContext.OpenConnection();
            var rooms = new List<RoomModel>();

            try
            {
                string sql = "SELECT RoomID, RoomFloor, RoomStatus, RoomType, Capacity, BasePrice FROM Room WHERE RoomStatus='Available'";
                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rooms.Add(new RoomModel
                        {
                            RoomID = reader.GetInt32(0),
                            RoomFloor = reader.GetInt32(1),
                            RoomStatus = reader.GetString(2),
                            RoomType = reader.GetString(3),
                            Capacity = reader.GetInt32(4),
                            BasePrice = reader.GetDecimal(5),
                        });
                    }
                    return rooms;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading rooms: " + ex.Message);
            }
            return null;
        }
        //
        public void UpdateStatusRoom(int roomid, string status)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "UPDATE Room SET RoomStatus=@status WHERE RoomID=@roomid";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@roomid", roomid);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating room status: " + ex.Message);
            }
        }

        //CREATE ROOM -- admin
        public void CreateRoom(int roomid,int roomFloor, string roomStatus, string roomType, int capacity, decimal basePrice)
        {
            using var conn = DBContext.OpenConnection();
            try
            {
                string sql = "INSERT INTO Room (RoomID,RoomFloor, RoomStatus, RoomType, Capacity, BasePrice) VALUES (@roomID, @roomFloor, @roomStatus, @roomType, @capacity, @basePrice)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@roomID", roomid);
                    cmd.Parameters.AddWithValue("@roomFloor", roomFloor);
                    cmd.Parameters.AddWithValue("@roomStatus", roomStatus);
                    cmd.Parameters.AddWithValue("@roomType", roomType);
                    cmd.Parameters.AddWithValue("@capacity", capacity);
                    cmd.Parameters.AddWithValue("@basePrice", basePrice);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Room created successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating room: " + ex.Message);
            }
        }

        
        //UPDATE ROOM -- admin

        public void UpdateRoom(int roomid, string roomStatus, int capacity, decimal basePrice)
        {
            using var conn = DBContext.OpenConnection();
            try
            {
                string sql = "UPDATE Room SET  RoomStatus=@roomStatus, Capacity=@capacity, BasePrice=@basePrice WHERE RoomID=@roomid";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@roomStatus", roomStatus);
                    cmd.Parameters.AddWithValue("@capacity", capacity);
                    cmd.Parameters.AddWithValue("@basePrice", basePrice);
                    cmd.Parameters.AddWithValue("@roomid", roomid);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Room updated successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating Room: " + ex.Message);
            }
        }

        //DELETE ROOM -- admin
        public void DeleteRoom(int roomid)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "DELETE FROM Room WHERE RoomID=@roomid ";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@roomid", roomid);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Room deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting Room: " + ex.Message);
            }
        }        
    }
}
// CRUD of rooms Melo