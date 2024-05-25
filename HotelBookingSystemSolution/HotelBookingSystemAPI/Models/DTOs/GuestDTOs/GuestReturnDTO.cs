using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.GuestDTOs
{
    public class GuestReturnDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }

        public GuestReturnDTO(int id, string name, string email, string phoneNumber, string address, string role)
        {
            Id = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            Role = role;
        }
    }
}
