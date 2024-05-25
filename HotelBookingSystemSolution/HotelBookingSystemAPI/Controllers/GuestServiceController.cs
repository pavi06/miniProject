using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using HotelBookingSystemAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using Microsoft.AspNetCore.Authorization;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using System.Security.Claims;

namespace HotelBookingSystemAPI.Controllers
{
    [Authorize(Roles ="User")]
    [Route("api/[controller]")]
    [ApiController]
    public class GuestServiceController : ControllerBase
    {
        private readonly IGuestService _guestService;

        public GuestServiceController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("GetHotelsByLocationAndDate")]
        [ProducesResponseType(typeof(List<HotelReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<HotelReturnDTO>>> GetHotels(SearchHotelDTO searchHotelDTO)
        {
            try
            {
                List<HotelReturnDTO> result = await _guestService.GetHotelsByLocationAndDate(searchHotelDTO);
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

        [Authorize(Roles ="Admin")]
        [HttpGet("GetRoomsByHotel")]
        [ProducesResponseType(typeof(List<AvailableRoomTypesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AvailableRoomTypesDTO>>> GetRoomsForHotel(SearchRoomsDTO searchRoomDTO)
        {
            try
            {
                List<AvailableRoomTypesDTO> result = await _guestService.GetAvailableRoomTypesByHotel(searchRoomDTO);
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

        [HttpGet("BookRooms")]
        [ProducesResponseType(typeof(BookingReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingReturnDTO>> BookRooms(List<BookDetailsDTO> bookRooms)
        {
            try
            {
                var loggedUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                BookingReturnDTO result = await _guestService.BookRooms(bookRooms,loggedUser);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
    }
}
