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
using HotelBookingSystemAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace HotelBookingSystemAPI.Controllers
{
    [Authorize(Roles ="User")]
    [Route("api/[controller]")]
    [ApiController]
    public class GuestServiceController : ControllerBase
    {
        private readonly IGuestSearchService _guestService;
        private readonly IGuestBookingService _bookingService;
        protected BookingReturnDTO bookingDetails;
        protected SearchRoomsDTO searchRoom;
        protected int paymentId;

        public GuestServiceController(IGuestSearchService guestService, IGuestBookingService bookingService)
        {
            _guestService = guestService;
            _bookingService = bookingService;
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("GetHotelsByLocationAndDate")]
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
        [HttpPost("GetRoomsByHotel")]
        [ProducesResponseType(typeof(List<AvailableRoomTypesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AvailableRoomTypesDTO>>> GetRoomsForHotel(SearchRoomsDTO searchRoomDTO)
        {
            searchRoom = searchRoomDTO;
            try
            {
                TimeSpan diff = searchRoom.CheckoutDate - searchRoom.CheckInDate;
                if ((int)diff.TotalDays >= 8)
                {
                    return BadRequest(new ErrorModel(404, "Cannot book for more than a week"));
                }
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

        [HttpPost("BookRooms")]
        [ProducesResponseType(typeof(BookingReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingReturnDTO>> BookRooms(List<BookDetailsDTO> bookRooms)
        {
            try
            {
                var loggedUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                BookingReturnDTO result = await _bookingService.BookRooms(bookRooms,loggedUser);
                bookingDetails = result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpPost("MakePayment")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> MakePayment(double amount)
        {
            try
            {
                paymentId = await _bookingService.MakePayment(amount);
                string result = paymentId >0 ? "Payment Successfull" : "Payment Not Successfull";
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpPost("ConfirmBooking")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> ConfirmBooking(BookingReturnDTO bookingDetails,int paymentId)
        {
            try
            {
                var loggedUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                string result = await _bookingService.ConfirmBooking(bookingDetails,paymentId,loggedUser);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpPut("CancelBooking")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> CancelBooking( int bookingId)
        {
            try
            {
                string result = await _bookingService.CancelBooking(bookingId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpPost("ProvideRatings")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> ProvideRating(AddRatingDTO ratingDTO)
        {
            try
            {
                var loggedInUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                string result = await _bookingService.ProvideRating(ratingDTO,loggedInUser);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

    }
}
