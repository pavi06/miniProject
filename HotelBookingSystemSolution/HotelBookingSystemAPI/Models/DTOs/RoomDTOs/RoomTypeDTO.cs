using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class RoomTypeDTO
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public int Occupancy { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public int CotsAvailable { get; set; }
        [Required]
        public string Amenities { get; set; }
        [Required]
        public double Discount { get; set; }
        [Required]
        public int HotelId { get; set; }
    }
}
