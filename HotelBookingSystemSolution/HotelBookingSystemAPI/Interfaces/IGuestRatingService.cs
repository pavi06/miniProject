using HotelBookingSystemAPI.Models.DTOs.RatingDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IGuestRatingService
    {
        public Task<RatingReturnDTO> ProvideRating(AddRatingDTO ratingDTO, int loggedUser);
        public Task<string> DeleteRatingProvided(int rateId);
    }
}
