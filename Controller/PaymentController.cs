using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hotel_C4ta.Controller
{
    public class PaymentController
    {
        public PaymentModel _paymentModel;

        public PaymentModel GetPayment(int paymentId)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "SELECT * FROM Payment WHERE PaymentID=@paymentId";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@paymentId", paymentId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    return new PaymentModel
                    {
                        PaymentID = reader.GetInt32(0),
                        PaymentDate = reader.GetDateTime(1),
                        Amount = reader.GetDecimal(2),
                        PaymentMethod = reader.GetString(3),
                        BillID = reader.GetInt32(4),
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading payment: " + ex.Message);
            }
            return null;
        }

        public int RegisterPayment(decimal amount, string paymentMethod, int billId)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "INSERT INTO Payment (PaymentDate, Amount, PaymentMethod, BillID)" +
                    "OUTPUT INSERTED.PaymentID" +
                    "VALUES (GETDATE(), @amount, @paymentMethod @billId)";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@paymentMethod", paymentMethod);
                cmd.Parameters.AddWithValue("@billId", billId);

                MessageBox.Show("Payment realized successfully.");

                return (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error realizing payment: " + ex.Message);
                return 0;
            }
        }
    }
}
