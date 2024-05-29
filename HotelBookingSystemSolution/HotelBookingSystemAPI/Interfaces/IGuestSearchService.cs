using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IGuestSearchService
    {
        /// <summary>
        /// Returns the list of hotels available in that location and date
        /// If rooms are available can able to book else not able to visit that hotel 
        /// </summary>
        /// <param name="hotelDTO"> dto object with location and date</param>
        /// <returns></returns>
        public Task<List<HotelReturnDTO>> GetHotelsByLocationAndDate(SearchHotelDTO hotelDTO);

        /// <summary>
        /// Return list of available hotels ( based on location and date) ranked on ratings
        /// </summary>
        /// <param name="hotelDTO">DTO object which holds hotelId, date</param>
        /// <returns></returns>
        public Task<List<HotelReturnDTO>> GetHotelsByRatings(SearchHotelDTO hotelDTO);

        /// <summary>
        /// Return list of hotels that satisfy a specific condition 
        /// </summary>
        /// <param name="features">feature to look for in the hotel, which is of string type</param>
        /// <param name="hotelDTO">DTO object which holds hotelId, date</param>
        /// <returns></returns>
        public Task<List<HotelReturnDTO>> GetHotelsByFeatures(List<string> features, SearchHotelDTO hotelDTO);

        /// <summary>
        /// Returns the list of roomtypes and no of rooms available in that type for the specified hotel
        /// </summary>
        /// <param name="searchRoomDTO">DTO object with roomtype, checkin and chechout date values</param>
        /// <returns></returns>
        public Task<List<AvailableRoomTypesDTO>> GetAvailableRoomTypesByHotel(SearchRoomsDTO searchRoomDTO);

        /// <summary>
        /// Retrieve description for that room type
        /// </summary>
        /// <param name="hotelId">hotelId of that roomtype belongs to</param>
        /// <param name="roomType">roomtype value in string</param>
        /// <returns></returns>
        public Task<RoomTypeDescriptionDTO> GetDetailedDescriptionOfRoomType(int hotelId, string roomType);

        /// <summary>
        /// Method which recommands the user with hotels that undergoes a discount based on the past booking history. 
        /// </summary>
        /// <param name="loggedUser">logged user id</param>
        /// <returns>returns a list of hotel along with the discount offerings going right now</returns>
        public Task<List<HotelRecommendationDTO>> HotelRecommendations(int loggedUser);

    }
}
