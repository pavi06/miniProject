using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.HotelDTOs
{
    public class UpdateHotelStatusDTO
    {
        [Required]
        public int HotelId { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}
