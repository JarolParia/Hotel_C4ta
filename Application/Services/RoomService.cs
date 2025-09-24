using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Application.Services
{
    public class RoomService
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        }

        public Room GetRoom(int roomId) => _roomRepository.GetRoom(roomId);
        public List<Room> GetAllRooms() => _roomRepository.GetAllRooms();
        public List<Room> GetAllAvailableRooms() => _roomRepository.GetAllAvailableRooms();

        public void CreateRoom(int roomId, int roomFloor, string roomStatus, string roomType, int capacity, decimal basePrice)
        {
            if (_roomRepository.RoomExists(roomId))
            {
                throw new InvalidOperationException($"Ya existe una habitación con ID {roomId}");
            }

            _roomRepository.CreateRoom(roomId, roomFloor, roomStatus, roomType, capacity, basePrice);
        }

        public void UpdateRoom(int roomId, string roomStatus, int capacity, decimal basePrice)
        {
            if (!_roomRepository.RoomExists(roomId))
            {
                throw new InvalidOperationException($"No existe una habitación con ID {roomId}");
            }

            _roomRepository.UpdateRoom(roomId, roomStatus, capacity, basePrice);
        }

        public void UpdateStatusRoom(int roomId, string status) => _roomRepository.UpdateStatusRoom(roomId, status);
        public void DeleteRoom(int roomId) => _roomRepository.DeleteRoom(roomId);
        public bool RoomExists(int roomId) => _roomRepository.RoomExists(roomId);
    }
}
