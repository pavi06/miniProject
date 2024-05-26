﻿using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IGuestSearchService
    {
        public Task<List<HotelReturnDTO>> GetHotelsByLocationAndDate(SearchHotelDTO hotelDTO);
        public Task<List<HotelReturnDTO>> GetHotelsByRatings(SearchHotelDTO hotelDTO);
        public Task<List<AvailableRoomTypesDTO>> GetAvailableRoomTypesByHotel(SearchRoomsDTO searchRoomDTO);

    }
}