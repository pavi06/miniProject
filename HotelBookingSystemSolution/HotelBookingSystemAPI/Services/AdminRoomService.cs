using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using HotelBookingSystemAPI.Repositories;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace HotelBookingSystemAPI.Services
{
    public class AdminRoomService : IAdminRoomService
    {
        private readonly IRepository<int, Room> _roomRepository;
        private readonly IRepository<int, RoomType> _roomTypeRepository;

        public AdminRoomService(IRepository<int, Room> roomRepository, IRepository<int, RoomType> roomTypeRepository)
        {
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
        }

        #region AddRoom
        public async Task<ReturnRoomDTO> RegisterRoomForHotel(AddRoomDTO roomDTO)
        {
            var addedRoom = await _roomRepository.Add(new Room(roomDTO.TypeId, roomDTO.HotelId, roomDTO.Images));
            if (addedRoom != null)
            {
                return new ReturnRoomDTO(addedRoom.RoomId, addedRoom.TypeId, addedRoom.HotelId, addedRoom.Images, addedRoom.IsAvailable);
            }
            throw new ObjectAlreadyExistsException("Room");
        }
        #endregion

        #region AddRoomType
        public async Task<RoomTypeReturnDTO> RegisterRoomTypeForHotel(RoomTypeDTO roomTypeDTO)
        {
            try
            {
                var addedRoomType = await _roomTypeRepository.Add(new RoomType(roomTypeDTO.Type, roomTypeDTO.Occupancy, roomTypeDTO.Images, roomTypeDTO.Amount, roomTypeDTO.CotsAvailable,
                    roomTypeDTO.Amenities, roomTypeDTO.Discount, roomTypeDTO.HotelId));

                return new RoomTypeReturnDTO(addedRoomType.RoomTypeId, addedRoomType.Type, addedRoomType.Occupancy, addedRoomType.Images, addedRoomType.Amount, addedRoomType.CotsAvailable, addedRoomType.Amenities,
                    addedRoomType.Discount, addedRoomType.HotelId);
            }
            catch (ObjectAlreadyExistsException)
            {
                throw new ObjectAlreadyExistsException("RoomType");
            }

        }
        #endregion

        #region UpdateRoomImages
        public async Task<string> UpdateRoomImages(int id, string imageUrls)
        {
            try
            {
                var room = await _roomRepository.Get(id);
                room.Images = imageUrls;
                if (await _roomRepository.Update(room) != null)
                {
                    return "Images updated successfully";
                }
                return "Images not updated! Try again later";
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Room");
            }
        }
        #endregion

        #region UpdateRoomStatus
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
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Room");
            }

        }
        #endregion

        #region UpdateRoomTypeByAttribute
        public async Task<RoomTypeReturnDTO> UpdateRoomTypeByAttribute(UpdateRoomTypeDTO updateDTO)
        {
            try
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
                    case "images":
                        roomType.Images = updateDTO.AttributeValue;
                        break;
                    default:
                        throw new Exception("No such attribute available!");
                }
                var updatedRoomType = await _roomTypeRepository.Update(roomType);
                return new RoomTypeReturnDTO(updatedRoomType.RoomTypeId, updatedRoomType.Type, updatedRoomType.Occupancy, updatedRoomType.Images, updatedRoomType.Amount, updatedRoomType.CotsAvailable,
                    updatedRoomType.Amenities, updatedRoomType.Discount, updatedRoomType.HotelId);
            }
            catch (ObjectsNotAvailableException)
            {
                throw new ObjectNotAvailableException("RoomType");
            }

        }
        #endregion
    }

}
