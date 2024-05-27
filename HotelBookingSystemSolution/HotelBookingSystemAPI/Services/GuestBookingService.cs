using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using HotelBookingSystemAPI.Repositories;
using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;

namespace HotelBookingSystemAPI.Services
{
    public class GuestBookingService : IGuestBookingService
    {
        private readonly IRepository<int, RoomType> _roomTypeRepository;
        private readonly IRepository<int, Guest> _guestRepository;
        private readonly IRepository<int, Payment> _paymentRepository;
        private readonly IRepository<int, Booking> _bookingRepository;
        private readonly IRepositoryForCompositeKey<int, int, BookedRooms> _bookedRoomsRepository;
        private readonly IRepository<int, Rating> _ratingRepository;
        private readonly IRepository<int, Hotel> _hotelRepository;
        private readonly IRepository<int, Room> _roomRepository;
        private readonly IRepositoryForCompositeKey<int,DateTime, HotelAvailabilityByDate> _hotelAvailability;
        protected static List<BookDetailsDTO> bookingRoomsList { get; set; }
        protected static BookingReturnDTO bookingDetails { get; set; }


        public GuestBookingService(IRepository<int, RoomType> roomTypeRepository, IRepository<int, Guest> guestRepository, IRepository<int, Payment> paymentRepository, 
            IRepository<int, Booking> bookingRepository, IRepositoryForCompositeKey<int, int, BookedRooms> bookedRoomsRepository, IRepository<int,Rating> ratingRepository, 
            IRepository<int, Hotel> hotelRepository, IRepository<int, Room> roomRepository, IRepositoryForCompositeKey<int, DateTime, HotelAvailabilityByDate> hotelAvailabilityByDate)
        { 

            _roomTypeRepository = roomTypeRepository;
            _guestRepository = guestRepository;
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
            _bookedRoomsRepository = bookedRoomsRepository;
            _ratingRepository = ratingRepository;
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
            _hotelAvailability = hotelAvailabilityByDate;

        }


        public async Task<BookingReturnDTO> BookRooms(List<BookDetailsDTO> bookDetails, int loggedUserId, SearchRoomsDTO searchRooms)
        {
            bookingRoomsList = bookDetails;
            try
            {
                double finalAmount = 0.0;
                int roomsCount = 0;
                double totalAmount = 0.0;
                double discountPercent = 0.0;
                foreach (var roomtype in bookDetails)
                {
                    var roomType = _roomTypeRepository.Get().Result.FirstOrDefault(r=>r.Type==roomtype.RoomType);
                    totalAmount += roomType.Amount * roomtype.RoomsNeeded;
                    roomsCount += roomtype.RoomsNeeded;
                    discountPercent += roomType.Discount;
                    finalAmount += (roomType.Amount - (roomType.Amount * (roomType.Discount / 100))) * roomtype.RoomsNeeded;
                }
                if (_guestRepository.Get(loggedUserId).Result.bookings.Count() > 3)
                {
                    finalAmount = finalAmount - (finalAmount * 0.05);
                    discountPercent += 5;
                }
                bookingDetails = new  BookingReturnDTO(searchRooms.HotelId,roomsCount, searchRooms.CheckInDate, searchRooms.CheckoutDate, totalAmount, discountPercent, finalAmount, finalAmount / 2);
                return bookingDetails;
            }
            catch (ObjectNotAvailableException)
            {
                throw ;
            }
        }

        public async Task<PaymentReturnDTO> MakePayment(double amount, int loggedUser, SearchRoomsDTO searchRooms)
        {
            var payment = new Payment(amount, "InProcess - Advance Payment", "Online Payment");
            var res = _paymentRepository.Add(payment).Result.PaymentId;
            var bookingConfirmation = await ConfirmBooking(bookingDetails,res, loggedUser,searchRooms);
            return new PaymentReturnDTO(bookingConfirmation.BookingId ,bookingConfirmation.BookingId> 0 ? $"Payment Successfull..\n{bookingConfirmation.Status}" : $"Payment Not Successfull\n{bookingConfirmation.Status}");

        }


