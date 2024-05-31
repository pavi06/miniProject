using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class UpdateRoomTypeDTO
    {
        [Required(ErrorMessage ="HotelId cannot be null")]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "RoomTypeId cannot be null")]
        public int RoomTypeId { get; set; }

        [Required(ErrorMessage = "AttributeName cannot be null")]
        public string AttributeName { get; set; }

        [Required(ErrorMessage = "AttributeValue cannot be null")]
        public string AttributeValue { get; set; }
    }
}
