using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;
using System.Diagnostics.CodeAnalysis;
using HotelBookingSystemAPI.Services;

namespace HotelBookingSystemAPI.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBasicController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AdminBasicController> _logger;

        public AdminBasicController(IUserService userService, ILogger<AdminBasicController> logger)
        {
            _userService = userService;
            _logger = logger;
            _logger = logger;
        }

        #region ActivateUser
        [HttpPut("ActivateUser")]
        [ProducesResponseType(typeof(UserActivationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserActivationDTO>> ActivateUser(UserActivationDTO user)
        {
            try
            {
                UserActivationDTO result = await _userService.GetUserForActivation(user);
                _logger.LogInformation("User Activated successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion
    }
}
