using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IAdminHotelService
    {
        public Task<HotelReturnDTO> RegisterHotel(HotelRegisterDTO hotelDTO);
        public Task<bool> UpdateHotelAvailabilityService(UpdateHotelStatusDTO updateHotelStatusDTO);
        public Task<HotelReturnDTO> UpdateHotelAttribute(UpdateHotelDTO updateHotelDTO);
        public Task<List<HotelReturnDTO>> GetAllHotels();
        public Task<HotelReturnDTO> GetHotelById(int hotelId);
        public Task<HotelReturnDTO> RemoveHotel(int hotelId);

    }
}
