using System;
using System.Collections.Generic;
using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using Hotel_C4ta.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace Hotel_C4ta.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DBContext _dbContext;

        public PaymentRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Payment> GetAllPayments()
        {
            var payments = new List<Payment>();
            using var conn = _dbContext.OpenConnection();

            string sql = "SELECT PaymentID, PaymentDate, Amount, PaymentMethod, BillID FROM Payment";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                payments.Add(new Payment
                {
                    PaymentID = reader.GetInt32(0),
                    PaymentDate = reader.GetDateTime(1),
                    Amount = reader.GetDecimal(2),
                    PaymentMethod = reader.GetString(3),
                    BillID = reader.GetInt32(4)
                });
            }

            return payments;
        }

        public Payment? GetPayment(int paymentId)
        {
            using var conn = _dbContext.OpenConnection();

            string sql = "SELECT PaymentID, PaymentDate, Amount, PaymentMethod, BillID FROM Payment WHERE PaymentID=@PaymentID";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PaymentID", paymentId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Payment
                {
                    PaymentID = reader.GetInt32(0),
                    PaymentDate = reader.GetDateTime(1),
                    Amount = reader.GetDecimal(2),
                    PaymentMethod = reader.GetString(3),
                    BillID = reader.GetInt32(4)
                };
            }

            return null;
        }

        public int RegisterPayment(decimal amount, string paymentMethod, int billId)
        {
            using var conn = _dbContext.OpenConnection();

            string sql = @"
              INSERT INTO Payment (PaymentDate, Amount, PaymentMethod, BillID)
              OUTPUT INSERTED.PaymentID
              VALUES (GETDATE(), @Amount, @PaymentMethod, @BillID)";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Amount", amount);
            cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
            cmd.Parameters.AddWithValue("@BillID", billId);

            return (int)cmd.ExecuteScalar();
        }
    }
}
