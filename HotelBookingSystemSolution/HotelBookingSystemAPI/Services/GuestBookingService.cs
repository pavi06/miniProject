using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using HotelBookingSystemAPI.Repositories;
using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
        private readonly IRepositoryForCompositeKey<int, DateTime, HotelAvailabilityByDate> _hotelAvailability;
        private readonly IRepository<int, Refund> _refundRepository;

        //static variable to access btw method calls
        protected static List<BookDetailsDTO> bookingRoomsList { get; set; }
        protected static BookingReturnDTO bookingDetails { get; set; }


        public GuestBookingService(IRepository<int, RoomType> roomTypeRepository, IRepository<int, Guest> guestRepository, IRepository<int, Payment> paymentRepository,
            IRepository<int, Booking> bookingRepository, IRepositoryForCompositeKey<int, int, BookedRooms> bookedRoomsRepository,
            IRepository<int, Hotel> hotelRepository, IRepository<int, Room> roomRepository, IRepositoryForCompositeKey<int, DateTime, HotelAvailabilityByDate> hotelAvailabilityByDate,
            IRepository<int, Refund> refundRepository)
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
                    var roomType = _roomTypeRepository.Get().Result.FirstOrDefault(r => r.Type.ToLower() == roomtype.RoomType.ToLower());
                    totalAmount += roomType.Amount * roomtype.RoomsNeeded;
                    discountPercent += roomType.Discount;
                    finalAmount += (roomType.Amount - (roomType.Amount * (roomType.Discount / 100))) * roomtype.RoomsNeeded;
                }
                if (_guestRepository.Get(loggedUserId).Result.bookings.Count() > 3)
                {
                    finalAmount = finalAmount - (finalAmount * 0.05);
                    discountPercent += 5;
                }
                bookingDetails = new BookingReturnDTO(searchRooms.HotelId, bookDetails.Sum(roomtype => roomtype.RoomsNeeded), searchRooms.CheckInDate, searchRooms.CheckoutDate, totalAmount, discountPercent, finalAmount, finalAmount / 2);
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
                Payment payment = null; 
                if(amount == bookingDetails.TotalAmount)
                    payment = new Payment(amount, "Successful - Full Payment Done", "Online Payment");
                else
                    payment = new Payment(amount, "InProcess - Advance Payment", "Online Payment");
                var payId = _paymentRepository.Add(payment).Result.PaymentId;
                //method call to confirm booking 
                var bookingConfirmation = await ConfirmBooking(bookingDetails, payId, loggedUser, searchRooms);
                return new PaymentReturnDTO(bookingConfirmation.BookingId, bookingConfirmation.BookingId > 0 ? $"Payment Successfull..\n{bookingConfirmation.Status}" : $"Payment Not Successfull\n{bookingConfirmation.Status}");
            }
            catch (Exception)
            {
                throw;
            }

        }

        #region ConfirmBooking
        public async Task<BookingConfirmationDTO> ConfirmBooking(BookingReturnDTO bookingDetails, int payId, int loggedUser, SearchRoomsDTO searchRooms)
        {
            if (payId > 0)
            {
                var addBooking = new Booking(loggedUser, bookingDetails.NoOfRoomsBooked, bookingDetails.FinalAmount, bookingDetails.AdvancePayment,
                bookingDetails.DiscountPercent, "Confirmed", payId, searchRooms.HotelId);
                var bookId = _bookingRepository.Add(addBooking).Result.BookId;
                await UpdateHotelAvailability(bookingDetails, searchRooms);
                await AllocateRooms(bookId, searchRooms);
                return new BookingConfirmationDTO(bookId, "Booking Confirmed!");
            }
            return new BookingConfirmationDTO(-1, "Booking process Failed!");

        }

        public async Task UpdateHotelAvailability(BookingReturnDTO bookingDetails, SearchRoomsDTO searchRooms)
        {

            DateTime currentDate = bookingDetails.CheckInDate;
            while (currentDate <= bookingDetails.CheckOutDate)
            {
                await _hotelAvailability.Add(new HotelAvailabilityByDate(searchRooms.HotelId, currentDate, _hotelRepository.Get(searchRooms.HotelId).Result.TotalNoOfRooms - bookingDetails.NoOfRoomsBooked - _roomRepository.Get().Result.Count(r=>r.IsAvailable == false)));
                currentDate = currentDate.AddDays(1);
            }
        }

        public async Task AllocateRooms(int bookId, SearchRoomsDTO searchRooms)
        {
            foreach (var roomType in bookingRoomsList)
            {
                //check availability
                var rooms = _roomRepository.Get().Result.Where(r => r.HotelId == searchRooms.HotelId).Where(r => r.RoomType.Type.ToLower() == roomType.RoomType.ToLower() && r.IsAvailable == true).ToList();
                for (int i = 0; i < roomType.RoomsNeeded; i++)
                {
                    foreach (var room in rooms)
                    {
                        if (!room.roomsBooked.Any(r => (searchRooms.CheckInDate >= r.CheckInDate && searchRooms.CheckInDate < r.CheckOutDate) && (searchRooms.CheckoutDate >= r.CheckInDate && searchRooms.CheckoutDate < r.CheckOutDate)))
                        {
                            BookedRooms bkroom = new BookedRooms(room.RoomId, bookId, searchRooms.CheckInDate, searchRooms.CheckoutDate);
                            await _bookedRoomsRepository.Add(bkroom);
                            rooms.Remove(room);
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
                    catch (ObjectNotAvailableException)
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
            catch (ObjectNotAvailableException)
            {
                throw new ObjectNotAvailableException("Booking");
            }
        }

        public async Task RefundProcessingForCancellation(int bookId, DateTime checkInDate, int loggedUser)
        {
            var totalAmountForBooking = _bookingRepository.Get(bookId).Result.TotalAmount;           
            await _refundRepository.Add(new Refund(loggedUser, bookId, Math.Round(await CalculateRefundAmount(checkInDate,totalAmountForBooking), 2)));
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

        public async Task<string> ModifyBooking(int loggedUser, int bookingId, List<CancelRoomDTO> cancelRoom)
        {
            if(cancelRoom.Sum(r=>r.NoOfRoomsToCancel) == _bookingRepository.Get(bookingId).Result.RoomsBooked.Count())
            {
                return "Cannot Modify instead proceed with cancel booking!";
            }
            try
            {
                var roomsBooked = _bookedRoomsRepository.Get().Result.Where(br => br.BookingId == bookingId).ToList();
                var totalAmount = 0.0;
                foreach (var room in cancelRoom)
                {
                    var res = roomsBooked.Where(rb => _roomRepository.Get(rb.RoomId).Result.RoomType.Type == room.RoomType).Take(room.NoOfRoomsToCancel).ToList();
                    totalAmount += _roomRepository.Get(res[0].RoomId).Result.RoomType.Amount * room.NoOfRoomsToCancel;
                    foreach (var r in res)
                    {
                        _bookedRoomsRepository.Delete(r.RoomId, r.BookingId);
                    }
                    DateTime currentDate = roomsBooked[0].CheckInDate;
                    while (currentDate <= roomsBooked[0].CheckOutDate)
                    {
                        var hotelAvailability = _hotelAvailability.Get(_bookingRepository.Get(bookingId).Result.HotelId, currentDate).Result;
                        hotelAvailability.RoomsAvailableCount += res.Count();
                        currentDate.AddDays(1);
                    }

                }
                var bookedObj = _bookingRepository.Get(bookingId).Result;
                bookedObj.NoOfRooms -= cancelRoom.Sum(r => r.NoOfRoomsToCancel);
                bookedObj.TotalAmount -= totalAmount; 
                var updatedBooking = await _bookingRepository.Update(bookedObj);
                if (updatedBooking != null)
                {
                    await calculateRefundForRoomCancel(loggedUser, updatedBooking, totalAmount);
                    return $"Canceled successfully..\nRooms Booked - {updatedBooking.NoOfRooms}";
                }
                throw new ObjectNotAvailableException("Booking");
            }
            catch (ObjectNotAvailableException )
            {
                throw;
            }
            
        }

        public async Task calculateRefundForRoomCancel(int loggedUser, Booking updatedBooking, double totalAmount)
        {
            if(_paymentRepository.Get((int)updatedBooking.PaymentId).Result.PaymentStatus == "Successful - Full Payment Done")
            {
                await _refundRepository.Add(new Refund(loggedUser, updatedBooking.BookId, Math.Round(await CalculateRefundAmount(updatedBooking.RoomsBooked[0].CheckInDate, totalAmount), 2)));
            }
        }

        public async Task<double> CalculateRefundAmount(DateTime checkInDate, double totalAmount)
        {
            var refundAmount = 0.0;
            switch ((checkInDate - DateTime.Now).TotalDays)
            {
                case 1:
                    refundAmount = totalAmount / 2;
                    break;
                case 2:
                    refundAmount = totalAmount / 2;
                    break;
                case 3:
                    refundAmount = totalAmount / 10;
                    break;
                default:
                    refundAmount = totalAmount;
                    break;
            }
            return refundAmount;
        }
    }
}
