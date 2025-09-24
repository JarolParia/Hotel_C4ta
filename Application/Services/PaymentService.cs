using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Application.Services
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public IEnumerable<Payment> GetAllPayments()
        {
            return _paymentRepository.GetAllPayments();
        }

        public Payment? GetPayment(int paymentId)
        {
            return _paymentRepository.GetPayment(paymentId);
        }

        public int RegisterPayment(decimal amount, string paymentMethod, int billId)
        {
            if (amount <= 0)
                throw new ArgumentException("The amount must be greater than zero.");

            if (string.IsNullOrWhiteSpace(paymentMethod))
                throw new ArgumentException("The payment method cannot be empty.");

            return _paymentRepository.RegisterPayment(amount, paymentMethod, billId);
        }
    }
}
