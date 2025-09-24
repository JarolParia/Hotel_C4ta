using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Application.Services
{
    /// Service layer for handling operations related to payments.
    public class PaymentService
    {
        // Reference to the payment repository.  The repository is responsible for the actual persistence (DB) operations.
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        // Retrieves all payments from the repository.
        public IEnumerable<Payment> GetAllPayments()
        {
            return _paymentRepository.GetAllPayments();
        }

        // Retrieves a single payment by its unique ID.
        // The return type "Payment?" allows null if no payment is found.
        public Payment? GetPayment(int paymentId)
        {
            return _paymentRepository.GetPayment(paymentId);
        }

        // Registers (creates) a new payment in the system.
        // Includes validation to ensure business rules are respected
        public int RegisterPayment(decimal amount, string paymentMethod, int billId)
        {
            if (amount <= 0)
                throw new ArgumentException("The amount must be greater than zero."); // Amount must be greater than 0

            if (string.IsNullOrWhiteSpace(paymentMethod)) 
                throw new ArgumentException("The payment method cannot be empty."); // Payment method cannot be empty

            return _paymentRepository.RegisterPayment(amount, paymentMethod, billId);
        }
    }
}
