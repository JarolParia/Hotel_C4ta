using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Model
{
    class RoomModel
    {
        public int RoomID { get; set; }
        public int RoomFloor { get; set; }
        public string RoomStatus { get; set; } = "Available"; // Available (by Default), Occupied
        public string RoomType { get; set; } // Simple, Double, Suite, Matrimonial
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
    }
}
