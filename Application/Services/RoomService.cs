using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Application.Services
{
    /// Service layer responsible for managing operations related to hotel rooms.
    public class RoomService
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        }

        // Retrieves a single room by its unique identifier.
        public Room GetRoom(int roomId) => _roomRepository.GetRoom(roomId);

        // Retrieves a list of all rooms in the system 
        public List<Room> GetAllRooms() => _roomRepository.GetAllRooms();

        // Retrieves only the rooms that are currently available 
        public List<Room> GetAllAvailableRooms() => _roomRepository.GetAllAvailableRooms();

        // Creates a new room in the system.
        // Before creating, checks if a room with the same ID already exists.
        // If it exists, throws an exception to enforce the business rule of unique room IDs.
        public void CreateRoom(int roomId, int roomFloor, string roomStatus, string roomType, int capacity, decimal basePrice)
        {
            if (_roomRepository.RoomExists(roomId))
            {
                throw new InvalidOperationException($"Ya existe una habitación con ID {roomId}");
            }

            _roomRepository.CreateRoom(roomId, roomFloor, roomStatus, roomType, capacity, basePrice);
        }

        // Updates an existing room’s details (status, capacity, base price).
        // First validates that the room exists; if not, throws an exception.
        public void UpdateRoom(int roomId, string roomStatus, int capacity, decimal basePrice)
        {
            if (!_roomRepository.RoomExists(roomId))
            {
                throw new InvalidOperationException($"No existe una habitación con ID {roomId}");
            }

            _roomRepository.UpdateRoom(roomId, roomStatus, capacity, basePrice);
        }

        // Updates only the status of a room 
        public void UpdateStatusRoom(int roomId, string status) => _roomRepository.UpdateStatusRoom(roomId, status);

        // Deletes a room by ID
        public void DeleteRoom(int roomId) => _roomRepository.DeleteRoom(roomId);

        // Checks whether a room exists in the system by its ID.
        public bool RoomExists(int roomId) => _roomRepository.RoomExists(roomId);
    }
}
