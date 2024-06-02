using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;
using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;
using HotelBookingSystemAPI.Repositories;

namespace HotelBookingSystemAPI.Services
{
    public class HotelEmployeeService : IHotelEmployeeService
    {
        private readonly IRepository<int, Guest> _guestRepository;
        private readonly IRepository<int, Room> _roomRepository;
        private readonly IRepository<int, Booking> _bookingRepository; 
        private readonly ILogger<HotelEmployeeService> _logger; 

        public HotelEmployeeService(IRepository<int, Guest> guestRepository, IRepository<int, Room> roomRepository,
            IRepository<int,Booking> bookingRepository, ILogger<HotelEmployeeService> logger) {
            _guestRepository = guestRepository;
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
            _logger = logger;
        }

        #region GetAllBookingRequest
        public async Task<List<BookingDetailsForEmployeeDTO>> GetAllBookingRequest(int loggedUserWorksFor)
        {
            var bookings =  _bookingRepository.Get().Result.Where(b=>b.HotelId == loggedUserWorksFor).ToList();
            var mappedValue =  await MapBookingDetailsForEmployee(bookings);
            if(mappedValue.Count>0 )
            {
                _logger.LogInformation("Bookings retrieved");
                return mappedValue;
            }
            _logger.LogInformation("No bookings are available");
            throw new ObjectsNotAvailableException("Bookings");
        }
        #endregion

        #region GetBookingsByDateAndMonth
        public async Task<List<BookingDetailsForEmployeeDTO>> GetAllBookingRequestByFilteration(int loggedUserWorksFor, string attribute, string attributeValue)
        {
            List<Booking> bookings = null;
            switch (attribute.ToLower())
            {
                case "month":
                    bookings = _bookingRepository.Get().Result.Where(b => b.HotelId == loggedUserWorksFor && b.Date.Month == Convert.ToInt32(attributeValue)).ToList();
                    break;
                case "date":
                    bookings = _bookingRepository.Get().Result.Where(b => b.HotelId == loggedUserWorksFor && b.Date.Date == Convert.ToDateTime(attributeValue).Date).ToList();
                    break;
                default:
                    throw new Exception("Invalid Attribute value");
            }
            _logger.LogInformation("Bookings retrieved");
            return await MapBookingDetailsForEmployee(bookings);
        }
        #endregion

        #region GetAllBookingDoneToday
        public async Task<List<BookingDetailsForEmployeeDTO>> GetAllBookingRequestDoneToday(int loggedUserWorksFor)
        {
                //get all bookings for that hotel
            var bookings = _bookingRepository.Get().Result.Where(b => b.HotelId == loggedUserWorksFor && b.Date.Date == DateTime.Now.Date).ToList();
            _logger.LogInformation("Bookings retrieved");
            return await MapBookingDetailsForEmployee(bookings);
            
        }
        #endregion

        #region TodayCheckIn
        public async Task<List<GuestDetailsForCheckInDTO>> GetAllCheckInForToday(int loggedUserWorksFor)
        {
            var guestDetails = new List<GuestDetailsForCheckInDTO>();
            var bookings = _bookingRepository.Get().Result.Where(b => b.HotelId == loggedUserWorksFor);
            foreach(var booking in bookings)
            {
                if(booking.RoomsBooked.Any(b => b.CheckInDate.Date == DateTime.Now.Date))
                {
                    var guest = await _guestRepository.Get(booking.GuestId);
                    guestDetails.Add(new GuestDetailsForCheckInDTO(guest.Name, guest.PhoneNumber));
                }
            }
            if(guestDetails.Count() > 0)
            {
                _logger.LogInformation("Guest details retrieved");
                return guestDetails;
            }
            _logger.LogInformation("No bookings are available");
            throw new ObjectsNotAvailableException("Booking");
        }
        #endregion

        #region BookingDetailsMapper
        public async Task<List<BookingDetailsForEmployeeDTO>> MapBookingDetailsForEmployee(List<Booking> bookings)
        {
            var BookingRequests = new List<BookingDetailsForEmployeeDTO>();
            foreach (var booking in bookings)
            {
                var guest = await _guestRepository.Get(booking.GuestId);
                //mapping roomtype and count 
                var roomTypeAndCount = booking.RoomsBooked
                    .Select(rb => _roomRepository.Get(rb.RoomId).Result.RoomType.Type)
                    .GroupBy(type => type)
                    .ToDictionary(g => g.Key, g => g.Count());
                BookingRequests.Add(new BookingDetailsForEmployeeDTO(booking.BookId, guest.Name, guest.PhoneNumber, roomTypeAndCount, booking.BookingStatus));
            }
            _logger.LogInformation("Bookings retrieved");
            return BookingRequests;

        }
        #endregion

    }
}
