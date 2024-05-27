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
    [Authorize(Roles ="Admin,User")]
    [Route("api/[controller]")]
    [ApiController]
    public class GuestServiceController : ControllerBase
    {
        private readonly IGuestSearchService _guestService;
        private readonly IGuestBookingService _bookingService;
        protected static SearchRoomsDTO searchRoom { get; set; }
        protected int paymentId { get; set; }

        public GuestServiceController(IGuestSearchService guestService, IGuestBookingService bookingService)
        {
            _guestService = guestService;
            _bookingService = bookingService;
        }

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

        [HttpPost("GetRoomsByHotel")]
        [ProducesResponseType(typeof(List<AvailableRoomTypesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AvailableRoomTypesDTO>>> GetRoomsForHotel(SearchRoomsDTO searchRoomDTO)
        {
            searchRoom = searchRoomDTO;
            try
            {
                TimeSpan diff = searchRoomDTO.CheckoutDate - searchRoomDTO.CheckInDate;
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
                BookingReturnDTO result = await _bookingService.BookRooms(bookRooms,loggedUser, searchRoom);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpPost("MakePaymentAndConfirmBooking")]
        [ProducesResponseType(typeof(PaymentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentReturnDTO>> MakePayment(double amount)
        {
            try
            {
                var loggedUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                var payment = await _bookingService.MakePayment(amount, loggedUser,searchRoom);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        //[HttpPost("ConfirmBooking")]
        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<string>> ConfirmBooking(int paymentId)
        //{
        //    try
        //    {
        //        var loggedUser = Convert.ToInt32(User.FindFirstValue("UserId"));
        //        string result = await _bookingService.ConfirmBooking(bookingDetails,paymentId,loggedUser, searchRoom);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ErrorModel(400, ex.Message));
        //    }

        //}

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
