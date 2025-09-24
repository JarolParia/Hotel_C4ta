using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Entities
{
    public class Room
    {
        public int RoomID { get; set; }
        public int RoomFloor { get; set; }
        public string RoomStatus { get; set; } = "Available";
        public string RoomType { get; set; }
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
    }
}
