using Hotel_C4ta.Model;
using Hotel_C4ta.Utils;
using Microsoft.Data.SqlClient;
using PdfSharp.Snippets.Font;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;

namespace Hotel_C4ta.Controller
{
    public class BillController
    {
        public BillModel _billModel;

        public List<BillModel> GetAllBills()
        {
            var bills = new List<BillModel>();

            using var conn = DBContext.OpenConnection();
            try
            {
                string sql = "SELECT * FROM Bill";
                using var cmd = new SqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bills.Add(new BillModel
                    {
                        BillID = reader.GetInt32(0),
                        IssueDate = reader.GetDateTime(1),
                        TotalAmount = reader.GetDecimal(2),
                        PdfFile = reader.IsDBNull(3) ? null : (byte[])reader[3],
                        BookingID = reader.GetInt32(4)
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bills: " + ex.Message);
            }

            return bills;
        }


        public BillModel GetBill(int billId)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "SELECT * FROM Bill WHERE BillID=@billId";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@billId", billId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    return new BillModel
                    {
                        BillID = reader.GetInt32(0),
                        IssueDate = reader.GetDateTime(1),
                        TotalAmount = reader.GetDecimal(2),
                        PdfFile = reader.IsDBNull(3) ? null : (byte[])reader[3],
                        BookingID = reader.GetInt32(4),
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bill: " + ex.Message);
            }
            return null;
        }

        public int RegisterBill(decimal totalAmount, int bookingId)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = @"
                    INSERT INTO Bill (IssueDate, TotalAmount, BookingID)
                    OUTPUT INSERTED.BillID
                    VALUES (GETDATE(), @totalAmount, @bookingId)";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@totalAmount", totalAmount);
                cmd.Parameters.AddWithValue("@bookingId", bookingId);

                MessageBox.Show("Bill generated successfully.");

                return (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating bill: " + ex.Message);
                return 0;
            }
        }

        public bool SavePDF(int billId, byte[]pdfBytes)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = @"UPDATE Bill SET PdfFile = @pdfBytes WHERE BillID = @billId";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@pdfBytes", pdfBytes);
                cmd.Parameters.AddWithValue("@billId", billId);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }          
        }     
    }
}
