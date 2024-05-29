using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.GuestDTOs
{
    public class GuestRegisterDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email cannot be empty")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone cannot be empty")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address cannot be empty")]
        public string Address { get; set; }

        [MinLength(6, ErrorMessage = "Password has to be minimum of 6 chars long")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }
    }
}
