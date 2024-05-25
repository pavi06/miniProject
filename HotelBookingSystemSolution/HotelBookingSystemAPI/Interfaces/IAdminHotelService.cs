using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IAdminHotelService
    {
        public Task<HotelReturnDTO> RegisterHotel(HotelRegisterDTO hotelDTO);
        //public Task<UpdateHotelReturnDTO> UpdateHotel(UpdateHotelDTO hotelDTO);
        public Task<bool> UpdateHotelAvailabilityService(UpdateHotelStatusDTO updateHotelStatusDTO);
        //public Task<int> UpdateHotelRoomAvailabilityService(int hotelId, int availableCount);
        public Task<List<HotelReturnDTO>> GetAllHotels();
        public Task<HotelReturnDTO> GetHotelById(int hotelId);
        public Task<HotelReturnDTO> RemoveHotel(int hotelId);

    }
}
