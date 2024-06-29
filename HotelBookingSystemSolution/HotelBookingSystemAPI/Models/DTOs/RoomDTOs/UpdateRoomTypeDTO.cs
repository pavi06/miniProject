using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class UpdateRoomTypeDTO
    {
        [Required(ErrorMessage ="HotelId cannot be null")]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "RoomTypeId cannot be null")]
        public int RoomTypeId { get; set; }

        [Required(ErrorMessage = "Attribute and Value cannot be null")]
        public Dictionary<string, string> AttributeValuesPair { get; set; } = new Dictionary<string, string>();
    }
}
