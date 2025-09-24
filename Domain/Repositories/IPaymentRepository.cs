using Hotel_C4ta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Repositories
{
    /// Defines the contract for retrieving and creating payments in the system.
    public interface IPaymentRepository
    {
        IEnumerable<Payment> GetAllPayments(); /// Retrieves all payments.
        Payment? GetPayment(int paymentId); /// Retrieves a specific payment by its unique identifier.
        int RegisterPayment(decimal amount, string paymentMethod, int billId); /// Registers (creates) a new payment associated with a bill.
    }
}
