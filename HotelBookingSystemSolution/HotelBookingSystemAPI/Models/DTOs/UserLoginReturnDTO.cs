namespace HotelBookingSystemAPI.Models.DTOs
{
    public class UserLoginReturnDTO
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
