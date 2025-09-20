using Hotel_C4ta.Controller;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hotel_C4ta.Utils
{
    public static class BillsJoinPayments
    {
        public static List<dynamic> LoadBillsJoinPayment()
        {
            using (var conn = DBContext.OpenConnection())
            {
                var result = new List<dynamic>();

                try
                {
                    string sqlJoin = @"
                        SELECT b.BillID, b.IssueDate, b.TotalAmount, b.BookingID,
                               p.PaymentMethod, p.Amount, p.PaymentDate,
                               b.PdfFile
                        FROM Bill b
                        LEFT JOIN Payment p ON b.BillID = p.BillID
                        ORDER BY b.IssueDate DESC;";

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
                    MessageBox.Show("Error loading bills/payments: " + ex.Message);
                }
                return result;
            }
        }
    }
}
