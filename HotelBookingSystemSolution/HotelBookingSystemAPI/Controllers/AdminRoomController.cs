using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models.DTOs;
using HotelBookingSystemAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HotelBookingSystemAPI.Controllers
{
    [Authorize(Roles ="Admin")]
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


    }
}
