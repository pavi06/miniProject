﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Booking Book { get; set; }
        public double AmountPaid { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMode { get; set; } 
        public DateTime PaymentDate { get; set; } = DateTime.Now;

    }
}