        public async Task<BookingConfirmationDTO> ConfirmBooking(BookingReturnDTO bookingDetails, int payId, int loggedUser, SearchRoomsDTO searchRooms)
        {
            if(payId > 0)
            {
                var addBooking = new Booking(loggedUser, bookingDetails.NoOfRoomsBooked, bookingDetails.FinalAmount, bookingDetails.AdvancePayment,
                bookingDetails.DiscountPercent, "Confirmed", payId, searchRooms.HotelId);
                var bookId = _bookingRepository.Add(addBooking).Result.BookId;
                await UpdateHotelAvailability(bookingDetails, searchRooms);
                await AllocateRooms(bookId, searchRooms);
                return new BookingConfirmationDTO(bookId,"Booking Confirmed!");
            }
            return new BookingConfirmationDTO(-1,"Booking process Failed!");

        }

        public async Task UpdateHotelAvailability(BookingReturnDTO bookingDetails, SearchRoomsDTO searchRooms)
        {
            
            DateTime currentDate = bookingDetails.CheckInDate;
            while (currentDate <= bookingDetails.CheckOutDate)
            {
                await _hotelAvailability.Add(new HotelAvailabilityByDate(searchRooms.HotelId, currentDate, _hotelRepository.Get(searchRooms.HotelId).Result.TotalNoOfRooms - bookingDetails.NoOfRoomsBooked));
                currentDate = currentDate.AddDays(1);
            }
        }

        public async Task AllocateRooms(int bookId, SearchRoomsDTO searchRooms)
        {
            foreach (var roomType in bookingRoomsList)
            {
                var rooms = _roomRepository.Get().Result.Where(r => r.HotelId == searchRooms.HotelId).Where(r => r.RoomType.Type == roomType.RoomType).ToList();
                for (int i = 0; i < roomType.RoomsNeeded; i++)
                {
                    foreach (var room in rooms)
                    {
                        if (!room.roomsBooked.Any(r => (searchRooms.CheckInDate >= r.CheckInDate && searchRooms.CheckInDate < r.CheckOutDate) && (searchRooms.CheckoutDate >= r.CheckInDate && searchRooms.CheckoutDate < r.CheckOutDate)))
                        {
                            BookedRooms bkroom = new BookedRooms(room.RoomId, bookId, searchRooms.CheckInDate, searchRooms.CheckoutDate);
                            await _bookedRoomsRepository.Add(bkroom);
                            break;
                        }
                        else
                        {
                            rooms.Remove(room);
                        }

                    }
                }
            }
        }

        public async Task<string> CancelBooking(int bookId)
        {
            try
            {
                var booking = await _bookingRepository.Get(bookId);
                foreach (var bookedRoom in booking.RoomsBooked)
                {
                    if (DateTime.Now == bookedRoom.CheckInDate)
                    {
                        return "You cannot cancel today!";
                    }
                    await _bookedRoomsRepository.Delete(bookedRoom.RoomId, bookedRoom.BookingId);
                    DateTime currentDate = bookedRoom.CheckInDate;
                    while (currentDate <= bookedRoom.CheckOutDate)
                    {
                        var res = await _hotelAvailability.Get(bookedRoom.Room.HotelId, currentDate);
                        res.RoomsAvailableCount += 1;
                        await _hotelAvailability.Update(res);
                    }

                }
                booking.BookingStatus = "Cancelled";
                await _bookingRepository.Update(booking);
                return "Booking Canceled successfully!";
            }
            catch(ObjectNotAvailableException)
            {
                throw ;
            }
        }


        public async Task<List<MyBookingDTO>> GetMyBookings(int loggedUser)
        {
            var bookings = _guestRepository.Get(loggedUser).Result.bookings;
            var bookingToBookingReturnMapper = new List<MyBookingDTO>();
            foreach (var booking in bookings)
            {
                bookingToBookingReturnMapper.Add(new MyBookingDTO(booking.HotelId, booking.NoOfRooms, booking.Date, booking.TotalAmount, booking.Discount, booking.TotalAmount - booking.Discount / 100 * booking.HotelId));
            }
            return bookingToBookingReturnMapper;
        }
    }
}
