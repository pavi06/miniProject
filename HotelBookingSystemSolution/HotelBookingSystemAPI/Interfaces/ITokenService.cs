using HotelBookingSystemAPI.Models;
using System.Security.Claims;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(Guest person);
        public string GenerateTokenForEmployee(HotelEmployee emp);
        public RefreshToken GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    }
}

