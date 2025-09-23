using Hotel_C4ta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Repositories
{
    public interface IRoomRepository
    {
        Room GetRoom(int roomId);
        List<Room> GetAllRooms();
        List<Room> GetAllAvailableRooms();

        void CreateRoom(int roomId, int roomFloor, string roomStatus, string roomType, int capacity, decimal basePrice);
        void UpdateRoom(int roomId, string roomStatus, int capacity, decimal basePrice);
        void UpdateStatusRoom(int roomId, string status);
        void DeleteRoom(int roomId);
        bool RoomExists(int roomId);
    }
}
