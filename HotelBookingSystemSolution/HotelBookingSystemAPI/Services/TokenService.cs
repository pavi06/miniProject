using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HotelBookingSystemAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _secretKey = configuration.GetSection("TokenKey").GetSection("JWT").Value.ToString();
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        }

        //public string GenerateRefreshToken()
        //{
        //    var refreshToken = new RefreshToken()
        //    {
        //        RfrshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        //        ExpiresOn = DateTime.Now.AddDays(1)
        //    };
        //    return refreshToken.ToString();
        //}

        public string GenerateToken(Guest guest)
        {
            string token = string.Empty;
            var claims = new List<Claim>(){
                new Claim("UserId", guest.GuestId.ToString()),
                new Claim("Email", guest.Email),
                new Claim("Role",guest.Role)
            };
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var myToken = new JwtSecurityToken(null, null, claims, expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(myToken);
            return token;
        }

        public string GenerateTokenForEmployee(HotelEmployee emp)
        {
            string token = string.Empty;
            var claims = new List<Claim>(){
                new Claim("UserId", emp.EmpId.ToString()),
                new Claim("Email", emp.Email),
                new Claim("Role",emp.Role)
            };
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var myToken = new JwtSecurityToken(null, null, claims, expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(myToken);
            return token;
        }
    }
}
