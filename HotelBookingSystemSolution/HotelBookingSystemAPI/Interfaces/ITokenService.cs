using HotelBookingSystemAPI.Models;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(Guest person);
        //public string GenerateRefreshToken();
    }
}

