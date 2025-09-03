using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Model
{
    class BookingModel
    {
        public int _Id { get; set; }
        public DateTime _StartDate { get; set; }
        public DateTime _EndDate { get; set; }
        public string _Status { get; set; } = "";

        public double _EstimatedPrice { get; set; }

        public string _DniClient { get; set; } = "";
        public int _IdReceptionist { get; set; }
        public int _RoomNumber { get; set; }
    }
}
