using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using HotelBookingSystemAPI.Repositories;

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
                var roomType = new RoomType(roomTypeDTO.Type,roomTypeDTO.Occupancy,roomTypeDTO.Images,roomTypeDTO.Amount, roomTypeDTO.CotsAvailable, 
                    roomTypeDTO.Amenities, roomTypeDTO.Discount, roomTypeDTO.HotelId);

                var addedRoomType = await _roomTypeRepository.Add(roomType);

                return new RoomTypeReturnDTO(addedRoomType.RoomTypeId, addedRoomType.Type,addedRoomType.Occupancy, addedRoomType.Images,addedRoomType.Amount,  addedRoomType.CotsAvailable, addedRoomType.Amenities,
                    addedRoomType.Discount,addedRoomType.HotelId);
            }
            catch (ObjectAlreadyExistsException)
            {
                throw ;
            }
            
        }

        public async Task<bool> UpdateRoomImages(int roomId, string imageUrls)
        {
            var room = await _roomRepository.Get(roomId);
            room.Images = imageUrls;
            if(await _roomRepository.Update(room) != null)
            {
                return true;
            }
            return false;
        }

        public async Task<ReturnRoomDTO> UpdateRoomStatusForHotel(int roomId)
        {
            try
            {
                var room = await _roomRepository.Get(roomId);
                if (room.IsAvailable)
                    room.IsAvailable = false;
                else
                    room.IsAvailable = true;
                var updatedRoom = await _roomRepository.Update(room);
                return new ReturnRoomDTO(updatedRoom.RoomId, updatedRoom.TypeId, updatedRoom.HotelId, updatedRoom.Images, updatedRoom.IsAvailable);
            }
            catch(ObjectNotAvailableException )
            {
                throw ;
            }            
            
        }

        public async Task<RoomTypeReturnDTO> UpdateRoomTypeByAttribute(UpdateRoomTypeDTO updateDTO)
        {
            var roomType = await _roomTypeRepository.Get(updateDTO.RoomTypeId);
            switch (updateDTO.AttributeName.ToLower())
            {
                case "amount":
                    roomType.Amount = Convert.ToDouble(updateDTO.AttributeValue);
                    break;
                case "amenities":
                    roomType.Amenities = updateDTO.AttributeValue;
                    break;
                case "discount":
                    roomType.Discount = Convert.ToDouble(updateDTO.AttributeValue);
                    break;
                default:
                    return null;
            }
            var updatedRoomType = await _roomTypeRepository.Update(roomType);
            return new RoomTypeReturnDTO(updatedRoomType.RoomTypeId,updatedRoomType.Type,updatedRoomType.Occupancy, string.Join(",",updatedRoomType.Images),updatedRoomType.Amount,updatedRoomType.CotsAvailable,
                string.Join(",",updatedRoomType.Amenities),updatedRoomType.Discount,updatedRoomType.HotelId);
        }



    }
}
