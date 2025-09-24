using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using Hotel_C4ta.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Infrastructure.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly DBContext _context;

        public BillRepository(DBContext context)
        {
            _context = context;
        }

        public IEnumerable<Bill> GetAllBills()
        {
            var bills = new List<Bill>();

            using var conn = _context.OpenConnection();
            string sql = "SELECT * FROM Bill";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                bills.Add(new Bill
                {
                    BillID = reader.GetInt32(0),
                    IssueDate = reader.GetDateTime(1),
                    TotalAmount = reader.GetDecimal(2),
                    PdfFile = reader.IsDBNull(3) ? null : (byte[])reader[3],
                    BookingID = reader.GetInt32(4)
                });
            }

            return bills;
        }

        public Bill? GetBill(int billId)
        {
            using var conn = _context.OpenConnection();
            string sql = "SELECT * FROM Bill WHERE BillID = @billId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@billId", billId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Bill
                {
                    BillID = reader.GetInt32(0),
                    IssueDate = reader.GetDateTime(1),
                    TotalAmount = reader.GetDecimal(2),
                    PdfFile = reader.IsDBNull(3) ? null : (byte[])reader[3],
                    BookingID = reader.GetInt32(4)
                };
            }

            return null;
        }

        public int RegisterBill(decimal totalAmount, int bookingId)
        {
            using var conn = _context.OpenConnection();
            string sql = @"
             INSERT INTO Bill (IssueDate, TotalAmount, BookingID)
             OUTPUT INSERTED.BillID
             VALUES (GETDATE(), @totalAmount, @bookingId)";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@totalAmount", totalAmount);
            cmd.Parameters.AddWithValue("@bookingId", bookingId);

            return (int)cmd.ExecuteScalar();
        }

        public IEnumerable<dynamic> GetBillsWithPayments()
        {
            var result = new List<dynamic>();
            using var conn = _context.OpenConnection();

            try
            {
                string sqlJoin = @"
         SELECT b.BillID, b.IssueDate, b.TotalAmount, b.BookingID,
                p.PaymentMethod, p.Amount, p.PaymentDate,
                b.PdfFile
         FROM Bill b
         LEFT JOIN Payment p ON b.BillID = p.BillID
         ORDER BY b.IssueDate DESC";

                using var cmd = new SqlCommand(sqlJoin, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new
                    {
                        BillID = reader.GetInt32(0),
                        IssueDate = reader.GetDateTime(1),
                        TotalAmount = reader.GetDecimal(2),
                        BookingID = reader.GetInt32(3),
                        PaymentMethod = reader.IsDBNull(4) ? "Pending" : reader.GetString(4),
                        Amount = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5),
                        PaymentDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                        PdfBytes = reader.IsDBNull(7) ? null : (byte[])reader[7]
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading bills with payments: {ex.Message}", ex);
            }

            return result;
        }
        public bool SavePDF(int billId, byte[] pdfBytes)
        {
            using var conn = _context.OpenConnection();
            string sql = "UPDATE Bill SET PdfFile = @pdfBytes WHERE BillID = @billId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@pdfBytes", pdfBytes);
            cmd.Parameters.AddWithValue("@billId", billId);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
