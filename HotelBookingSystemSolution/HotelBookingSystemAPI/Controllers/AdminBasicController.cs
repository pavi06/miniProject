using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;

namespace HotelBookingSystemAPI.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBasicController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminBasicController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("ActivateUser")]
        [ProducesResponseType(typeof(UserActivationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserActivationDTO>> ActivateUser(UserActivationDTO user)
        {
            try
            {
                UserActivationDTO result = await _userService.GetUserForActivation(user);
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
    }
}
