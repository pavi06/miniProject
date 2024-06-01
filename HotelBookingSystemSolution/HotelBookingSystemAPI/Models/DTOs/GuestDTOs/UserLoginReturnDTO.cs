namespace HotelBookingSystemAPI.Models.DTOs.GuestDTOs
{
    public class UserLoginReturnDTO
    {
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
    }
}
