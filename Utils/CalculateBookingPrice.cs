using Hotel_C4ta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Utils
{
    public static class CalculateBookingPrice
    {
        public static decimal Calculate(Room room, DateTime start, DateTime end)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room));

            if (end <= start)
                return 0;

            var days = (end - start).Days;
            if (days <= 0) days = 1; // mínimo 1 noche

            var total = days * room.BasePrice;

            return Math.Round(total, 2);
        }
    }
}
