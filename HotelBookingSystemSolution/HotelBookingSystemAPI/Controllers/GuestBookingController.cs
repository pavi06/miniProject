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
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Cors;

namespace HotelBookingSystemAPI.Controllers
{
    [ExcludeFromCodeCoverage]
    [EnableCors("MyCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class GuestBookingController : ControllerBase
    {
        private readonly IGuestSearchService _guestSearchService;
        private readonly IGuestBookingService _bookingService;
        private readonly ILogger<GuestBookingController> _logger;
        protected static SearchRoomsDTO searchRoom { get; set; }
        protected static SearchHotelDTO searchHotel { get; set; }
        protected static List<AvailableRoomTypesDTO> roomsAvailable { get; set; }

        public GuestBookingController(IGuestSearchService guestService,IGuestBookingService bookingService, ILogger<GuestBookingController> logger)
        {
            _bookingService = bookingService;
            _guestSearchService = guestService;
            _logger = logger;
        }


        [AllowAnonymous]
        #region GetHotelsByLocationAndDate
        [HttpPost("GetHotelsByLocationAndDate")]
        [ProducesResponseType(typeof(List<HotelReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<HotelReturnDTO>>> GetHotels([FromBody]SearchHotelDTO searchHotelDTO)
        {
            searchHotel = searchHotelDTO;
            try
            {
                List<HotelReturnDTO> result = await _guestSearchService.GetHotelsByLocationAndDate(searchHotelDTO);
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


        [AllowAnonymous]
        #region GetHotelsByRating
        [HttpGet("GetHotelsByRating")]
        [ProducesResponseType(typeof(List<HotelReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<HotelReturnDTO>>> GetHotelsByRanking()
        {
            try
            {
                List<HotelReturnDTO> result = await _guestSearchService.GetHotelsByRatings(searchHotel);
                _logger.LogInformation("Hotels retrieved successfully based on rating");
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

        [AllowAnonymous]
        #region GetHotelsByCertainFeatures
        [HttpPost("GetHotelsByFeatures")]
        [ProducesResponseType(typeof(List<HotelReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<HotelReturnDTO>>> GetHotelsByFeature(List<string> features)
        {
            try
            {
                List<HotelReturnDTO> result = await _guestSearchService.GetHotelsByFeatures(features,searchHotel);
                _logger.LogInformation("Successfully retrieved hotels by its features");
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion

        [AllowAnonymous]
        #region GetAvailableRoomTypesInThatHotel
        [HttpPost("GetRoomsByHotel")]
        [ProducesResponseType(typeof(List<AvailableRoomTypesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AvailableRoomTypesDTO>>> GetRoomsForHotel([FromBody]SearchRoomsDTO searchRoomDTO)
        {
            searchRoom = searchRoomDTO;
            try
            {
                TimeSpan diff = searchRoomDTO.CheckoutDate - searchRoomDTO.CheckInDate;
                if ((int)diff.TotalDays >= 8)
                {
                    return BadRequest(new ErrorModel(404, "Cannot book for more than a week"));
                }
                roomsAvailable = await _guestSearchService.GetAvailableRoomTypesByHotel(searchRoomDTO);
                _logger.LogInformation("Available roomsTypes displayed successfully!");
                return Ok(roomsAvailable);
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

        [AllowAnonymous]
        #region GetDetailsOfRoomType
        [HttpGet("GetDetailsOfRoomType")]
        [ProducesResponseType(typeof(RoomTypeDescriptionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomTypeDescriptionDTO>> GetDetailsOfRoom(string roomType)
        {
            try
            {
                var result = await _guestSearchService.GetDetailedDescriptionOfRoomType(searchRoom.HotelId,roomType);
                _logger.LogInformation("Description provided");
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

        [Authorize(Roles = "Admin,User")]
        #region RequestRoomsNeeded
        [HttpPost("BookRooms")]
        [ProducesResponseType(typeof(BookingReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingReturnDTO>> BookRooms(List<BookDetailsDTO> bookRooms)
        {
            try
            {
                foreach(var book in bookRooms)
                {
                    if (!roomsAvailable.Any(r=>r.RoomType.ToLower() == book.RoomType.ToLower() && book.RoomsNeeded <= r.NoOfRoomsAvailable))
                    {
                        _logger.LogError("Invalid input!");
                        return BadRequest(new ErrorModel(400,"Invalid input!"));
                    }
                }
                //Retrieved current user 
                var loggedUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                BookingReturnDTO result = await _bookingService.BookRooms(bookRooms, loggedUser, searchRoom);
                _logger.LogInformation("Booking confirmation Description provided");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion

        [Authorize(Roles = "Admin,User")]
        #region MakePaymentAndConfirmBooking
        [HttpPost("MakePaymentAndConfirmBooking")]
        [ProducesResponseType(typeof(PaymentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentReturnDTO>> MakePayment([FromBody]double amount)
        {
            try
            {
                var loggedUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                var payment = await _bookingService.MakePayment(amount, loggedUser, searchRoom);
                _logger.LogInformation("Payment done and booking confirmed!");
                return Ok(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion

        [Authorize(Roles = "Admin,User")]
        #region CancelBooking
        [HttpPut("CancelBooking")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> CancelBooking([FromBody] int bookingId)
        {
            try
            {
                var loggedUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                string result = await _bookingService.CancelBooking(bookingId, loggedUser);
                _logger.LogInformation("Booking Cancelled successfully");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion


        [Authorize(Roles = "Admin,User")]
        #region ModifyBooking
        [HttpPut("ModifyBooking")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> ModifyBooking([FromBody] ModifyBookingDTO modifyBooking)
        {
            try
            {
                var loggedUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                string result = await _bookingService.ModifyBooking(loggedUser, modifyBooking.BookingId, modifyBooking.CancelRooms);
                _logger.LogInformation("Booking Modified successfully");
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

        [Authorize(Roles = "Admin,User")]
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
                    _logger.LogInformation("User Bookings retrieved!");
                    return Ok(result);
                }
                _logger.LogInformation("No bookings");
                return NotFound(new ErrorModel(404,"No bookings were done"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion

        [Authorize(Roles = "Admin,User")]
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
                    _logger.LogInformation("Recommendations provided");
                    return Ok(result);
                }
                return NotFound(new ErrorModel(404, "No Hotels are recommanded"));
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
