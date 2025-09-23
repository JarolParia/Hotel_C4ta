using Hotel_C4ta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Repositories
{
    public interface IBillRepository
    {
        IEnumerable<Bill> GetAllBills();
        Bill? GetBill(int billId);
        int RegisterBill(decimal totalAmount, int bookingId);
        bool SavePDF(int billId, byte[] pdfBytes);
        IEnumerable<dynamic> GetBillsWithPayments();
    }
}
