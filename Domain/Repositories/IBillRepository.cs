using Hotel_C4ta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Repositories
{
    /// Repository interface for managing bill persistence operations.
    /// without specifying the implementation (handled in Infrastructure).
    public interface IBillRepository
    {
        IEnumerable<Bill> GetAllBills(); /// Retrieves all bills from the data source.
        Bill? GetBill(int billId); /// The matching bill if found; otherwise, null.
        int RegisterBill(decimal totalAmount, int bookingId); /// The unique identifier (ID) of the newly created bill.
        bool SavePDF(int billId, byte[] pdfBytes); /// The file contain invoice details or a receipt.
        IEnumerable<dynamic> GetBillsWithPayments(); /// A collection combining bills and their associated payments.
    }
}
