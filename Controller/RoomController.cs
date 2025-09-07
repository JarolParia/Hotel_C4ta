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
        //UPDATE ROOM -- admin
        //DELETE ROOM -- admin
        /*public void DeleteClient(string dni)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "DELETE FROM Client WHERE DNI=@dni";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@dni", dni);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Client deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting client: " + ex.Message);
            }
        }
        */
    }
}
