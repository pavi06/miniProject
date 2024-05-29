using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class RoomTypeDTO
    {
        [Required(ErrorMessage = "Value cannot be null")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Value cannot be null")]
        public int Occupancy { get; set; }
        [Required(ErrorMessage = "Value cannot be null")]
        public string Images { get; set; }
        [Required(ErrorMessage = "Value cannot be null")]
        public double Amount { get; set; }
        [Required(ErrorMessage = "Value cannot be null")]
        public int CotsAvailable { get; set; }
        [Required(ErrorMessage = "Value cannot be null")]
        public string Amenities { get; set; }
        [Required(ErrorMessage = "Value cannot be null")]
        public double Discount { get; set; }
        [Required(ErrorMessage = "Value cannot be null")]
        public int HotelId { get; set; }
    }
}
