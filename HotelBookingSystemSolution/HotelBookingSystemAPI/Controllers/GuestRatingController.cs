using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models.DTOs.RatingDTOs;
using HotelBookingSystemAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HotelBookingSystemAPI.Controllers
{
    [Authorize(Roles ="User")]
    [Route("api/[controller]")]
    [ApiController]
    public class GuestRatingController : ControllerBase
    {
        private readonly IGuestRatingService _ratingService;

        public GuestRatingController(IGuestRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost("ProvideRatings")]
        [ProducesResponseType(typeof(RatingReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RatingReturnDTO>> ProvideRating(AddRatingDTO ratingDTO)
        {
            try
            {
                var loggedInUser = Convert.ToInt32(User.FindFirstValue("UserId"));
                var result = await _ratingService.ProvideRating(ratingDTO, loggedInUser);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpPost("RemoveMyRatings")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> RemoveRating(int ratingId)
        {
            try
            {
                string result = await _ratingService.DeleteRatingProvided(ratingId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
    }
}
