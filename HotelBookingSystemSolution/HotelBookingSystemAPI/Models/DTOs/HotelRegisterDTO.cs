using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs
{
    public class HotelRegisterDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Hotel Name cannot be empty")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address cannot be empty")]
        public string Address { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "City cannot be empty")]
        public string City { get; set; }
        [Required(ErrorMessage = "Provide the number of rooms.It cannot be 0")]
        public int TotalNoOfRooms { get; set; }
        [Required]
        public int NoOfRoomsAvailable { get; set; }
        public string? Amenities { get; set; }
        public string? Restrictions { get; set; }
    }
}
