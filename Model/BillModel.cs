using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Model
{
    class BillModel
    {
        public int BillID { get; set; }
        public DateTime IssueDate { get; set; }
        public double TotalAmount { get; set; }
        public int BookingID { get; set; }
    }
}
