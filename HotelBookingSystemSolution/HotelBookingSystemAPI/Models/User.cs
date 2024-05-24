﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models
{
    public class User
    {
        [Key]
        public int GuestId { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordHashKey { get; set; }
        public string Status { get; set; }

        [ForeignKey("GuestId")]
        public Guest Guest { get; set; }
    }
}
