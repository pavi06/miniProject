using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs
{
    public class RoomTypeDTO
    {
        [Required]
        public RoomTypes Type { get; set; }
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
