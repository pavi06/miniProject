using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models.DTOs.RatingDTOs;
using HotelBookingSystemAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;
using HotelBookingSystemAPI.Services;

namespace HotelBookingSystemAPI.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class GuestRatingController : ControllerBase
    {
        private readonly IGuestRatingService _ratingService;
        private readonly ILogger<GuestRatingController> _logger;

        public GuestRatingController(IGuestRatingService ratingService, ILogger<GuestRatingController> logger)
        {
            _ratingService = ratingService;
            _logger = logger;
        }

        #region ProvideRatings
        [HttpPost("ProvideRatings")]
        [ProducesResponseType(typeof(RatingReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RatingReturnDTO>> ProvideRating(AddRatingDTO ratingDTO)
        {
            try
            {
                var loggedInUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                var result = await _ratingService.ProvideRating(ratingDTO, loggedInUser);
                _logger.LogInformation("Rating provided successfully");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion

        #region GetAllBookings
        [HttpDelete("RemoveMyRatings")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> RemoveRating(int ratingId)
        {
            try
            {
                string result = await _ratingService.DeleteRatingProvided(ratingId);
                _logger.LogInformation("Rating removed successfully");
                return Ok(result);
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
