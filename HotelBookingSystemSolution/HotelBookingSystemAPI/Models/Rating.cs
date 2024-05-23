﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public int PersonId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public double ReviewRating { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

    }
}