using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Model
{
    class RoomModel
    {
        public int RoomID { get; set; }
        public int Floor { get; set; }
        public string RoomStatus { get; set; }
        public string RoomType { get; set; }
        public int Capacity { get; set; }
        public double BasePrice { get; set; }
    }
}
