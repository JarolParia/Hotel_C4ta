using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Model
{
    class PaymentModel
    {
        public int _Id { get; set; }
        public DateTime _PaymentDate { get; set; }
        public double _Amount { get; set; }
        public string _PaymentMethod { get; set; } = "";
        public int _IdBill { get; set; }
    }
}
