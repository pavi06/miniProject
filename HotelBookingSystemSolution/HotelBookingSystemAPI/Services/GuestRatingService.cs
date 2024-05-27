using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.RatingDTOs;
using HotelBookingSystemAPI.Repositories;

namespace HotelBookingSystemAPI.Services
{
    public class GuestRatingService : IGuestRatingService
    {
        private readonly IRepository<int, Rating> _ratingRepository;
        private readonly IRepository<int, Hotel> _hotelRepository;

        public GuestRatingService(IRepository<int,Rating> ratingRepository, IRepository<int,Hotel> hotelRepository) { 
            _ratingRepository = ratingRepository;
            _hotelRepository = hotelRepository;
        }

        public async Task<string> DeleteRatingProvided(int rateId)
        {
            try
            {
                if(await _ratingRepository.Delete(rateId) != null)
                {
                    return "Your feedback removed successfully";
                }
                else
                {
                    return "Your feedback not removed..Try again later.";
                }
            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }
        }

        public async Task<RatingReturnDTO> ProvideRating(AddRatingDTO ratingDTO, int loggedUser)
        {
            try
            {
                Rating rating = new Rating(loggedUser, ratingDTO.HotelId, ratingDTO.ReviewRating, ratingDTO.Comments);
                var ratingAdded = await _ratingRepository.Add(rating);
                var hotel = _hotelRepository.Get(ratingDTO.HotelId).Result;
                hotel.Rating = (hotel.Rating + ratingDTO.ReviewRating) / hotel.Ratings.Count();
                await _hotelRepository.Update(hotel);
                return new RatingReturnDTO(ratingAdded.RatingId,ratingAdded.ReviewRating,ratingAdded.Comments);
            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }

        }
    }
}
