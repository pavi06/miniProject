﻿using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;
using HotelBookingSystemAPI.Models.DTOs.RatingDTOs;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;

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
        public Task<string> UpdateHotelAvailabilityService(int hotelId);
        //update hotel by attributes(Amenities, totalNoOfRooms, etc)
        public Task<HotelReturnDTO> UpdateHotelAttribute(UpdateHotelDTO updateHotelDTO);

        //to get all hotels available
        public Task<List<AdminHotelReturnDTO>> GetAllHotels(int limit, int skip);
        public Task<List<AdminHotelReturnDTO>> GetAllHotelsWithoutLimit();

        public Task<List<AdminHotelReturnDTO>> GetAllHotelsByLocation(string location);

        //search hotel by id
        public Task<AdminHotelReturnDTO> GetHotelById(int hotelId);

        //remove hotel
        public Task<HotelReturnDTO> RemoveHotel(int hotelId);
        public Task<List<RatingReturnDTO>> GetAllRatings(int hotelId);

        //to get basic details of app.(no of users, hotels , etc)
        public Task<AppDetailsDTO> GetDetails();


    }
}
