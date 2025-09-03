using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Model
{
    class BillModel
    {
        public int _Number { get; set; }
        public DateTime _IssueDate { get; set; }
        public double _TotalAmount { get; set; }

        public int _IdBooking { get; set; }

    }
}
