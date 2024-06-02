using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using System.Diagnostics.CodeAnalysis;
using HotelBookingSystemAPI.Services;

namespace HotelBookingSystemAPI.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoomController : ControllerBase
    {

        private readonly IAdminRoomService _roomService;
        private readonly ILogger<AdminRoomController> _logger;

        public AdminRoomController(IAdminRoomService roomService, ILogger<AdminRoomController> logger)
        {
            _roomService = roomService;
            _logger = logger;
        }

        #region AddRoomForHotel
        [HttpPost("RegisterRoomForHotel")]
        [ProducesResponseType(typeof(ReturnRoomDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnRoomDTO>> AddRoom(AddRoomDTO room)
        {
            try
            {
                ReturnRoomDTO result = await _roomService.RegisterRoomForHotel(room);
                _logger.LogInformation("successfully registered");
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

        #region AddRoomTypeForHotel
        [HttpPost("RegisterRoomTypeForHotel")]
        [ProducesResponseType(typeof(RoomTypeReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomTypeReturnDTO>> AddRoomType(RoomTypeDTO roomType)
        {
            try
            {
                RoomTypeReturnDTO result = await _roomService.RegisterRoomTypeForHotel(roomType);
                _logger.LogInformation("successfully registered");
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

        #region UpdateRoomStatus
        [HttpPut("UpdateRoomStatusForHotel")]
        [ProducesResponseType(typeof(ReturnRoomDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnRoomDTO>> IsRoomAvailable(int roomId)
        {
            try
            {
                ReturnRoomDTO result = await _roomService.UpdateRoomStatusForHotel(roomId);
                _logger.LogInformation("successfully updated");
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

        #region UpdateRoomType
        [HttpPut("UpdateRoomTypeForHotel")]
        [ProducesResponseType(typeof(RoomTypeReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomTypeReturnDTO>> UpdateRoomType(UpdateRoomTypeDTO updateDTO)
        {
            try
            {
                var result = await _roomService.UpdateRoomTypeByAttribute(updateDTO);
                _logger.LogInformation("successfully updated");
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

        #region UpdateRoomImages
        [HttpPut("UpdateRoomImages")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateRoomImages(string type, int roomId, string images)
        {
            try
            {
                var result = await _roomService.UpdateRoomImages(type, roomId, images);
                _logger.LogInformation("successfully updated");
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
