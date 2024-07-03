using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Diagnostics;
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
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
        {
            _secretKey = configuration.GetSection("TokenKey").GetSection("JWT").Value.ToString();
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            _logger = logger;
        }

        #region RefreshTokenGeneration
        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                RfrshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresOn = DateTime.Now.AddMinutes(60)
            };
            return refreshToken;
        }
        #endregion

        #region AccessTokenGeneration
        public string GenerateToken(Guest guest)
        {
            string token = string.Empty;
            var claims = new List<Claim>(){
                new Claim("UserId", guest.GuestId.ToString()),
                new Claim("Email", guest.Email),
                new Claim("Role",guest.Role)
            };
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var myToken = new JwtSecurityToken(null, null, claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(myToken);
            _logger.LogInformation("Token generated successfully");
            return token;
        }
        #endregion

        #region AccessTokenGenerationForEmployee
        public string GenerateTokenForEmployee(HotelEmployee emp)
        {
            string token = string.Empty;
            var claims = new List<Claim>(){
                new Claim("UserId", emp.EmpId.ToString()),
                new Claim("Email", emp.Email),
                new Claim("Role",emp.Role),
                new Claim("HotelId",emp.HotelId.ToString())
            };
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var myToken = new JwtSecurityToken(null, null, claims, expires: DateTime.Now.AddMinutes(20), signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(myToken);
            _logger.LogInformation("Token generated successfully");
            return token;
        }
        #endregion

        #region TokenPrincipal
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, 
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
        #endregion

    }
}
