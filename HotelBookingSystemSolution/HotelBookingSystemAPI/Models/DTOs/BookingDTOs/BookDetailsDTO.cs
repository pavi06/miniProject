using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.BookingDTOs
{
    public class BookDetailsDTO
    {
        [Required(AllowEmptyStrings =false, ErrorMessage = "RoomType cannot be empty")]
        public string RoomType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RoomsNeeded cannot be empty")]
        [Range(1,1000, ErrorMessage ="Value must be >=1")]
        public int RoomsNeeded { get; set; }

    }
}
