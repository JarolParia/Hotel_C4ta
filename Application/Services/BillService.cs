using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Application.Services
{
    /// Service layer for handling operations related to bills. <summary>
    /// Service layer for handling operations related to bills.This class coordinates business logic and delegates persistence to the repository layer.
    public class BillService
    {
        private readonly IBillRepository _billRepository;

        public BillService(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        // Get all bills from the repository
        public IEnumerable<Bill> GetAllBills()
        {
            return _billRepository.GetAllBills();
        }

        // Get a single bill by its ID
        public Bill? GetBill(int billId)
        {
            return _billRepository.GetBill(billId);
        }

        // Register a new bill, only if the total amount is greater than zero
        public int RegisterBill(decimal totalAmount, int bookingId)
        {
            if (totalAmount <= 0)
                throw new ArgumentException("The total amount must be greater than zero.");

            return _billRepository.RegisterBill(totalAmount, bookingId);
        }

        // Get all bills along with their related payments
        public IEnumerable<dynamic> GetBillsWithPayments()
        {
            return _billRepository.GetBillsWithPayments();
        }

        // Save a PDF file for a specific bill
        public bool SavePDF(int billId, byte[] pdfBytes)
        {
            if (pdfBytes == null || pdfBytes.Length == 0)
                throw new ArgumentException("PDF file cannot be empty.");

            return _billRepository.SavePDF(billId, pdfBytes);
        }
    }
}
