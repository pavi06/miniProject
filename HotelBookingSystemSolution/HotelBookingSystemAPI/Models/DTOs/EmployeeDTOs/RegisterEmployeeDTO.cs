using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.EmployeeDTOs
{
    public class RegisterEmployeeDTO
    {
        [Required(ErrorMessage ="HotelId cannot be null")]
        public int HotelId { get; set; }
        [Required(ErrorMessage = "Name cannot be null")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email cannot be null")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "PhoneNumber cannot be null")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Address cannot be null")]
        public string Address { get; set; }

        [MinLength(6, ErrorMessage = "Password has to be minimum of 6 chars long")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }
    }
}
