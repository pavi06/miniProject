using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IAdminRoomService
    {
        public Task<RoomTypeReturnDTO> RegisterRoomTypeForHotel(RoomTypeDTO roomTypeDTO);
        public Task<ReturnRoomDTO> RegisterRoomForHotel(AddRoomDTO roomDTO);
        public Task<ReturnRoomDTO> RemoveRoomFromHotel(int roomId);
    }
}
