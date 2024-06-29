using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;
using System.Diagnostics.CodeAnalysis;
using HotelBookingSystemAPI.Services;
using Microsoft.AspNetCore.Cors;

namespace HotelBookingSystemAPI.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [EnableCors("MyCors")]
    [ApiController]
    public class AdminBasicController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AdminBasicController> _logger;

        public AdminBasicController(IUserService userService, ILogger<AdminBasicController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        #region ActivateUser
        [HttpPut("UpdateUserStatus")]
        [ProducesResponseType(typeof(UserStatusDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserStatusDTO>> ActivateUser(UserStatusDTO user)
        {
            try
            {
                UserStatusDTO result = await _userService.UpdateUserStatus(user);
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

        #region GetAllUsersForActivaion
        [HttpGet("GetAllUsersForActivaion")]
        [ProducesResponseType(typeof(List<AllInActiveUsersDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AllInActiveUsersDTO>>> GetAllInActiveUsers()
        {
            try
            {
                List<AllInActiveUsersDTO> result = await _userService.GetAllUsersForActivation();
                _logger.LogInformation("All users retrieved successfully");
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
