using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Model
{
    class RoomModel
    {
        public int _Number { get; set; }
        public int _Floor { get; set; }
        public string _Status { get; set; } = "";
        public string _Type { get; set; } = "";
        public int _Capacity { get; set; }
        public double _BasePrice { get; set; }
    }
}
