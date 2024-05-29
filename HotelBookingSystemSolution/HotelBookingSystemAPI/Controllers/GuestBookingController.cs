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
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using HotelBookingSystemAPI.Models.DTOs.RatingDTOs;
using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;
using HotelBookingSystemAPI.Services;

namespace HotelBookingSystemAPI.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [Route("api/[controller]")]
    [ApiController]
    public class GuestBookingController : ControllerBase
    {
        private readonly IGuestSearchService _guestSearchService;
        private readonly IGuestBookingService _bookingService;
        protected static SearchRoomsDTO searchRoom { get; set; }
        protected static SearchHotelDTO searchHotel { get; set; }

        public GuestBookingController(IGuestSearchService guestService,IGuestBookingService bookingService )
        {
            _bookingService = bookingService;
            _guestSearchService = guestService;
        }

        #region GetHotels
        [HttpPost("GetHotelsByLocationAndDate")]
        [ProducesResponseType(typeof(List<HotelReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<HotelReturnDTO>>> GetHotels(SearchHotelDTO searchHotelDTO)
        {
            searchHotel = searchHotelDTO;
            try
            {
                List<HotelReturnDTO> result = await _guestSearchService.GetHotelsByLocationAndDate(searchHotelDTO);
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
            {
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion


        #region GetHotelsByRating
        [HttpGet("GetHotelsByRating")]
        [ProducesResponseType(typeof(List<HotelReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<HotelReturnDTO>>> GetHotelsByRanking()
        {
            try
            {
                List<HotelReturnDTO> result = await _guestSearchService.GetHotelsByRatings(searchHotel);
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
            {
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion


        #region GetHotelsByCertainFeatures
        [HttpPost("GetHotelsByFeatures")]
        [ProducesResponseType(typeof(List<HotelReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<HotelReturnDTO>>> GetHotelsByFeature(List<string> features)
        {
            try
            {
                List<HotelReturnDTO> result = await _guestSearchService.GetHotelsByFeatures(features,searchHotel);
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
            {
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion


        #region getRoomsAvailableInThatHotel
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
                List<AvailableRoomTypesDTO> result = await _guestSearchService.GetAvailableRoomTypesByHotel(searchRoomDTO);
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
        #endregion


        #region GetDetailsOfRoomType
        [HttpPost("GetDetailsOfRoomType")]
        [ProducesResponseType(typeof(RoomTypeDescriptionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomTypeDescriptionDTO>> GetDetailsOfRoom(string roomType)
        {
            try
            {
                var result = await _guestSearchService.GetDetailedDescriptionOfRoomType(searchRoom.HotelId,roomType);
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
        #endregion


        #region RequestRoomsNeeded
        [HttpPost("BookRooms")]
        [ProducesResponseType(typeof(BookingReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingReturnDTO>> BookRooms(List<BookDetailsDTO> bookRooms)
        {
            try
            {
                var loggedUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                BookingReturnDTO result = await _bookingService.BookRooms(bookRooms, loggedUser, searchRoom);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion


        #region MakePaymentAndConfirmBooking
        [HttpPost("MakePaymentAndConfirmBooking")]
        [ProducesResponseType(typeof(PaymentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentReturnDTO>> MakePayment(double amount)
        {
            try
            {
                var loggedUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                var payment = await _bookingService.MakePayment(amount, loggedUser, searchRoom);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion


        #region CancelBooking
        [HttpPut("CancelBooking")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> CancelBooking( int bookingId)
        {
            try
            {
                var loggedUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                string result = await _bookingService.CancelBooking(bookingId, loggedUser);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion


        #region ModifyBooking
        [HttpPut("ModifyBooking")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> ModifyBooking(int bookingId, List<CancelRoomDTO> cancelRoomDTO)
        {
            try
            {
                var loggedUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                string result = await _bookingService.ModifyBooking(loggedUser, bookingId,cancelRoomDTO);
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
        #endregion


        #region GetMyBookings
        [HttpGet("GetMyBookings")]
        [ProducesResponseType(typeof(List<MyBookingDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<MyBookingDTO>>> GetBookings()
        {
            try
            {
                var loggedInUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                var result = await _bookingService.GetMyBookings(loggedInUser);
                if (result.Count > 0)
                {
                    return Ok(result);
                }
                return NotFound(new ErrorModel(404,"No bookings were done"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion

        #region GetRecommandedHotel
        [HttpGet("GetRecommandation")]
        [ProducesResponseType(typeof(List<HotelRecommendationDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<HotelRecommendationDTO>>> GetRecommandedHotels()
        {
            try
            {
                var loggedInUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                var result = await _guestSearchService.HotelRecommendations(loggedInUser);
                if (result.Count > 0)
                {
                    return Ok(result);
                }
                return NotFound(new ErrorModel(404, "No Hotels are recommanded"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion

    }
}
