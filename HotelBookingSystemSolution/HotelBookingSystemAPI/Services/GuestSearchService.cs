using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;

namespace HotelBookingSystemAPI.Services
{
    public class GuestSearchService : IGuestSearchService
    {
        protected readonly IRepository<int, Hotel> _hotelRepository;
        protected readonly IRepository<int, Room> _roomRepository;
        protected readonly IRepositoryForCompositeKey<int, DateTime, HotelAvailabilityByDate> _hotelAvailability;
        protected List<BookDetailsDTO> bookingDetailsDTO;
        protected SearchRoomsDTO searchRoom;

        public GuestSearchService(IRepository<int, Hotel> hotelRepository, IRepository<int, Room> roomRepository,IRepositoryForCompositeKey<int,DateTime, HotelAvailabilityByDate> hotelAvailabilityByDate) {
            _hotelRepository = hotelRepository;  
            _roomRepository = roomRepository;
            _hotelAvailability = hotelAvailabilityByDate;
        }

        public async Task<int> NoOfRoomsAvailableInThatType(List<Room> rooms, DateTime checkinDate, DateTime checkoutDate)
        {
            int count = 0;
            foreach (Room room in rooms)
            {
                if (!room.roomsBooked.Any(rb => (checkinDate>= rb.CheckInDate && checkinDate< rb.CheckOutDate) && (checkoutDate >= rb.CheckInDate && checkoutDate < rb.CheckOutDate))){
                    count++;
                }
            }
            return count;
        }

        public async Task<List<AvailableRoomTypesDTO>> GetAvailableRoomTypesByHotel(SearchRoomsDTO searchRoomDTO)
        {
            searchRoom = searchRoomDTO;
            try
            {
                var roomTypes = _hotelRepository.Get(searchRoomDTO.HotelId).Result.RoomTypes;
                var roomTypesDTO = new List<AvailableRoomTypesDTO>();
                foreach (var roomType in roomTypes)
                {
                    var roomsAvailableCount = NoOfRoomsAvailableInThatType(_roomRepository.Get().Result.Where(r => r.HotelId == searchRoomDTO.HotelId).ToList(), searchRoomDTO.CheckInDate, searchRoomDTO.CheckoutDate).Result;
                    roomTypesDTO.Append(new AvailableRoomTypesDTO(roomType.Type, roomsAvailableCount, roomType.Occupancy, roomType.Amount, roomType.CotsAvailable, roomType.Amenities, roomType.Discount));
                }
                return roomTypesDTO;
            }
            catch(ObjectNotAvailableException e)
            {
                throw e;
            }
            
        }

        public async Task<List<HotelReturnDTO>> GetHotelsByLocationAndDate(SearchHotelDTO hotelDTO)
        {
            var hotels = new List<HotelReturnDTO>();
                var hotelsAvailable = _hotelRepository.Get().Result.Where(h => h.City == hotelDTO.Location);
                if (hotelsAvailable.Count() > 0)
                {
                    foreach (var hotel in hotelsAvailable)
                    {
                        if (_hotelAvailability.Get(hotel.HotelId, hotelDTO.Date).Result.RoomsAvailableCount > 0)                        
                        {
                            hotel.IsAvailable = true;
                        }
                        else
                        {
                            hotel.IsAvailable = false;
                        }
                        hotels.Add(new HotelReturnDTO(hotel.HotelId, hotel.Name, hotel.Address, hotel.City, hotel.Rating, hotel.Amenities, hotel.Restrictions, hotel.IsAvailable));
                    }
                }
                throw new ObjectsNotAvailableException("hotel");
        }

        public async Task<List<HotelReturnDTO>> GetHotelsByRatings(SearchHotelDTO hotelDTO)
        {
            var hotels = await GetHotelsByLocationAndDate(hotelDTO);
            return hotels.OrderBy(h => h.Rating).ToList();
        }

    }
}
