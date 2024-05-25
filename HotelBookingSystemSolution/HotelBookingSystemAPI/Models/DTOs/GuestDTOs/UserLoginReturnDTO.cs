namespace HotelBookingSystemAPI.Models.DTOs.GuestDTOs
{
    public class UserLoginReturnDTO
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
