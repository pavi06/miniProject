using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.BookingDTOs
{
    public class BookDetailsDTO
    {
        [Required(AllowEmptyStrings =false, ErrorMessage = "Value cannot be empty")]
        public string RoomType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Value cannot be empty")]
        public int RoomsNeeded { get; set; }

    }
}
