using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repositories;
using HotelBookingSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystemAPIBLTests
{
    public class HotelEmployeeServiceTests
    {
        HotelBookingContext context;
        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                         .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            
        }

        [Test]
        public async Task GetAllBookingDoneTodayPassTest()
        {
            IRepository<int, Guest> guestRepository = new GuestBookingsRepository(context);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context);
            IHotelEmployeeService empService = new HotelEmployeeService(guestRepository,roomRepository,bookingRepository);

            //Action
            var result = empService.GetAllBookingRequestDoneToday(1);

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetAllBookingDoneTodayFailTest()
        {
            IRepository<int, Guest> guestRepository = new GuestBookingsRepository(context);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context);
            IHotelEmployeeService empService = new HotelEmployeeService(guestRepository, roomRepository, bookingRepository);

            //Action
            var result = await empService.GetAllBookingRequestDoneToday(2);

            //Assert
            Assert.AreEqual(result.Count(),0);
        }


        [Test]
        public async Task GetAllBookingDoneTodayExceptionTest()
        {
            IRepository<int, Guest> guestRepository = new GuestBookingsRepository(context);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context);
            IHotelEmployeeService empService = new HotelEmployeeService(guestRepository, roomRepository, bookingRepository);

            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => empService.GetAllBookingRequestDoneToday(18));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);
        }

        [Test]
        public async Task GetAllCheckInTodayPassTest()
        {
            IRepository<int, Guest> guestRepository = new GuestBookingsRepository(context);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context);
            IHotelEmployeeService empService = new HotelEmployeeService(guestRepository, roomRepository, bookingRepository);

            //Action
            var result = empService.GetAllCheckInForToday(1);

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetAllCheckInTodayFailTest()
        {
            IRepository<int, Guest> guestRepository = new GuestBookingsRepository(context);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, Booking> bookingRepository = new BookingRepository(context);
            IHotelEmployeeService empService = new HotelEmployeeService(guestRepository, roomRepository, bookingRepository);

            //Action
            var exception = Assert.ThrowsAsync<ObjectsNotAvailableException>(() => empService.GetAllCheckInForToday(2));
            //Assert
            Assert.AreEqual("No Booking are available!", exception.Message);
        }
    }
}
