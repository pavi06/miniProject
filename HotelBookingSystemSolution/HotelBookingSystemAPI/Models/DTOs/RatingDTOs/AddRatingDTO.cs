using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models.DTOs.RatingDTOs
{
    public class AddRatingDTO
    {
        [Required(ErrorMessage = "HotelId cannot be empty")]
        public int HotelId { get; set; }
        [Required(ErrorMessage = "Rating cannot be empty and it should be between 5")]
        [Range(0,5)]
        public double ReviewRating { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Comments cannot be empty")]
        public string Comments { get; set; }
    }
}
