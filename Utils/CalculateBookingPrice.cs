using Hotel_C4ta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Utils
{
    public static class CalculateBookingPrice
    {
        public static decimal Calculate(RoomModel room, DateTime start, DateTime end) 
        {
            if (end <= start)
            {
                return 0;
            }

            var days = (end - start).Days;
            var total = days * room.BasePrice;

            return total;
        }
    }
}
