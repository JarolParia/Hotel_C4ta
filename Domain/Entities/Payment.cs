using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Entities
{
    /// Represents a payment made for a bill.
    public class Payment
    {
        public int PaymentID { get; set; } /// Unique identifier of the payment.
        public DateTime PaymentDate { get; set; } /// Date and time when the payment was made.
        public decimal Amount { get; set; } /// Amount of money paid in this transaction.
        public string PaymentMethod { get; set; } /// Method of paymen (cash,card)
        public int BillID { get; set; } /// Identifier of the bill associated with this payment.
    }
}
