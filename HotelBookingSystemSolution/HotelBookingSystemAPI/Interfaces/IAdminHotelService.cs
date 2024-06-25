using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IAdminHotelService
    {

        /// <summary>
        /// To add new hotel to the system
        /// </summary>
        /// <param name="hotelDTO">Dto of  hotel informstion</param>
        /// <returns>return dto if registration successfull</returns>
        public Task<HotelReturnDTO> RegisterHotel(HotelRegisterDTO hotelDTO);

        //updates the hotel availability status -> soft delete.
        public Task<string> UpdateHotelAvailabilityService(UpdateHotelStatusDTO updateHotelStatusDTO);
        //update hotel by attributes(Amenities, totalNoOfRooms, etc)
        public Task<HotelReturnDTO> UpdateHotelAttribute(UpdateHotelDTO updateHotelDTO);

        //to get all hotels available
        public Task<List<AdminHotelReturnDTO>> GetAllHotels();

        //search hotel by id
        public Task<HotelReturnDTO> GetHotelById(int hotelId);

        //remove hotel
        public Task<HotelReturnDTO> RemoveHotel(int hotelId);

    }
}
