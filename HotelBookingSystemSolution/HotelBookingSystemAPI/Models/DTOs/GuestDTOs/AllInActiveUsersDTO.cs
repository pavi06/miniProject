namespace HotelBookingSystemAPI.Models.DTOs.GuestDTOs
{
    public class AllInActiveUsersDTO
    {
        public int GuestId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; } = "User";

        public AllInActiveUsersDTO(int guestId, string email, string phoneNumber, string role) { 
            GuestId = guestId;
            Email = email;
            PhoneNumber = phoneNumber;
            Role = role;
        }
    }
}
