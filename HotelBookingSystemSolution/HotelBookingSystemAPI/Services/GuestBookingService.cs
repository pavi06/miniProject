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
        private readonly IRepository<int, Hotel> _hotelRepository;
        private readonly IRepository<int, Room> _roomRepository;
        private readonly IRepositoryForCompositeKey<int,DateTime, HotelAvailabilityByDate> _hotelAvailability;
        private readonly IRepository<int, Refund> _refundRepository;

        //static variable to access btw method calls
        protected static List<BookDetailsDTO> bookingRoomsList { get; set; }
        protected static BookingReturnDTO bookingDetails { get; set; }


        public GuestBookingService(IRepository<int, RoomType> roomTypeRepository, IRepository<int, Guest> guestRepository, IRepository<int, Payment> paymentRepository, 
            IRepository<int, Booking> bookingRepository, IRepositoryForCompositeKey<int, int, BookedRooms> bookedRoomsRepository, 
            IRepository<int, Hotel> hotelRepository, IRepository<int, Room> roomRepository, IRepositoryForCompositeKey<int, DateTime, HotelAvailabilityByDate> hotelAvailabilityByDate,
            IRepository<int,Refund> refundRepository)
        { 

            _roomTypeRepository = roomTypeRepository;
            _guestRepository = guestRepository;
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
            _bookedRoomsRepository = bookedRoomsRepository;
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
            _hotelAvailability = hotelAvailabilityByDate;
            _refundRepository = refundRepository;

        }


        public async Task<BookingReturnDTO> BookRooms(List<BookDetailsDTO> bookDetails, int loggedUserId, SearchRoomsDTO searchRooms)
        {
            bookingRoomsList = bookDetails;
            try
            {
                double finalAmount = 0.0;
                double totalAmount = 0.0;
                double discountPercent = 0.0;
                foreach (var roomtype in bookDetails)
                {
                    var roomType = _roomTypeRepository.Get().Result.FirstOrDefault(r=>r.Type==roomtype.RoomType);
                    totalAmount += roomType.Amount * roomtype.RoomsNeeded;
                    discountPercent += roomType.Discount;
                    finalAmount += (roomType.Amount - (roomType.Amount * (roomType.Discount / 100))) * roomtype.RoomsNeeded;
                }
                if (_guestRepository.Get(loggedUserId).Result.bookings.Count() > 3)
                {
                    finalAmount = finalAmount - (finalAmount * 0.05);
                    discountPercent += 5;
                }
                bookingDetails = new  BookingReturnDTO(searchRooms.HotelId, bookDetails.Sum(roomtype => roomtype.RoomsNeeded), searchRooms.CheckInDate, searchRooms.CheckoutDate, totalAmount, discountPercent, finalAmount, finalAmount / 2);
                return bookingDetails;
            }
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("RoomType");
            }
        }

        public async Task<PaymentReturnDTO> MakePayment(double amount, int loggedUser, SearchRoomsDTO searchRooms)
        {
            try
            {
                var payment = new Payment(amount, "InProcess - Advance Payment", "Online Payment");
                var payId = _paymentRepository.Add(payment).Result.PaymentId;
                //method call to confirm booking 
                var bookingConfirmation = await ConfirmBooking(bookingDetails, payId, loggedUser, searchRooms);
                return new PaymentReturnDTO(bookingConfirmation.BookingId, bookingConfirmation.BookingId > 0 ? $"Payment Successfull..\n{bookingConfirmation.Status}" : $"Payment Not Successfull\n{bookingConfirmation.Status}");
            }
            catch(Exception)
            {
                throw ;
            }

        }

        #region ConfirmBooking
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
                            //to reduce the iteration
                        }

                    }
                }
            }
        }

        #endregion

        public async Task<string> CancelBooking(int bookId, int loggedUserId)
        {
            try
            {
                var booking = await _bookingRepository.Get(bookId);
                var checkInDate = DateTime.MinValue;
                if (booking.RoomsBooked[0].CheckInDate == DateTime.Now)
                {
                    checkInDate = booking.RoomsBooked[0].CheckInDate;
                    return "You cannot cancel today!";
                }
                foreach (var bookedRoom in booking.RoomsBooked)
                {
                    //deleting the booked rooms and updating the hotel rooms availability counttry
                    try
                    {
                        await _bookedRoomsRepository.Delete(bookedRoom.RoomId, bookedRoom.BookingId);
                    }
                    catch(ObjectNotAvailableException)
                    {
                        throw new ObjectNotAvailableException("BookedRoom");
                    }
                    DateTime currentDate = bookedRoom.CheckInDate;
                    try
                    {
                        while (currentDate <= bookedRoom.CheckOutDate)
                        {
                            var res = await _hotelAvailability.Get(bookedRoom.Room.HotelId, currentDate);
                            res.RoomsAvailableCount += 1;
                            await _hotelAvailability.Update(res);
                        }
                    }
                    catch (ObjectNotAvailableException)
                    {
                        throw new Exception("Room is not booked on that day");
                    }

                }
                //updating the status of booking
                booking.BookingStatus = "Cancelled";
                await _bookingRepository.Update(booking);
                //calculate refund
                await RefundProcessingForCancellation(bookId, checkInDate, loggedUserId);
                return "Booking Canceled successfully!";
            }
            catch(ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Booking");
            }
        }

        public async Task RefundProcessingForCancellation(int bookId, DateTime checkInDate, int loggedUser)
        {
            var totalAmountForBooking = _bookingRepository.Get(bookId).Result.TotalAmount;
            var refundAmount = 0.0;
            switch((checkInDate - DateTime.Now).TotalDays)
            {
                case 1:
                    refundAmount = totalAmountForBooking / 2;
                    break;
                case 2:
                    refundAmount = totalAmountForBooking / 2;
                    break;
                case 3:
                    refundAmount = totalAmountForBooking / 10;
                    break;
                default:
                    refundAmount = totalAmountForBooking;
                    break;
            }
            await _refundRepository.Add(new Refund(loggedUser, bookId, Math.Round(refundAmount, 2)));
        }


        public async Task<List<MyBookingDTO>> GetMyBookings(int loggedUser)
        {
            var bookings = _guestRepository.Get(loggedUser).Result.bookings;
            //mapping each booking to dto
            List<MyBookingDTO> myBookings = bookings.Select(b => new MyBookingDTO
            {
                HotelId = b.HotelId,
                NoOfRoomsBooked = b.NoOfRooms,
                BookedDate = b.Date,
                TotalAmount = b.TotalAmount,
                DiscountPercent = b.Discount,
                FinalAmount = b.TotalAmount - (b.Discount / 100 * b.HotelId)
             }).ToList();
            return myBookings;
        }
    }
}
