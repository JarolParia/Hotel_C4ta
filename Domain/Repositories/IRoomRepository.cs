using Hotel_C4ta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Repositories
{
    /// Defines the contract for retrieving, creating, updating, and deleting rooms.
    public interface IRoomRepository
    {
        Room GetRoom(int roomId); /// Recupera una habitación específica por su identificador único.
        List<Room> GetAllRooms(); /// Retrieves all rooms
        List<Room> GetAllAvailableRooms(); /// Retrieves only the available rooms 
        void CreateRoom(int roomId, int roomFloor, string roomStatus, string roomType, int capacity, decimal basePrice); /// Creates a new room with the provided details.
        void UpdateRoom(int roomId, string roomStatus, int capacity, decimal basePrice); /// Updates the details of an existing room.
        void UpdateStatusRoom(int roomId, string status); /// Updates only the status of a specific room.
        void DeleteRoom(int roomId); /// Deletes a room from the system using its unique identifier.
        bool RoomExists(int roomId); /// Checks whether a room with the given ID exists in the system.
    }
}
