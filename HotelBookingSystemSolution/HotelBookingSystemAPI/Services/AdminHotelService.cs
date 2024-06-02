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

        #region GetAllHotels
        public async Task<List<HotelReturnDTO>> GetAllHotels()
        {
            List<HotelReturnDTO> hotels = (await _hotelRepository.Get())
                .Select(hotel => new HotelReturnDTO(
                    hotel.HotelId,
                    hotel.Name,
                    hotel.Address,
                    hotel.City,
                    hotel.Rating,
                    hotel.Amenities,
                    hotel.Restrictions,
                    hotel.IsAvailable))
                .ToList();
            if (hotels.Count()>=1)
                return hotels;
            throw new ObjectsNotAvailableException("hotels");
        }
        #endregion

        #region GetHotelById
        public async Task<HotelReturnDTO> GetHotelById(int hotelId)
        {
            try
            {
                var hotel = await _hotelRepository.Get(hotelId);
                return new HotelReturnDTO(hotel.HotelId, hotel.Name, hotel.Address, hotel.City, hotel.Rating, hotel.Amenities, hotel.Restrictions, hotel.IsAvailable);
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Hotel");
            }
        }
        #endregion

        #region AddHotel
        public async Task<HotelReturnDTO> RegisterHotel(HotelRegisterDTO hotelDTO)
        {
            try
            {
                var addedHotel = await _hotelRepository.Add( new Hotel(hotelDTO.Name, hotelDTO.Address, hotelDTO.City, hotelDTO.TotalNoOfRooms, 0,
                 hotelDTO.Amenities, hotelDTO.Restrictions, true) );
                return new HotelReturnDTO(addedHotel.HotelId, addedHotel.Name, addedHotel.Address, addedHotel.City, 
                     addedHotel.Rating, addedHotel.Amenities, addedHotel.Restrictions, addedHotel.IsAvailable);
            }
            catch (ObjectAlreadyExistsException)
            {
                throw new ObjectAlreadyExistsException("Hotel");
            }
            
        }       
        #endregion

        #region RemoveHotel
        public async Task<HotelReturnDTO> RemoveHotel(int hotelId)
        {
            try
            {
                var hotel = await _hotelRepository.Delete(hotelId);
                return new HotelReturnDTO(hotel.HotelId, hotel.Name, hotel.Address, hotel.City,   hotel.Rating, hotel.Amenities, hotel.Restrictions, hotel.IsAvailable);
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Hotel");
            }
            
        }
        #endregion

        #region UpdateHotelAttribute
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
        #endregion

        #region UpdateHotelAvailability
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
        #endregion

    }
}
