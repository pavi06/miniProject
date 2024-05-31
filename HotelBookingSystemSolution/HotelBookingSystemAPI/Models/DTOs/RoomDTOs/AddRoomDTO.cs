using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class AddRoomDTO
    {
        [Required(ErrorMessage = "TypeId cannot be null")]
        public int TypeId { get; set; }
        [Required(ErrorMessage = "HotelID cannot be null")]
        public int HotelId { get; set; }
        public string? Images { get; set; }
    }
}
