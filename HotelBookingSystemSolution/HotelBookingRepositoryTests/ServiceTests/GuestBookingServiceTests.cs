using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repositories;
using HotelBookingSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public void Setup()
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
            guestRepository.Add(guest);

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
            hotelRepository.Add(hotel);

            Payment payment = new Payment()
            {
                PaymentId = 1,
                AmountPaid = 2500,
                PaymentStatus = "Inprocess - AdvancePayment",
                PaymentMode = "Online",
            };
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
            
            var result = await bookingService.GetMyBookings(1);
            Assert.AreEqual(0,result.Count());
        }

        [Test]
        public async Task CalculateRefundPassTest()
        {
            var result = await bookingService.CalculateRefundAmount(Convert.ToDateTime("2024-05-31"), 2500);
            Assert.AreEqual(1250, result);
        }

    }
}
