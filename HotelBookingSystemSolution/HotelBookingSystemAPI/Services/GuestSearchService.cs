using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using HotelBookingSystemAPI.Repositories;
using System.Linq;

namespace HotelBookingSystemAPI.Services
{
    public class GuestSearchService : IGuestSearchService
    {
        protected readonly IRepository<int, Hotel> _hotelRepository;
        protected readonly IRepository<int, Guest> _guestRepository;
        protected readonly IRepository<int, Room> _roomRepository;
        protected readonly IRepository<int, Booking> _bookingRepository;
        protected readonly IRepository<int, RoomType> _roomTypeRepository;
        protected readonly IRepositoryForCompositeKey<int, DateTime, HotelAvailabilityByDate> _hotelAvailability;

        public GuestSearchService(IRepository<int, Hotel> hotelRepository, IRepository<int, Room> roomRepository,IRepositoryForCompositeKey<int,DateTime, HotelAvailabilityByDate> hotelAvailabilityByDate,
            IRepository<int,RoomType> roomTypeRepository,  IRepository<int, Guest> guestRepository, IRepository<int,Booking> bookingRepository) {
            _hotelRepository = hotelRepository;  
            _roomRepository = roomRepository;
            _hotelAvailability = hotelAvailabilityByDate;
            _roomTypeRepository = roomTypeRepository;
            _guestRepository = guestRepository;
            _bookingRepository = bookingRepository;
        }

        #region GetAvailableRoomTypes
        public async Task<int> NoOfRoomsAvailableInThatType(List<Room> rooms, DateTime checkinDate, DateTime checkoutDate)
        {
            int count = 0;
            foreach (Room room in rooms)
            {
                //Date overlap check
                if (!room.roomsBooked.Any(rb => (checkinDate.Date < rb.CheckOutDate.Date && checkoutDate.Date>rb.CheckInDate.Date))){
                    count++;
                }
            }
            return count;
        }

        public async Task<List<AvailableRoomTypesDTO>> GetAvailableRoomTypesByHotel(SearchRoomsDTO searchRoomDTO)
        {
            try
            {
                var roomTypes = _hotelRepository.Get(searchRoomDTO.HotelId).Result.RoomTypes;
                var roomTypesDTO = new List<AvailableRoomTypesDTO>();
                foreach (var roomType in roomTypes)
                {
                    var roomsAvailableCount = NoOfRoomsAvailableInThatType(_roomRepository.Get().Result.Where(r => r.HotelId == searchRoomDTO.HotelId && r.RoomType.Type == roomType.Type && r.IsAvailable == true).ToList(), searchRoomDTO.CheckInDate, searchRoomDTO.CheckoutDate).Result;
                    roomTypesDTO.Add(new AvailableRoomTypesDTO(roomType.RoomTypeId,roomType.Type,roomsAvailableCount, roomType.Occupancy, roomType.Amount, roomType.Discount, roomType.Amenities));
                }
                return roomTypesDTO;
            }
            catch(ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Hotel");
            }
            
        }
        #endregion

        #region GetHotelsByLocation
        public async Task<List<AdminHotelReturnDTO>> GetHotelsByLocationAndDate(int limit, int skip,SearchHotelDTO hotelDTO)
        {

            var hotelsAvailable = _hotelRepository.Get().Result.Where(h => h.City.ToLower() == hotelDTO.Location.ToLower()).Skip(skip).Take(limit);
            if (hotelsAvailable.Count() > 0)
            {
                List<AdminHotelReturnDTO> hotels = hotelsAvailable.Select( hotel =>
                {
                    var hotelAvailability = _hotelAvailability.Get(hotel.HotelId, hotelDTO.Date).Result;
                    var isAvailable = hotelAvailability == null || hotelAvailability.RoomsAvailableCount > 0;
                    Dictionary<int,string> roomTypes = new Dictionary<int,string>();
                    foreach (var rt in hotel.RoomTypes)
                    {
                        roomTypes.Add(rt.RoomTypeId, rt.Type);
                    }
                    return new AdminHotelReturnDTO(hotel.HotelId, hotel.Name, hotel.Address, hotel.City, hotel.Rating, hotel.Ratings.Count() ,hotel.Amenities, hotel.Restrictions, isAvailable, hotel.TotalNoOfRooms,roomTypes);

                }).ToList();
                return hotels;
            }
            return new List<AdminHotelReturnDTO>();
        }

        public async Task<List<AdminHotelReturnDTO>> GetHotelsByRatings(int limit, int skip, SearchHotelDTO hotelDTO)
        {
            var hotels = await GetHotelsByLocationAndDate(limit, skip, hotelDTO);
            return hotels.OrderByDescending(h => h.Rating).ToList();
        }
        #endregion

        #region GetHotelsByFeature
        public async Task<List<AdminHotelReturnDTO>> GetHotelsByFeatures(int limit, int skip,List<string> features,SearchHotelDTO hotelDTO)
        {
            var hotels = await GetHotelsByLocationAndDate(limit, skip,hotelDTO);
            return  hotels.Where(hotel => features.All(feature => hotel.Amenities.Contains(feature))).ToList();
        }
        #endregion

        #region DescriptionOfRoomType
        public async Task<RoomTypeDescriptionDTO> GetDetailedDescriptionOfRoomType(int hotelId, string roomType)
        {
            var roomTypeRetrieved = _roomTypeRepository.Get().Result.FirstOrDefault(rt => rt.Type == roomType && rt.HotelId == hotelId);
            if (roomTypeRetrieved == null)
                throw new ObjectNotAvailableException("RoomType");
            return new RoomTypeDescriptionDTO(roomType, roomTypeRetrieved.Images, roomTypeRetrieved.Occupancy, roomTypeRetrieved.CotsAvailable, roomTypeRetrieved.Amenities);
        }
        #endregion

        #region GetRecommendedHotels
        public async Task<List<HotelRecommendationDTO>> HotelRecommendations(int loggedUser)
        {
            var booking = _bookingRepository.Get().Result.Where(b=>b.GuestId == loggedUser).ToList();
            if(booking.Count > 0)
            {
                var hotels = booking.Select(b => b.HotelId).Distinct().ToList();
                //recommended based on previously booked preferences roomtype, hotel
                var roomTypes = booking.SelectMany(b => b.RoomsBooked.Select(r => _roomRepository.Get(r.RoomId).Result.RoomType.Type)).Distinct().ToList();
                var roomsForRecomm = _roomTypeRepository.Get().Result.Where(r => roomTypes.Contains(r.Type) && r.Discount >= 0 && hotels.Contains(r.HotelId)).OrderByDescending(r => r.Discount).ToList();
                List<HotelRecommendationDTO> rooms = roomsForRecomm.Select(r => new HotelRecommendationDTO(r.HotelId,r.Hotel.Name, r.Hotel.Address, r.Hotel.City, r.Type, r.Discount)).ToList();
                return rooms;

            }
            //for first time user , the rooms with discount is recommended
            var roomsForRecommForFirstTimeUser = _roomTypeRepository.Get().Result.Where(r => r.Discount >= 0).OrderByDescending(r=>r.Discount).ToList();
            List<HotelRecommendationDTO> roomsRecommended = roomsForRecommForFirstTimeUser.Select(r => new HotelRecommendationDTO(r.HotelId,r.Hotel.Name, r.Hotel.Address, r.Hotel.City, r.Type, r.Discount)).ToList();
            return roomsRecommended;
        }
        #endregion
    }
}
