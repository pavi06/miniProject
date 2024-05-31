using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class RoomTypeDTO
    {
        [Required(ErrorMessage = "RoomType cannot be null")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Occupancy cannot be null")]
        public int Occupancy { get; set; }
        [Required(ErrorMessage = "Images need to be provided")]
        public string Images { get; set; }
        [Required(ErrorMessage = "Amount cannot be empty")]
        public double Amount { get; set; }
        [Required(ErrorMessage = "CotsAvailable need to be provided")]
        public int CotsAvailable { get; set; }
        [Required(ErrorMessage = "Amenities cannot be null")]
        public string Amenities { get; set; }
        [Required(ErrorMessage = "Discount cannot be null")]
        public double Discount { get; set; }
        [Required(ErrorMessage = "HotelID cannot be null")]
        public int HotelId { get; set; }
    }
}
