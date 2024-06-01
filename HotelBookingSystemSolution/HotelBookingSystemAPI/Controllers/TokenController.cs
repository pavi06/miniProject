using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBookingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly HotelBookingContext _context;
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService, HotelBookingContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }


        [HttpPost]
        [Route("refreshToken")]
        public IActionResult Refresh(Token tokenModel)
        {
            if (tokenModel == null)
                return BadRequest("Invalid client request");
            string accessToken = tokenModel.AccessToken;
            string refreshToken = tokenModel.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.FindFirstValue("UserId");
            var user = _context.Users.SingleOrDefault(u => u.GuestId.ToString() == username);
            if (user == null || user.RefreshToken != refreshToken || user.ExpiresOn <= DateTime.Now)
                return BadRequest("Invalid client request");
            var newAccessToken = _tokenService.GenerateToken(_context.Guests.SingleOrDefault(g=>g.GuestId == user.GuestId));
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken.RfrshToken;
            user.CreatedOn = newRefreshToken.Created;
            user.ExpiresOn = newRefreshToken.ExpiresOn;
            _context.SaveChangesAsync();
            return Ok(new AuthenticatedResponseToken()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.RfrshToken
            });
        }



        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var username = User.FindFirstValue("UserId");
            var user = _context.Users.SingleOrDefault(u => u.GuestId.ToString() == username);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
