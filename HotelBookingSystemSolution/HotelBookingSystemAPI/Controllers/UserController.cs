using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace HotelBookingSystemAPI.Controllers
{
    [ExcludeFromCodeCoverage]
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }


        #region UserLogin
        [HttpPost("Login")]
        [ProducesResponseType(typeof(UserLoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLoginReturnDTO>> Login([FromBody]UserLoginDTO userLoginDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userService.Login(userLoginDTO);
                    _logger.LogInformation("Login successfull");
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("User not authenticated!");
                    return Unauthorized(new ErrorModel(401, ex.Message));
                }
            }
            return BadRequest("All details are not provided. Please check");
        }
        #endregion

        #region UserRegistration
        [HttpPost("Register")]
        [ProducesResponseType(typeof(GuestReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GuestReturnDTO>> Register([FromBody]GuestRegisterDTO guestDTO)
        {
            try
            {
                var guest = await _userService.Register(guestDTO);
                _logger.LogInformation("Registration successfull");
                return Ok(guest);
            }
            catch (Exception ex)
            {
                _logger.LogError("Cannot register at this moment");
                return BadRequest(new ErrorModel(400, ex.Message));
            }
        }
        #endregion
    }
}
