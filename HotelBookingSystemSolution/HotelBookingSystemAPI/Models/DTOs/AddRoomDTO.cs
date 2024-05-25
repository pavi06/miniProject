using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models.DTOs
{
    public class AddRoomDTO
    {
        [Required]
        public int TypeId { get; set; }
        [Required]
        public int HotelId { get; set; }
        [Required]
        public string? Images { get; set; }
    }
}
