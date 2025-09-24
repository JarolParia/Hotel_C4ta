using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Entities
{
    /// Represents a bill (invoice) issued for a hotel booking.
    public class Bill
    {
        public int BillID { get; set; } /// Unique identifier of the bill.
        public DateTime IssueDate { get; set; } /// Date and time when the bill was issued.
        public decimal TotalAmount { get; set; } /// Total amount to be paid in the bill.
        public byte[]? PdfFile { get; set; } /// PDF file associated with the bill.
        public int BookingID { get; set; } /// Identifier of the booking associated with this bill.
    }
}
