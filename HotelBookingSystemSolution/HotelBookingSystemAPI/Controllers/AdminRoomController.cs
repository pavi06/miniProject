using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using System.Diagnostics.CodeAnalysis;

namespace HotelBookingSystemAPI.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoomController : ControllerBase
    {

        private readonly IAdminRoomService _roomService;

        public AdminRoomController(IAdminRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost("RegisterRoomForHotel")]
        [ProducesResponseType(typeof(ReturnRoomDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnRoomDTO>> AddRoom(AddRoomDTO room)
        {
            try
            {
                ReturnRoomDTO result = await _roomService.RegisterRoomForHotel(room);
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

        [HttpPost("RegisterRoomTypeForHotel")]
        [ProducesResponseType(typeof(RoomTypeReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomTypeReturnDTO>> AddRoomType(RoomTypeDTO roomType)
        {
            try
            {
                RoomTypeReturnDTO result = await _roomService.RegisterRoomTypeForHotel(roomType);
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

        [HttpPut("UpdateRoomStatusForHotel")]
        [ProducesResponseType(typeof(ReturnRoomDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnRoomDTO>> IsRoomAvailable(int roomId)
        {
            try
            {
                ReturnRoomDTO result = await _roomService.UpdateRoomStatusForHotel(roomId);
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

        [HttpPut("UpdateRoomTypeForHotel")]
        [ProducesResponseType(typeof(RoomTypeReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomTypeReturnDTO>> UpdateRoomType(UpdateRoomTypeDTO updateDTO)
        {
            try
            {
                var result = await _roomService.UpdateRoomTypeByAttribute(updateDTO);
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
