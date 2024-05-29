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

namespace HotelBookingRepositoryTests
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
            BookedRooms br = new BookedRooms()
            {
                RoomId = 1,
                BookingId = 1,
                CheckInDate = new DateTime(),
                CheckOutDate = new DateTime()
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
                CheckInDate = new DateTime(),
                CheckOutDate = new DateTime()
            };
            var res = await repository.Add(br);
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task DeleteRoomSuccessTest()
        {
            //Action
            var result = await repository.Delete(1,1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteRoomFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2,1));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteRoomExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2,3));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }

        [Test]
        public async Task GetRoomSuccessTest()
        {
            var result = await repository.Get(1,1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetRoomFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(1,3));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }

        [Test]
        public async Task GetRoomExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3,4));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);

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
            BookedRooms br = new BookedRooms()
            {
                RoomId = 2,
                BookingId = 1,
                CheckInDate = new DateTime(),
                CheckOutDate = new DateTime()
            };
            var retrievedRoom = await repository.Add(br);
            retrievedRoom.BookingId = 2;
            var result = await repository.Update(retrievedRoom);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateRoomExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3,1));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }
    }
}
