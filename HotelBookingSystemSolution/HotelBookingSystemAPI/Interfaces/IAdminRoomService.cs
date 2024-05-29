using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IAdminRoomService
    {
        public Task<RoomTypeReturnDTO> RegisterRoomTypeForHotel(RoomTypeDTO roomTypeDTO);
        public Task<ReturnRoomDTO> RegisterRoomForHotel(AddRoomDTO roomDTO);
        public Task<ReturnRoomDTO> UpdateRoomStatusForHotel(int roomId);
        public Task<RoomTypeReturnDTO> UpdateRoomTypeByAttribute(UpdateRoomTypeDTO updateDTO);
        public Task<bool> UpdateRoomImages(int roomId, string imageUrls);

    }
}
