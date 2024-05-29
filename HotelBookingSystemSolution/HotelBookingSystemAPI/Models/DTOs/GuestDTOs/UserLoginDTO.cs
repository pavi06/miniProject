using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.GuestDTOs
{
    public class UserLoginDTO
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email cannot be empty")]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(6, ErrorMessage = "Password has to be minmum 6 chars long")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; } = string.Empty;
    }
}
