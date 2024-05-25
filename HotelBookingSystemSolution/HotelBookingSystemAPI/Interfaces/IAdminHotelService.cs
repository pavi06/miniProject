using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IAdminHotelService
    {
        public Task<HotelReturnDTO> RegisterHotel(HotelRegisterDTO hotelDTO);
        //public Task<UpdateHotelReturnDTO> UpdateHotel(UpdateHotelDTO hotelDTO);
        public Task<bool> UpdateHotelAvailabilityService(int hotelId, bool status);
        public Task<int> UpdateHotelRoomAvailabilityService(int hotelId, int availableCount);
        public Task<List<HotelReturnDTO>> GetAllHotels();
        public Task<HotelReturnDTO> GetHotelById(int hotelId);
        public Task<HotelReturnDTO> RemoveHotel(int hotelId);

    }
}
