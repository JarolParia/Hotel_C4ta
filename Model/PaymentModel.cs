using Hotel_C4ta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Model
{
    public class PaymentModel
    {
        public int PaymentID { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Today; // Current by Default
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "Cash"; // Cash (by Default), Card, Transfer
        public int BillID { get; set; }
    }
}