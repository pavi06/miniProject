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
                    hotels.Append(new HotelReturnDTO(hotel.HotelId, hotel.Name, hotel.Address, hotel.City, hotel.Rating, string.Join(",", hotel.Amenities), string.Join(",", hotel.Restrictions), hotel.IsAvailable));
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
                return new HotelReturnDTO(hotel.HotelId, hotel.Name, hotel.Address, hotel.City, hotel.Rating, string.Join(",", hotel.Amenities), string.Join(",", hotel.Restrictions), hotel.IsAvailable);
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Hotel");
            }
        }

        public async Task<HotelReturnDTO> RegisterHotel(HotelRegisterDTO hotelDTO)
        {
            try
            {
                var hotel = new Hotel(hotelDTO.Name, hotelDTO.Address, hotelDTO.City, hotelDTO.TotalNoOfRooms,0,
                 hotelDTO.Amenities, hotelDTO.Restrictions, true);
                var addedHotel = await _hotelRepository.Add(hotel);
                return new HotelReturnDTO(addedHotel.HotelId, addedHotel.Name, addedHotel.Address, addedHotel.City, 
                     addedHotel.Rating, addedHotel.Amenities, addedHotel.Restrictions, addedHotel.IsAvailable);
            }
            catch (ObjectAlreadyExistsException)
            {
                throw new ObjectAlreadyExistsException("Hotel");
            }
            
        }

        public async Task<HotelReturnDTO> RemoveHotel(int hotelId)
        {
            try
            {
                var hotel = await _hotelRepository.Delete(hotelId);
                return new HotelReturnDTO(hotel.HotelId, hotel.Name, hotel.Address, hotel.City,   hotel.Rating, string.Join(",", hotel.Amenities), string.Join(",", hotel.Restrictions), hotel.IsAvailable);
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Hotel");
            }
            
        }

        public async Task<HotelReturnDTO> UpdateHotelAttribute(UpdateHotelDTO updateHotelDTO)
        {
            try{
                var hotel = await _hotelRepository.Get(updateHotelDTO.HotelId);
                switch (updateHotelDTO.AttributeName.ToLower())
                {
                    case "amenities":
                        hotel.Amenities = updateHotelDTO.AttributeValue;
                        break;
                    case "restrictions":
                        hotel.Restrictions = updateHotelDTO.AttributeValue;
                        break;
                    case "totalnoofrooms":
                        hotel.TotalNoOfRooms = Convert.ToInt32(updateHotelDTO.AttributeValue);
                        break;
                    case "name":
                        hotel.Name = updateHotelDTO.AttributeValue;
                        break;
                    default:
                        throw new Exception("No such attribute available!");
                }
                var updatedHotel = await _hotelRepository.Update(hotel);
                return new HotelReturnDTO(updatedHotel.HotelId, updatedHotel.Name, updatedHotel.Address, updatedHotel.City,
                    updatedHotel.Rating, updatedHotel.Amenities, updatedHotel.Restrictions, updatedHotel.IsAvailable);
            }
            catch (ObjectsNotAvailableException)
            {
                throw new ObjectNotAvailableException("Hotel");
            }
        }

        public async Task<string> UpdateHotelAvailabilityService(UpdateHotelStatusDTO statusDTO)
        {
            try
            {
                var hotel = await _hotelRepository.Get(statusDTO.HotelId);
                hotel.IsAvailable = statusDTO.Status;
                await _hotelRepository.Update(hotel);
                return "Status updated successfully";
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Hotel");
            }

        }

    }
}
