using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.HotelDTOs
{
    public class UpdateHotelDTO
    {
        [Required]
        public int HotelId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Hotel Name cannot be empty")]
        public string Name { get; set; }
        [Required]
        public int TotalNoOfRooms { get; set; }
        [Required]
        public int NoOfRoomsAvailable { get; set; }
        public double Ratings { get; set; }
        public string? Amenities { get; set; }
        public string? Restrictions { get; set; }

    }
}
