using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Application.Services
{
    public class BillService
    {
        private readonly IBillRepository _billRepository;

        public BillService(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        public IEnumerable<Bill> GetAllBills()
        {
            return _billRepository.GetAllBills();
        }

        public Bill? GetBill(int billId)
        {
            return _billRepository.GetBill(billId);
        }

        public int RegisterBill(decimal totalAmount, int bookingId)
        {
            if (totalAmount <= 0)
                throw new ArgumentException("The total amount must be greater than zero.");

            return _billRepository.RegisterBill(totalAmount, bookingId);
        }
        public IEnumerable<dynamic> GetBillsWithPayments()
        {
            return _billRepository.GetBillsWithPayments();
        }
        public bool SavePDF(int billId, byte[] pdfBytes)
        {
            if (pdfBytes == null || pdfBytes.Length == 0)
                throw new ArgumentException("PDF file cannot be empty.");

            return _billRepository.SavePDF(billId, pdfBytes);
        }
    }
}
