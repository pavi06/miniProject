using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Cors;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;

namespace HotelBookingSystemAPI.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [EnableCors("MyCors")]
    [ApiController]
    public class AdminHotelController : ControllerBase
    {
        private readonly IAdminHotelService _hotelService;
        private readonly ILogger<AdminHotelController> _logger;

        public AdminHotelController(IAdminHotelService hotelService, ILogger<AdminHotelController> logger)
        {
            _hotelService = hotelService;
            _logger = logger;
        }

        #region GetAllHotels
        [AllowAnonymous]
        [HttpGet("GetAllHotels")]
        [ProducesResponseType(typeof(List<AdminHotelReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AdminHotelReturnDTO>>> GetAllHotels()
        {
            try
            {
                List<AdminHotelReturnDTO> result = await _hotelService.GetAllHotels();
                _logger.LogInformation("Successfully retrieved hotels");
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
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

        #region AddHotel
        [HttpPost("RegisterHotel")]
        [ProducesResponseType(typeof(HotelReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelReturnDTO>> AddHotel([FromBody] HotelRegisterDTO hotel)
        {
            try
            {
                HotelReturnDTO result = await _hotelService.RegisterHotel(hotel);
                _logger.LogInformation("Hotel Added successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {   _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion

        #region UpdateHotelStatus
        [HttpPut("UpdateHotelAvailabilityStatus")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateHotelStatus([FromBody] int hotelId)
        {
            try
            {
                var result = await _hotelService.UpdateHotelAvailabilityService(hotelId);
                _logger.LogInformation("Hotel satatus updated");
                return Ok(new { message = result });
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

        #region UpdateHotel
        [HttpPut("UpdateHotel")]
        [ProducesResponseType(typeof(HotelReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelReturnDTO>> UpdateHotelByAttribute([FromBody] UpdateHotelDTO updateHotelDTO)
        {
            try
            {
                var result = await _hotelService.UpdateHotelAttribute(updateHotelDTO);
                _logger.LogInformation("Successfully updated");
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

        #region GetHotelById
        [AllowAnonymous]
        [HttpPost("GetHotel")]
        [ProducesResponseType(typeof(HotelDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelDTO>> GetHotel([FromBody] int id)
        {
            try
            {
                HotelDTO result = await _hotelService.GetHotelById(id);
                _logger.LogInformation("Successfully retrieved hotel");
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
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
