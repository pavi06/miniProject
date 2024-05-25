using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;

namespace HotelBookingSystemAPI.Services
{
    public class AdminHotelService : IAdminHotelService
    {
        private readonly IRepository<int, Hotel> _hotelRepository;

        public AdminHotelService(IRepository<int,Hotel> hotelRepository) { 
            _hotelRepository = hotelRepository;
        }

        public async Task<List<HotelReturnDTO>> GetAllHotels()
        {
            var hotels = new List<HotelReturnDTO>();
            if(hotels.Count()>=1)
            {
                foreach(var hotel in await _hotelRepository.Get())
                {
                    hotels.Append(new HotelReturnDTO(hotel.HotelId, hotel.Name, hotel.Address, hotel.City, hotel.Rating, hotel.Amenities, hotel.Restrictions, hotel.IsAvailable));
                }
                return hotels;
            }
            throw new ObjectsNotAvailableException("hotels");
        }

        public async Task<HotelReturnDTO> GetHotelById(int hotelId)
        {
            try
            {
                var hotel = await _hotelRepository.Get(hotelId);
                return new HotelReturnDTO(hotel.HotelId, hotel.Name, hotel.Address, hotel.City, hotel.Rating, hotel.Amenities, hotel.Restrictions, hotel.IsAvailable);
            }
            catch (ObjectNotAvailableException e)
            {
                throw e;
            }
        }

        public async Task<HotelReturnDTO> RegisterHotel(HotelRegisterDTO hotelDTO)
        {
            try
            {
                var hotel = new Hotel(hotelDTO.Name, hotelDTO.Address, hotelDTO.City, hotelDTO.TotalNoOfRooms,
                 hotelDTO.Amenities, hotelDTO.Restrictions);
                var addedHotel = await _hotelRepository.Add(hotel);
                return new HotelReturnDTO(addedHotel.HotelId, addedHotel.Name, addedHotel.Address, addedHotel.City, 
                     addedHotel.Rating, addedHotel.Amenities, addedHotel.Restrictions, addedHotel.IsAvailable);
            }
            catch (ObjectAlreadyExistsException ex)
            {
                throw ex;
            }
            
        }

        public async Task<HotelReturnDTO> RemoveHotel(int hotelId)
        {
            try
            {
                var hotel = await _hotelRepository.Delete(hotelId);
                return new HotelReturnDTO(hotel.HotelId, hotel.Name, hotel.Address, hotel.City,   hotel.Rating, hotel.Amenities, hotel.Restrictions, hotel.IsAvailable);
            }
            catch (ObjectNotAvailableException e)
            {
                throw e;
            }
            
        }

        //public async Task<UpdateHotelReturnDTO> UpdateHotel(UpdateHotelDTO hotelDTO)
        //{
        //    try
        //    {
        //        var hotel = new Hotel(hotelDTO.HotelId, hotelDTO.Name, hotelDTO.TotalNoOfRooms,
        //        hotelDTO.NoOfRoomsAvailable, hotelDTO.Ratings, hotelDTO.Amenities, hotelDTO.Restrictions);
        //        var updatedHotel = await _hotelRepository.Update(hotel);
        //        return new UpdateHotelReturnDTO(updatedHotel.HotelId, updatedHotel.Name, updatedHotel.TotalNoOfRooms, updatedHotel.NoOfRoomsAvailable,
        //                updatedHotel.Rating, updatedHotel.Amenities, updatedHotel.Restrictions);
        //    }
        //    catch (ObjectNotAvailableException e)
        //    {
        //        throw e;
        //    }

        //}

        public async Task<bool> UpdateHotelAvailabilityService(UpdateHotelStatusDTO statusDTO)
        {
            try
            {
                var hotel = await _hotelRepository.Get(statusDTO.HotelId);
                hotel.IsAvailable = statusDTO.Status;
                await _hotelRepository.Update(hotel);
                return true;
            }
            catch (ObjectNotAvailableException e)
            {
                throw e;
            }
            
        }

        //public async Task<int> UpdateHotelRoomAvailabilityService(int hotelId, int availableCount)
        //{
        //    try
        //    {
        //        var hotel = await _hotelRepository.Get(hotelId);
        //        hotel.NoOfRoomsAvailable = availableCount;
        //        return _hotelRepository.Update(hotel).Result.NoOfRoomsAvailable;
        //    }
        //    catch (ObjectNotAvailableException e)
        //    {
        //        throw e;
        //    }
        //}
    }
}
