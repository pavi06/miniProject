using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repositories;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using HotelBookingSystemAPI.Models.DTOs;

namespace HotelBookingSystemAPI.Services
{
    public class GuestBookingService :  GuestSearchService, IGuestBookingService
    {
        private readonly IRepository<int, RoomType> _roomTypeRepository;
        private readonly IRepository<int, Guest> _guestRepository;
        private readonly IRepository<int, Payment> _paymentRepository;
        private readonly IRepository<int, Booking> _bookingRepository;
        private readonly IRepositoryForCompositeKey<int, int, BookedRooms> _bookedRoomsRepository;
        private readonly IRepository<int, Rating> _ratingRepository;

        public GuestBookingService(IRepository<int, Hotel> hotelRepository, IRepository<int, Room> roomRepository, IRepositoryForCompositeKey<int, DateTime, HotelAvailabilityByDate> hotelAvailabilityByDate,IRepository<int, RoomType> roomTypeRepository, IRepository<int, Guest> guestRepository, IRepository<int, Payment> paymentRepository, 
            IRepository<int, Booking> bookingRepository, IRepositoryForCompositeKey<int, int, BookedRooms> bookedRoomsRepository, IRepository<int,Rating> ratingRepository):base(hotelRepository, roomRepository, hotelAvailabilityByDate) {

            _roomTypeRepository = roomTypeRepository;
            _guestRepository = guestRepository;
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
            _bookedRoomsRepository = bookedRoomsRepository;
            _ratingRepository = ratingRepository;

        }


        public async Task<BookingReturnDTO> BookRooms(List<BookDetailsDTO> bookDetails, int loggedUserId)
        {
            try
            {
                double finalAmount = 0.0;
                int roomsCount = 0;
                double totalAmount = 0.0;
                double discountPercent = 0.0;
                foreach (var book in bookDetails)
                {
                    var roomType = _roomTypeRepository.Get((int)book.RoomType).Result;
                    totalAmount += roomType.Amount * book.RoomsNeeded;
                    roomsCount += book.RoomsNeeded;
                    discountPercent += roomType.Discount;
                    finalAmount += (roomType.Amount - (roomType.Amount * (roomType.Discount / 100))) * book.RoomsNeeded;
                }
                if (_guestRepository.Get(loggedUserId).Result.bookings.Count() > 3)
                {
                    finalAmount = finalAmount - (finalAmount * 0.05);
                    discountPercent += 5;
                }
                return new BookingReturnDTO(roomsCount, searchRoom.CheckInDate, searchRoom.CheckoutDate, totalAmount, discountPercent, finalAmount, finalAmount / 2);
            }
            catch (ObjectNotAvailableException e)
            {
                throw e;
            }
        }

        public async Task<int> MakePayment(double amount)
        {
            var payment = new Payment(amount, "InProcess - Advance Payment", "GPay");
            return _paymentRepository.Add(payment).Result.PaymentId;

        }

        public async Task<string> ConfirmBooking(BookingReturnDTO bookingDetails, int payId, int loggedUser)
        {
            var addBooking = new Booking(loggedUser, bookingDetails.NoOfRoomsBooked, bookingDetails.FinalAmount, bookingDetails.AdvancePayment,
                bookingDetails.DiscountPercent, "Confirmed", payId);
            var bookId = _bookingRepository.Add(addBooking).Result.BookId;
            await UpdateHotelAvailability(bookingDetails);
            await AllocateRooms(bookId);
            return "Booking Confirmed";

        }

        public async Task UpdateHotelAvailability(BookingReturnDTO bookingDetails)
        {
            
            DateTime currentDate = bookingDetails.CheckInDate;
            while (currentDate <= bookingDetails.CheckOutDate)
            {
                await _hotelAvailability.Add(new HotelAvailabilityByDate(searchRoom.HotelId, currentDate, _hotelRepository.Get(searchRoom.HotelId).Result.TotalNoOfRooms - bookingDetails.NoOfRoomsBooked));
                currentDate = currentDate.AddDays(1);
            }
        }

        public async Task AllocateRooms(int bookId)
        {
            foreach (var roomType in bookingDetailsDTO)
            {
                var rooms = _roomRepository.Get().Result.Where(r => r.HotelId == searchRoom.HotelId).Where(r => r.RoomType.Type == roomType.RoomType).ToList();
                for (int i = 0; i < roomType.RoomsNeeded; i++)
                {
                    foreach (var room in rooms)
                    {
                        if (!room.roomsBooked.Any(r => (searchRoom.CheckInDate >= r.CheckInDate && searchRoom.CheckInDate < r.CheckOutDate) && (searchRoom.CheckoutDate >= r.CheckInDate && searchRoom.CheckoutDate < r.CheckOutDate)))
                        {
                            BookedRooms bkroom = new BookedRooms(room.RoomId, bookId, searchRoom.CheckInDate, searchRoom.CheckoutDate);
                            await _bookedRoomsRepository.Add(bkroom);
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
            catch(ObjectNotAvailableException e)
            {
                throw e;
            }
        }

        public async Task<string> ProvideRating(AddRatingDTO ratingDTO, int loggedUser)
        {
            try
            {
                Rating rating = new Rating(loggedUser, ratingDTO.HotelId, ratingDTO.ReviewRating, ratingDTO.Comments);
                _ratingRepository.Add(rating);
                var hotel = _hotelRepository.Get(ratingDTO.HotelId).Result;
                hotel.Rating = (hotel.Rating + ratingDTO.ReviewRating) / hotel.Ratings.Count();
                await _hotelRepository.Update(hotel);
                return "Thanks for your rating!";
            }
            catch (ObjectNotAvailableException e)
            {
                throw e;
            }
            
        }
    }
}
