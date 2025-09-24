using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using Hotel_C4ta.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hotel_C4ta.Infrastructure.Repositories
{
    /// Repository implementation for Room entity.
    /// Handles all CRUD operations (Create, Read, Update, Delete) with the database.
    internal class RoomRepository : IRoomRepository
    {
        private readonly DBContext _dbContext;

        public RoomRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// GET a single room by ID
        public Room GetRoom(int roomId)
        {
            using var conn = _dbContext.OpenConnection();
            try
            {
                string sql = "SELECT RoomID, RoomFloor, RoomStatus, RoomType, Capacity, BasePrice FROM Room WHERE RoomID=@roomId";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@roomId", roomId);
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Room
                    {
                        RoomID = reader.GetInt32("RoomID"),
                        RoomFloor = reader.GetInt32("RoomFloor"),
                        RoomStatus = reader.GetString("RoomStatus"),
                        RoomType = reader.GetString("RoomType"),
                        Capacity = reader.GetInt32("Capacity"),
                        BasePrice = reader.GetDecimal("BasePrice")
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading room: " + ex.Message);
            }
            return null; /// Return null if not found or error
        }

        public List<Room> GetAllRooms()
        {
            using var conn = _dbContext.OpenConnection();
            var rooms = new List<Room>();
            try
            {
                string sql = "SELECT RoomID, RoomFloor, RoomStatus, RoomType, Capacity, BasePrice FROM Room";
                using var cmd = new SqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    rooms.Add(new Room
                    {
                        RoomID = reader.GetInt32("RoomID"),
                        RoomFloor = reader.GetInt32("RoomFloor"),
                        RoomStatus = reader.GetString("RoomStatus"),
                        RoomType = reader.GetString("RoomType"),
                        Capacity = reader.GetInt32("Capacity"),
                        BasePrice = reader.GetDecimal("BasePrice")
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading rooms: " + ex.Message);
            }
            return rooms;
        }

        /// GET all available rooms
        public List<Room> GetAllAvailableRooms()
        {
            using var conn = _dbContext.OpenConnection();
            var rooms = new List<Room>();
            try
            {
                string sql = "SELECT RoomID, RoomFloor, RoomStatus, RoomType, Capacity, BasePrice FROM Room WHERE RoomStatus='Available'";
                using var cmd = new SqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    rooms.Add(new Room
                    {
                        RoomID = reader.GetInt32("RoomID"),
                        RoomFloor = reader.GetInt32("RoomFloor"),
                        RoomStatus = reader.GetString("RoomStatus"),
                        RoomType = reader.GetString("RoomType"),
                        Capacity = reader.GetInt32("Capacity"),
                        BasePrice = reader.GetDecimal("BasePrice")
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading available rooms: " + ex.Message);
            }
            return rooms;
        }

        /// CREATE a new room
        public void CreateRoom(int roomId, int roomFloor, string roomStatus, string roomType, int capacity, decimal basePrice)
        {
            using var conn = _dbContext.OpenConnection();
            try
            {
                string sql = "INSERT INTO Room (RoomID, RoomFloor, RoomStatus, RoomType, Capacity, BasePrice) VALUES (@roomID, @roomFloor, @roomStatus, @roomType, @capacity, @basePrice)";
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@roomID", roomId);
                cmd.Parameters.AddWithValue("@roomFloor", roomFloor);
                cmd.Parameters.AddWithValue("@roomStatus", roomStatus);
                cmd.Parameters.AddWithValue("@roomType", roomType);
                cmd.Parameters.AddWithValue("@capacity", capacity);
                cmd.Parameters.AddWithValue("@basePrice", basePrice);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Room created successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating room: " + ex.Message);
            }
        }

        /// UPDATE room info (status, capacity, base price)
        public void UpdateRoom(int roomId, string roomStatus, int capacity, decimal basePrice)
        {
            using var conn = _dbContext.OpenConnection();
            try
            {
                string sql = "UPDATE Room SET RoomStatus=@roomStatus, Capacity=@capacity, BasePrice=@basePrice WHERE RoomID=@roomId";
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@roomStatus", roomStatus);
                cmd.Parameters.AddWithValue("@capacity", capacity);
                cmd.Parameters.AddWithValue("@basePrice", basePrice);
                cmd.Parameters.AddWithValue("@roomId", roomId);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Room updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating room: " + ex.Message);
            }
        }

        /// UPDATE only the status of a room
        public void UpdateStatusRoom(int roomId, string status)
        {
            using var conn = _dbContext.OpenConnection();
            try
            {
                string sql = "UPDATE Room SET RoomStatus=@status WHERE RoomID=@roomId";
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@roomId", roomId);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating room status: " + ex.Message);
            }
        }

        /// DELETE a room
        public void DeleteRoom(int roomId)
        {
            using var conn = _dbContext.OpenConnection();
            try
            {
                string sql = "DELETE FROM Room WHERE RoomID=@roomId";
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@roomId", roomId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Room deleted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting room: " + ex.Message);
            }
        }

        /// CHECK if a room exists (returns true or false)
        public bool RoomExists(int roomId)
        {
            using var conn = _dbContext.OpenConnection();
            try
            {
                string sql = "SELECT COUNT(*) FROM Room WHERE RoomID=@roomId";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@roomId", roomId);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking if room exists: " + ex.Message);
                return false;
            }
        }
    }
}
