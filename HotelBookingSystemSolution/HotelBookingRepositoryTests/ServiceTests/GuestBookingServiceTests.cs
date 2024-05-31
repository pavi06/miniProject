using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using HotelBookingSystemAPI.Repositories;
using HotelBookingSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace HotelBookingSystemAPIBLTests
{
    public class GuestBookingServiceTests
    {
        HotelBookingContext context;
        IRepository<int, RoomType> roomTypeRepository;
        IRepository<int, Guest> guestRepository;
        IRepository<int, Payment> paymentRepository;
        IRepository<int, Booking> bookingRepository;
        IRepositoryForCompositeKey<int, int, BookedRooms> bookedRoomsRepository;
        IRepository<int, Hotel> hotelRepository;
        IRepository<int, Room> roomRepository;
        IRepositoryForCompositeKey<int, DateTime, HotelAvailabilityByDate> hotelAvailability;
        IRepository<int, Refund> refundRepository;
        IGuestBookingService bookingService; 

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                         .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            roomTypeRepository = new RoomTypeRepository(context);
            bookingRepository = new BookingRepository(context);
            guestRepository = new GuestRepository(context);
            paymentRepository = new PaymentRepository(context);
            bookedRoomsRepository = new BookedRoomsRepository(context);
            hotelRepository = new HotelRepository(context);
            roomRepository = new RoomRepository(context);
            hotelAvailability = new HotelsAvailabilityRepository(context);
            bookingService = new GuestBookingService(roomTypeRepository, guestRepository, paymentRepository, bookingRepository,
                bookedRoomsRepository, hotelRepository, roomRepository, hotelAvailability, refundRepository
                );
            Guest guest = new Guest()
            {
                GuestId = 1,
                Name = "Pavi",
                Email = "pavi@gmail.com",
                PhoneNumber = "9784567845",
                Address = "No 4, Gandhi nagar, Chennai"
            };
            await guestRepository.Add(guest);

            Hotel hotel = new Hotel()
            {
                HotelId = 1,
                Name = "abc",
                Address = "chennai",
                City = "chennai",
                TotalNoOfRooms = 3,
                Rating = 4.5,
                Amenities = "Wifi, Tv",
                Restrictions = "No pets",
                IsAvailable = true
            };
            await hotelRepository.Add(hotel);

            Payment payment = new Payment()
            {
                PaymentId = 1,
                AmountPaid = 2500,
                PaymentStatus = "Successful - Full Payment Done",
                PaymentMode = "Online",
            };
            await paymentRepository.Add(payment);

            RoomType roomType = new RoomType()
            {
                RoomTypeId = 1,
                Type = "Standard",
                Occupancy = 4,
                Images = "jhghfgh",
                Amount = 2500,
                CotsAvailable = 2,
                Amenities = "Wifi, Parking",
                Discount = 0,
                HotelId = 1
            };
            await roomTypeRepository.Add(roomType);

            Booking booking = new Booking()
            {
                GuestId = 1,
                NoOfRooms = 1,
                TotalAmount = 5000,
                AdvancePayment = 2500,
                Discount = 2,
                BookingStatus = "Confirmed",
                PaymentId = 1,
                HotelId = 1
            };
            await bookingRepository.Add(booking);

            HotelAvailabilityByDate hotelAvail = new HotelAvailabilityByDate()
            {
                HotelId = 1,
                Date = Convert.ToDateTime("2024-06-10"),
                RoomsAvailableCount = 0,
            };
            await hotelAvailability.Add(hotelAvail);

            BookedRooms bookedRooms = new BookedRooms()
            {
                RoomId = 1,
                BookingId = 1,
                CheckInDate = Convert.ToDateTime("2024-06-10"),
                CheckOutDate = Convert.ToDateTime("2024-06-12")
            };
            await bookedRoomsRepository.Add(bookedRooms);

            Room room = new Room()
            {
                RoomId = 1,
                TypeId = 1,
                HotelId = 1,
                Images = "jgjhhjbj",
                Hotel= hotel,
                RoomType=roomType,
                IsAvailable=true,
                roomsBooked = new List<BookedRooms>() { bookedRooms },
            };
            await roomRepository.Add(room); 

        }

        [Test]
        public async Task BookRoomsPassTest()
        {
            List<BookDetailsDTO> details = new List<BookDetailsDTO>() { new BookDetailsDTO() { RoomType="standard",RoomsNeeded=1}};
            var res = bookingService.BookRooms(details,1, new SearchRoomsDTO(){ HotelId=1, CheckInDate=Convert.ToDateTime("2024-06-02"),CheckoutDate=Convert.ToDateTime("2024-06-04")});
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task BookRoomsExceptionTest()
        {
            List<BookDetailsDTO> details = new List<BookDetailsDTO>() { new BookDetailsDTO() { RoomType = "deluxe", RoomsNeeded = 1 } };
            var res = Assert.ThrowsAsync<ObjectNotAvailableException>(()=>bookingService.BookRooms(details, 2, new SearchRoomsDTO() { HotelId = 1, CheckInDate = Convert.ToDateTime("2024-06-02"), CheckoutDate = Convert.ToDateTime("2024-06-04") }));
            Assert.AreEqual("User Not available!", res.Message);
        }

 
        [Test]
        public async Task GetMyBookingPassTest()
        {
            Booking booking = new Booking()
            {
                GuestId = 1,
                NoOfRooms = 2,
                TotalAmount = 5000,
                AdvancePayment = 2500,
                Discount = 2,
                BookingStatus = "Confirmed",
                PaymentId = 1,
                HotelId = 1
            };
            await bookingRepository.Add(booking);
            var result = await bookingService.GetMyBookings(1);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetMyBookingFailTest()
        {
            
            var result = Assert.ThrowsAsync<AggregateException>(()=> bookingService.GetMyBookings(2));
            Assert.AreEqual("One or more errors occurred. (User Not available!)", result.Message);
        }

        [Test]
        public async Task CalculateRefundFailTest()
        {
            var result = await bookingService.CalculateRefundAmount(Convert.ToDateTime("2024-05-31"), 2500);
            Assert.AreEqual(2500, result);
        }

        [Test]
        public async Task CalculateRefundPassTest1()
        {
            var result = await bookingService.CalculateRefundAmount(Convert.ToDateTime("2024-05-30"), 2500);
            Assert.AreEqual(1250, result);
        }

        [Test]
        public async Task CalculateRefundPassTest2()
        {
            var result = await bookingService.CalculateRefundAmount(Convert.ToDateTime("2024-05-29"), 2500);
            Assert.AreEqual(500, result);
        }

        [Test]
        public async Task CalculateRefundPassTest3()
        {
            var result = await bookingService.CalculateRefundAmount(Convert.ToDateTime("2024-05-28"), 2500);
            Assert.AreEqual(250, result);
        }

        [Test]
        public async Task MakePaymentPassTest()
        {
            var res = bookingService.MakePayment(2500, 1, new SearchRoomsDTO() { HotelId=1,CheckInDate=Convert.ToDateTime("2024-06-10"), CheckoutDate=Convert.ToDateTime("2024-06-12")});
            Assert.IsNotNull(res);   
        }

        [Test]
        public async Task ModifyBookingPassTest()
        {
            var res = bookingService.ModifyBooking(1,1,new List<CancelRoomDTO>() { new CancelRoomDTO() { RoomType="standard", NoOfRoomsToCancel=1} });
            Assert.AreEqual("Cannot Modify instead proceed with cancel booking!", res.Result);
        }

        [Test]
        public async Task ModifyBookingPassTest2()
        {
            var res = bookingService.ModifyBooking(1, 1, new List<CancelRoomDTO>() { new CancelRoomDTO() { RoomType = "standard", NoOfRoomsToCancel = 1 } });
            Assert.AreEqual("Cannot modify booking", res.Result);
        }

        [Test]
        public async Task ModifyBookingExceptionTest()
        {
            var res = Assert.ThrowsAsync<AggregateException>(()=>bookingService.ModifyBooking(1, 5, new List<CancelRoomDTO>() { new CancelRoomDTO() { RoomType = "standard", NoOfRoomsToCancel = 1 } }));
            Assert.AreEqual("One or more errors occurred. (Booking Not available!)", res.Message);
        }

        [Test]
        public async Task CancelBookingPassTest()
        {
            var res = bookingService.CancelBooking(1, 1);
            Assert.AreEqual("You cannot cancel today!",res.Result);
        }

        [Test]
        public async Task CancelBookingPassTests2()
        {
            var res = bookingService.CancelBooking(1, 1);
            Assert.AreEqual("Booking Canceled successfully!", res.Result);
        }

        [Test]
        public async Task CancelBookingExceptionTest()
        {
            var res = Assert.ThrowsAsync<ObjectNotAvailableException>(()=>bookingService.CancelBooking(3, 1));
            Assert.AreEqual("Booking Not available!", res.Message);
        }

        //[TearDown]
        //public void TearDown()
        //{
        //    context.Dispose();
        //}

    }
}
