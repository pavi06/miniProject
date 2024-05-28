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
        private readonly IRepository<int, Hotel> _hotelRepository;
        private readonly IRepository<int, Guest> _guestRepository;
        private readonly IRepository<int, Room> _roomRepository;
        private readonly IRepository<int, Booking> _bookingRepository;
        private readonly IRepositoryForCompositeKey<int, DateTime, BookedRooms> _bookedRoomsRepository;

        public HotelEmployeeService(IRepository<int,Hotel> hotelRepository, IRepository<int, Guest> guestRepository, IRepository<int, Room> roomRepository,
            IRepositoryForCompositeKey<int,DateTime,BookedRooms> bookedRoomsRepository, IRepository<int,Booking> bookingRepository) {
            _hotelRepository = hotelRepository;
            _guestRepository = guestRepository;
            _roomRepository = roomRepository;
            _bookedRoomsRepository = bookedRoomsRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<List<BookingDetailsForEmployeeDTO>> GetAllBookingRequestDoneToday(int loggedUserWorksFor)
        {
            var bookings = _hotelRepository.Get(loggedUserWorksFor).Result.bookingsForHotel.Where(b=>b.Date == DateTime.Now);
            var todayBookingRequests = new List<BookingDetailsForEmployeeDTO>();
            foreach(var booking in bookings)
            {
                var guest = await _guestRepository.Get(booking.GuestId);
                Dictionary<string, int> roomTypeAndCount = new Dictionary<string, int>();
                foreach(var room in booking.RoomsBooked)
                {
                    var key = _roomRepository.Get(room.RoomId).Result.RoomType.Type;
                    if (roomTypeAndCount.ContainsKey(key))
                    {
                        roomTypeAndCount[key] += 1;
                    }
                    else
                    {
                        roomTypeAndCount.Add(key, 1);
                    }
                }
                todayBookingRequests.Add(new BookingDetailsForEmployeeDTO(booking.BookId, guest.Name, guest.PhoneNumber, roomTypeAndCount));
            }
            return todayBookingRequests;
        }

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
            throw new ObjectsNotAvailableException("Booking");
        }

    }
}
