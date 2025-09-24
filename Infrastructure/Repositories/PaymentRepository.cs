using System;
using System.Collections.Generic;
using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using Hotel_C4ta.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace Hotel_C4ta.Infrastructure.Repositories
{
    /// Repository class responsible for managing Payment records in the database.
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DBContext _dbContext; /// Database context used to open SQL connections.

        public PaymentRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Payment> GetAllPayments() /// Retrieves all payments stored in the Payment table.
        {
            var payments = new List<Payment>();
            using var conn = _dbContext.OpenConnection(); /// Open a database connection

            /// SQL query to fetch all payments
            string sql = "SELECT PaymentID, PaymentDate, Amount, PaymentMethod, BillID FROM Payment";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader(); /// Execute the query and get a reader for the result set

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

            return payments; /// Return the list of payments
        }

        /// Retrieves a single payment by its PaymentID.
        /// Returns null if no record is found.
        public Payment? GetPayment(int paymentId)
        {
            using var conn = _dbContext.OpenConnection();

            // SQL query with parameter to fetch a specific payment
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

            return null; /// Return null if no matching record
        }

        // Registers a new payment into the Payment table.
        // Returns the newly generated PaymentID.
        public int RegisterPayment(decimal amount, string paymentMethod, int billId)
        {
            using var conn = _dbContext.OpenConnection();

            string sql = @"
              INSERT INTO Payment (PaymentDate, Amount, PaymentMethod, BillID)
              OUTPUT INSERTED.PaymentID
              VALUES (GETDATE(), @Amount, @PaymentMethod, @BillID)";

            using var cmd = new SqlCommand(sql, conn);
            /// Add parameters for the new payment
            cmd.Parameters.AddWithValue("@Amount", amount);
            cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
            cmd.Parameters.AddWithValue("@BillID", billId);

            return (int)cmd.ExecuteScalar(); /// Execute the insert and return the new PaymentID
        }
    }
}
