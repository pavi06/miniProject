using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models
{
    public class HotelEmployee
    {
        [Key]
        public int EmpId { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Role { get; set; } = "HotelEmployee";
    }
}
