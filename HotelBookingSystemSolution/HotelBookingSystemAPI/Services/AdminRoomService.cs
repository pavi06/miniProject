using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;

namespace HotelBookingSystemAPI.Services
{
    public class AdminRoomService : IAdminRoomService
    {
        private readonly IRepository<int, Room> _roomRepository;
        private readonly IRepository<int, RoomType> _roomTypeRepository;

        public AdminRoomService(IRepository<int,Room> roomRepository, IRepository<int,RoomType> roomTypeRepository) { 
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<ReturnRoomDTO> RegisterRoomForHotel(AddRoomDTO roomDTO)
        {
            var room = new Room(roomDTO.TypeId,roomDTO.HotelId,roomDTO.Images);
            var addedRoom = await _roomRepository.Add(room);
            if(addedRoom != null)
            {
                return new ReturnRoomDTO(addedRoom.RoomId, addedRoom.TypeId, addedRoom.HotelId, addedRoom.Images, addedRoom.IsAvailable);
            }
            throw new ObjectAlreadyExistsException("Room");
        }

        public async Task<RoomTypeReturnDTO> RegisterRoomTypeForHotel(RoomTypeDTO roomTypeDTO)
        {
            try
            {
                var roomType = new RoomType(roomTypeDTO.Type,roomTypeDTO.Occupancy,roomTypeDTO.Amount, roomTypeDTO.CotsAvailable, 
                    roomTypeDTO.Amenities,roomTypeDTO.Discount, roomTypeDTO.HotelId);
                var addedRoomType = await _roomTypeRepository.Add(roomType);
                return new RoomTypeReturnDTO(addedRoomType.RoomTypeId, addedRoomType.Occupancy, addedRoomType.Amount,  addedRoomType.CotsAvailable, addedRoomType.Amenities,
                    addedRoomType.Discount,addedRoomType.HotelId);
            }
            catch (ObjectAlreadyExistsException e)
            {
                throw e;
            }
            
        }

        public async Task<ReturnRoomDTO> RemoveRoomFromHotel(int roomId)
        {
            try
            {
                var room = await _roomRepository.Get(roomId);
                room.IsAvailable = false;
                var updatedRoom = await _roomRepository.Update(room);
                return new ReturnRoomDTO(updatedRoom.RoomId, updatedRoom.TypeId, updatedRoom.HotelId, updatedRoom.Images, updatedRoom.IsAvailable);
            }
            catch(ObjectNotAvailableException e)
            {
                throw e;
            }            
            
        }
    }
}
