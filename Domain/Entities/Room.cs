using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Entities
{
    /// Represents a hotel room that can be reserved by clients.
    public class Room
    {
        public int RoomID { get; set; } /// Unique identifier of the room.
        public int RoomFloor { get; set; } /// Floor number where the room is located.
        public string RoomStatus { get; set; } = "Available"; /// Current status of the room. Defaults to "Available"
        public string RoomType { get; set; } /// Type of the room 
        public int Capacity { get; set; } /// Maximum number of guests the room can accommodate.
        public decimal BasePrice { get; set; } /// Base daily price for the room
    }
}
