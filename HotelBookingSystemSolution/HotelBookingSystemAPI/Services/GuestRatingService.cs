using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.RatingDTOs;
using HotelBookingSystemAPI.Repositories;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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

        #region RemoveRatingProvided
        public async Task<string> DeleteRatingProvided(int rateId)
        {
            try
            {
                var ratingProvided = await _ratingRepository.Get(rateId);
                if (await _ratingRepository.Delete(rateId) != null)
                {
                    await UpdateOverAllRating(ratingProvided);
                    return "Your feedback removed successfully";
                }
                else
                {
                    return "Your feedback not removed..Try again later.";
                }
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Rating");
            }
        }
        #endregion

        #region ProvideRating
        public async Task<RatingReturnDTO> ProvideRating(AddRatingDTO ratingDTO, int loggedUser)
        {
            try
            {
                if(_hotelRepository.Get(ratingDTO.HotelId).Result!=null)
                {
                    Rating rating = new Rating(loggedUser, ratingDTO.HotelId, ratingDTO.ReviewRating, ratingDTO.Comments);
                    var ratingAdded = await _ratingRepository.Add(rating);
                    await UpdateOverAllRating(ratingAdded);
                    return new RatingReturnDTO(ratingAdded.RatingId, ratingAdded.ReviewRating, ratingAdded.Comments);
                }
                throw new ObjectNotAvailableException("Hotel");
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Hotel");
            }

        }
        #endregion

        #region UpdatingOverAllRating
        [ExcludeFromCodeCoverage]
        public async Task UpdateOverAllRating(Rating rating)
        {
            try{
                var hotel = _hotelRepository.Get(rating.HotelId).Result;
                var ratings = _ratingRepository.Get().Result.Where(r => r.HotelId == rating.HotelId);
                hotel.Rating = ratings.Sum(r => r.ReviewRating) / ratings.Count();               
                await _hotelRepository.Update(hotel);
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Hotel");
            }
        }
        #endregion

    }
}
