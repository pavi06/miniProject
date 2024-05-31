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
    public class BookedRoomsRepositoryTests
    {
        HotelBookingContext context;
        IRepositoryForCompositeKey<int, int, BookedRooms> repository;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            repository = new BookedRoomsRepository(context);
            IRepository<int, RoomType> roomTypeRepo = new RoomTypeRepository(context);
            RoomType roomType = new RoomType()
            {
                RoomTypeId = 1,
                Type = "Standard",
                Occupancy = 4,
                Images = "jhghfgh",
                Amount = 3000,
                CotsAvailable = 2,
                Amenities = "Wifi, Parking",
                Discount = 0,
                HotelId = 1
            };
            await roomTypeRepo.Add(roomType);
            IRepository<int, Room> roomRepo = new RoomRepository(context);
            Room room = new Room() { RoomId = 1, TypeId = 1, HotelId = 1, Images = "kjhjfgn" };
            await roomRepo.Add(room);
            IRepository<int, Booking> bookingRepo = new BookingRepository(context);
            Booking book = new Booking()
            {
                BookId = 1,
                GuestId = 1,
                NoOfRooms = 2,
                TotalAmount = 5000,
                AdvancePayment = 2500,
                Discount = 2,
                BookingStatus = "Confirmed",
                PaymentId = 1,
                HotelId = 1
            };
            await bookingRepo.Add(book);
            IRepository<int, Hotel> hotelRepo = new HotelRepository(context);
            Hotel hotel = new Hotel() { HotelId = 1, Name = "ABC Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 5, IsAvailable = true, Rating = 4.0, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
            await hotelRepo.Add(hotel);
            BookedRooms br = new BookedRooms()
            {
                RoomId = 1,
                BookingId = 1,
                CheckInDate = Convert.ToDateTime("2024-05-31"),
                CheckOutDate = Convert.ToDateTime("2024-06-02")
            };
            await repository.Add(br);
        }


        [Test]
        public async Task AddRoomSuccessTest()
        {
            BookedRooms br = new BookedRooms()
            {
                RoomId = 1,
                BookingId = 1,
                CheckInDate = Convert.ToDateTime("2024-05-31"),
                CheckOutDate = Convert.ToDateTime("2024-06-02")
            };
            var res = await repository.Add(br);
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task DeleteRoomSuccessTest()
        {
            //Action
            var result = await repository.Delete(1, 1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteRoomFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(1, 1));
            //Assert
            Assert.AreEqual("BookedRoom Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteRoomExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2, 3));
            //Assert
            Assert.AreEqual("BookedRoom Not available!", exception.Message);
        }

        [Test]
        public async Task GetRoomSuccessTest()
        {
            var result = await repository.Get(1, 1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetRoomFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(1, 3));
            //Assert
            Assert.AreEqual("BookedRoom Not available!", exception.Message);
        }

        [Test]
        public async Task GetRoomExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3, 4));
            //Assert
            Assert.AreEqual("BookedRoom Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllRoomSuccessTest()
        {
            //Action
            var result = await repository.Get();
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllRoomFailTest()
        {
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateRoomSuccessTest()
        {
            var val = await repository.Get(1, 1);
            val.BookingId = 3;
            var result = await repository.Update(val);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateRoomExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3, 1));
            //Assert
            Assert.AreEqual("BookedRoom Not available!", exception.Message);
        }
    }
}
