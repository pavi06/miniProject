namespace HotelBookingSystemAPI.Models.DTOs.GuestDTOs
{
    public class UserLoginReturnDTO
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
