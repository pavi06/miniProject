using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystemAPITests.RepositoryTests
{
    public class BookingRepositoryTests
    {
        HotelBookingContext context;
        IRepository<int, Booking> repository;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            repository = new BookingRepository(context);
            //Booking booking = new Booking()
            //{
            //    BookId = 1,
            //    GuestId = 1,
            //    NoOfRooms = 2,
            //    TotalAmount = 3500,
            //    AdvancePayment = 1750,
            //    Discount = 0,
            //    BookingStatus = "Confirmed",
            //    PaymentId = 1,
            //    HotelId = 1,
            //};
            //await repository.Add(booking);

        }


        [Test]
        public async Task AddBookingSuccessTest()
        {
            IRepository<int, Booking> repository = new BookingRepository(context);
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
            var result = await repository.Add(booking);
            Assert.IsNotNull(result);
        }


        [Test]
        public async Task DeleteBookingSuccessTest()
        {
            //Action
            var result = await repository.Delete(1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteBookingFailTest()
        {
            IRepository<int, Booking> repository = new BookingRepository(context);
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Booking Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteBookingExceptionTest()
        {
            IRepository<int, Booking> repository = new BookingRepository(context);
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Booking Not available!", exception.Message);
        }

        [Test]
        public async Task GetBookingSuccessTest()
        {
            //Action
            var result = await repository.Get(1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetBookingFailTest()
        {
            IRepository<int, Booking> repository = new BookingRepository(context);
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(2));
            //Assert
            Assert.AreEqual("Booking Not available!", exception.Message);
        }

        [Test]
        public async Task GetBookingExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Booking Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllBookingSuccessTest()
        {
            //Action
            var result = await repository.Get();
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllBookingFailTest()
        {
            IRepository<int, Room> repository = new RoomRepository(context);
            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateBookingSuccessTest()
        {
            var retrievedBooking = await repository.Get(1);
            retrievedBooking.HotelId = 2;
            var result = repository.Update(retrievedBooking).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateBookingExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Booking Not available!", exception.Message);
        }
    }
}
