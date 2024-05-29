using HotelBookingSystemAPI.Models.DTOs.RatingDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IGuestRatingService
    {
        /// <summary>
        /// Allows the user to provide feedback for the service provided by the hotel.
        /// And Updates the overall rating for the hotel 
        /// </summary>
        /// <param name="ratingDTO">DTO object which holds the info like hotelid, rating, comments</param>
        /// <param name="loggedUser">LoggedIn user id</param>
        /// <returns></returns>
        public Task<RatingReturnDTO> ProvideRating(AddRatingDTO ratingDTO, int loggedUser);


        /// <summary>
        /// Method to remove the rating provided by the guest.
        /// </summary>
        /// <param name="rateId">rating object id</param>
        /// <returns></returns>
        public Task<string> DeleteRatingProvided(int rateId);
    }
}
