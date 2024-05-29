using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class UpdateRoomTypeDTO
    {
        [Required(ErrorMessage ="Value cannot be null")]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Value cannot be null")]
        public int RoomTypeId { get; set; }

        [Required(ErrorMessage = "Value cannot be null")]
        public string AttributeName { get; set; }

        [Required(ErrorMessage = "Value cannot be null")]
        public string AttributeValue { get; set; }
    }
}
