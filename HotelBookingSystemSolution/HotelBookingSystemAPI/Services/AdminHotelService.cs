using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;
using HotelBookingSystemAPI.Models.DTOs.RatingDTOs;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using HotelBookingSystemAPI.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Services
{
    public class AdminHotelService : IAdminHotelService
    {
        private readonly IRepository<int, Hotel> _hotelRepository;
        private readonly IRepository<int, Rating> _ratingRepository;

        public AdminHotelService(IRepository<int,Hotel> hotelRepository, IRepository<int, Rating> ratingRepository) { 
            _hotelRepository = hotelRepository;
            _ratingRepository = ratingRepository;
        }

        #region GetAllHotels
        public async Task<List<AdminHotelReturnDTO>> GetAllHotels()
        {
            List<AdminHotelReturnDTO> hotels = (await _hotelRepository.Get())
             .Select(hotel =>
             {
                 var roomTypes = hotel.RoomTypes.Select(r => r.Type).ToList();
                 return new AdminHotelReturnDTO(
                    hotel.HotelId,
                    hotel.Name,
                    hotel.Address,
                    hotel.City,
                    hotel.Rating,
                    hotel.Ratings.Count(),
                    hotel.Amenities,
                    hotel.Restrictions,
                    hotel.IsAvailable,
                    hotel.TotalNoOfRooms,
                    roomTypes
                 );
             })
             .ToList();

            if (hotels.Count() >= 1)
            {
                return hotels;
            }
            throw new ObjectsNotAvailableException("hotels");
        }
        #endregion

        #region GetHotelById
        public async Task<AdminHotelReturnDTO> GetHotelById(int hotelId)
        {
            try
            {
                var hotel = await _hotelRepository.Get(hotelId);
                var roomTypes = hotel.RoomTypes.Select(r => r.Type).ToList();
                return new AdminHotelReturnDTO(hotel.HotelId, hotel.Name, hotel.Address, hotel.City, hotel.Rating, hotel.Ratings.Count(), hotel.Amenities, hotel.Restrictions, hotel.IsAvailable, hotel.TotalNoOfRooms,roomTypes);
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Hotel");
            }
        }
        #endregion

        #region AddHotel
        public async Task<HotelReturnDTO> RegisterHotel(HotelRegisterDTO hotelDTO)
        {
            try
            {
                var addedHotel = await _hotelRepository.Add( new Hotel(hotelDTO.Name, hotelDTO.Address, hotelDTO.City, hotelDTO.TotalNoOfRooms, 0,
                 hotelDTO.Amenities, hotelDTO.Restrictions, true) );
                return new HotelReturnDTO(addedHotel.HotelId, addedHotel.Name, addedHotel.Address, addedHotel.City, 
                     addedHotel.Rating, addedHotel.Amenities, addedHotel.Restrictions, addedHotel.IsAvailable, addedHotel.Ratings,addedHotel.RoomTypes);
            }
            catch (ObjectAlreadyExistsException)
            {
                throw new ObjectAlreadyExistsException("Hotel");
            }
            
        }       
        #endregion

        #region RemoveHotel
        public async Task<HotelReturnDTO> RemoveHotel(int hotelId)
        {
            try
            {
                var hotel = await _hotelRepository.Delete(hotelId);
                return new HotelReturnDTO(hotel.HotelId, hotel.Name, hotel.Address, hotel.City,   hotel.Rating, hotel.Amenities, hotel.Restrictions, hotel.IsAvailable, hotel.Ratings, hotel.RoomTypes);
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Hotel");
            }
            
        }
        #endregion

        #region UpdateHotelAttribute
        public async Task<HotelReturnDTO> UpdateHotelAttribute(UpdateHotelDTO updateHotelDTO)
        {
            try{
                var hotel = await _hotelRepository.Get(updateHotelDTO.HotelId);
                foreach(var pair in updateHotelDTO.AttributeValuesPair)
                {
                    Console.WriteLine(pair);
                    switch (pair.Key.ToLower())
                    {
                        case "amenities":
                            hotel.Amenities = pair.Value;
                            break;
                        case "restrictions":
                            hotel.Restrictions = pair.Value;
                            break;
                        case "totalnoofrooms":
                            hotel.TotalNoOfRooms = Convert.ToInt32(pair.Value);
                            break;
                        case "name":
                            hotel.Name = pair.Value;
                            break;
                        case "address":
                            hotel.Address = pair.Value;
                            break;
                        case "city":
                            hotel.City = pair.Value;
                            break;
                        default:
                            throw new Exception("No such attribute available!");
                    }
                }
                var updatedHotel = await _hotelRepository.Update(hotel);
                return new HotelReturnDTO(updatedHotel.HotelId, updatedHotel.Name, updatedHotel.Address, updatedHotel.City,
                    updatedHotel.Rating, updatedHotel.Amenities, updatedHotel.Restrictions, updatedHotel.IsAvailable, updatedHotel.Ratings, updatedHotel.RoomTypes);
            }
            catch (ObjectsNotAvailableException)
            {
                throw new ObjectNotAvailableException("Hotel");
            }
        }
        #endregion

        #region UpdateHotelAvailability
        public async Task<string> UpdateHotelAvailabilityService(int hotelId)
        {
            try
            {
                var hotel = await _hotelRepository.Get(hotelId);
                hotel.IsAvailable = !hotel.IsAvailable;
                await _hotelRepository.Update(hotel);
                return "Status updated successfully";
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Hotel");
            }

        }
        #endregion


        #region Ratings
        public async Task<List<RatingReturnDTO>> GetAllRatings(int hotelId)
        {
            var ratings = _ratingRepository.Get().Result.Where(r => r.HotelId == hotelId).ToList();
            List<RatingReturnDTO> ratingsList = new List<RatingReturnDTO>();
            foreach (var rating in ratings)
            {
                ratingsList.Add(
                    new RatingReturnDTO(rating.RatingId, rating.ReviewRating, rating.Comments, rating.Guest.Name, rating.Date));
            }
            return ratingsList;
        }
        #endregion


    }
}
