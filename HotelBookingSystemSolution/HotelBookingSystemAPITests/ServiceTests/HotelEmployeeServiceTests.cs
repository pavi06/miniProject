using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repositories;
using HotelBookingSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HotelBookingSystemAPIServiceTests
{
    public class HotelEmployeeServiceTests
    {
        HotelBookingContext context;
        ILogger<HotelEmployeeService> logger;
        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                         .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            logger = Mock.Of<ILogger<HotelEmployeeService>>();
            Hotel hotel = new Hotel()
            {
                HotelId = 1,
                Name = "abc hotel",
                Address = "ukfdgcvj",
                City = "Chennai",
                TotalNoOfRooms = 2,
                Rating = 3,
                Amenities = "Wifi, Tv",
                Restrictions = "No pet",
                IsAvailable = true,
                bookingsForHotel = new List<Booking>()
            };
            IRepository<int, Hotel> hotelRepo = new HotelRepository(context);
            await hotelRepo.Add(hotel);

            EmployeeRepository employeeRepo = new EmployeeRepository(context);
            HotelEmployee emp = new HotelEmployee()
            {
                HotelId = 1,
                Name = "sai",
                Email = "sai@gmail.com",
                PhoneNumber = "9776456756",
                Address = "ifhgjhkjk",
                Password = new byte[] { },
                PasswordHashKey = new byte[] { },
                Status = "Active"
            };
            await employeeRepo.Add(emp);

        }

        [Test]
        public async Task GetAllBookingDoneTodayPassTest()
        {
            IRepository<int, Guest> guestRepository = new GuestBookingsRepository(context);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context);
            IHotelEmployeeService empService = new HotelEmployeeService(guestRepository, roomRepository, bookingRepository, logger);

            //Action
            var result = await empService.GetAllBookingRequestDoneToday(1);

            //Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAllBookingDoneTodayPassTest2()
        {
            IRepository<int, Guest> guestRepository = new GuestBookingsRepository(context);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context);
            IHotelEmployeeService empService = new HotelEmployeeService(guestRepository, roomRepository, bookingRepository, logger);
            Booking booking = new Booking()
            {
                BookId = 1,
                GuestId = 1,
                NoOfRooms = 2,
                TotalAmount = 3500,
                AdvancePayment = 1750,
                Discount = 0,
                BookingStatus = "Confirmed",
                PaymentId = 1,
                HotelId = 1,
            };
            await bookingRepository.Add(booking);
            Guest guest = new Guest()
            {
                GuestId = 1,
                Name = "Pavi",
                Email = "pavi@gmail.com",
                PhoneNumber = "9874567856",
                Address = "fghbjknknk,",
                bookings = new List<Booking>() { booking },
            };
            await guestRepository.Add(guest);
            //Action
            var result = await empService.GetAllBookingRequestDoneToday(1);

            //Assert
            Assert.AreEqual(1, result.Count());
        }


        [Test]
        public async Task GetAllCheckInTodayPassTest()
        {
            IRepository<int, Guest> guestRepository = new GuestBookingsRepository(context);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context);
            IHotelEmployeeService empService = new HotelEmployeeService(guestRepository, roomRepository, bookingRepository, logger);
            BookedRooms br = new BookedRooms()
            {
                RoomId = 1,
                BookingId = 1,
                CheckInDate = Convert.ToDateTime("2024-06-01"),
                CheckOutDate = Convert.ToDateTime("2024-06-02")
            };
            IRepositoryForCompositeKey<int, int , BookedRooms> bookedRoomRepo = new BookedRoomsRepository(context);
            await bookedRoomRepo.Add(br);
            Booking booking = new Booking()
            {
                BookId = 1,
                GuestId = 1,
                NoOfRooms = 2,
                TotalAmount = 3500,
                AdvancePayment = 1750,
                Discount = 0,
                BookingStatus = "Confirmed",
                PaymentId = 1,
                HotelId = 1,
                RoomsBooked = new List<BookedRooms>() { br}
            };
            await bookingRepository.Add(booking);
            Guest guest = new Guest()
            {
                GuestId = 1,
                Name = "Pavi",
                Email = "pavi@gmail.com",
                PhoneNumber = "9874567856",
                Address = "fghbjknknk,",
                bookings = new List<Booking>() { booking },
            };
            await guestRepository.Add(guest);
            //Action
            var result = await empService.GetAllCheckInForToday(1);

            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllCheckInTodayExceptionTest()
        {
            IRepository<int, Guest> guestRepository = new GuestBookingsRepository(context);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context);
            IHotelEmployeeService empService = new HotelEmployeeService(guestRepository, roomRepository, bookingRepository, logger);

            //Action
            var result = Assert.ThrowsAsync<ObjectsNotAvailableException>(()=>empService.GetAllCheckInForToday(1));

            //Assert
            Assert.AreEqual("No Booking are available!", result.Message);
        }

        [Test]
        public async Task GetAllCheckInTodayFailTest()
        {
            IRepository<int, Guest> guestRepository = new GuestBookingsRepository(context);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context);
            IHotelEmployeeService empService = new HotelEmployeeService(guestRepository, roomRepository, bookingRepository, logger);

            //Action
            var exception = Assert.ThrowsAsync<ObjectsNotAvailableException>(() => empService.GetAllCheckInForToday(2));
            //Assert
            Assert.AreEqual("No Booking are available!", exception.Message);
        }


        [Test]
        public async Task GetAllBookingPassTest()
        {
            IRepository<int, Guest> guestRepository = new GuestBookingsRepository(context);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context);
            IHotelEmployeeService empService = new HotelEmployeeService(guestRepository, roomRepository, bookingRepository, logger);
            Booking booking = new Booking()
            {
                BookId = 1,
                GuestId = 1,
                NoOfRooms = 2,
                TotalAmount = 3500,
                AdvancePayment = 1750,
                Discount = 0,
                BookingStatus = "Confirmed",
                PaymentId = 1,
                HotelId = 1,
            };
            await bookingRepository.Add(booking);
            Guest guest = new Guest()
            {
                GuestId = 1,
                Name = "Pavi",
                Email = "pavi@gmail.com",
                PhoneNumber = "9874567856",
                Address = "fghbjknknk,",
                bookings = new List<Booking>() { booking },
            };
            await guestRepository.Add(guest);
            //Action
            var result = await empService.GetAllBookingRequest(1);

            //Assert
            Assert.AreEqual(1, result.Count());
        }


        [Test]
        public async Task GetAllBookingPassTest2()
        {
            IRepository<int, Guest> guestRepository = new GuestBookingsRepository(context);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context);
            IHotelEmployeeService empService = new HotelEmployeeService(guestRepository, roomRepository, bookingRepository, logger);
            
            //Action
            var exception = Assert.ThrowsAsync<ObjectsNotAvailableException>(() => empService.GetAllBookingRequest(1));
            //Assert
            Assert.AreEqual("No Bookings are available!", exception.Message);
        }

        [Test]
        public async Task GetAllBookingByFilterationPassTest()
        {
            IRepository<int, Guest> guestRepository = new GuestBookingsRepository(context);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context);
            IHotelEmployeeService empService = new HotelEmployeeService(guestRepository, roomRepository, bookingRepository, logger);

            //Action
            var exception = await empService.GetAllBookingRequestByFilteration(1, "date","2024-05-31");
            //Assert
            Assert.IsEmpty(exception);
        }

        [Test]
        public async Task GetAllBookingByFilterationPassTest2()
        {
            IRepository<int, Guest> guestRepository = new GuestBookingsRepository(context);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context);
            IHotelEmployeeService empService = new HotelEmployeeService(guestRepository, roomRepository, bookingRepository, logger);

            //Action
            var exception = await empService.GetAllBookingRequestByFilteration(1, "month", "05");
            //Assert
            Assert.IsEmpty(exception);
        }
    }
}
