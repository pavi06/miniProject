using System.Security.Permissions;

namespace HotelBookingSystemAPI.Models
{
    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
}
