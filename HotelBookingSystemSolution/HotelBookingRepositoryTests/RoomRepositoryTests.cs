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
    public class RoomRepositoryTests
    {
        IRepository<int, Room> roomRepository;
        HotelBookingContext context;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            roomRepository = new RoomRepository(context);
            Room room = new Room() { TypeId = 1, HotelId = 1, Images ="dfgdgdf"};
            roomRepository.Add(room);


        }


        [Test]
        public void AddRoomSuccessTest()
        {
            Room room = new Room() { TypeId = 1, HotelId = 1, Images = "dfgdgdf" };
            var result = roomRepository.Add(room);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void AddRoomFailTest()
        {
            Room room = new Room() { TypeId = 1, HotelId = 1, Images = "dfgdgdf" };
            var result = roomRepository.Add(room);
            //Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void AddRoomExceptionTest()
        {
            //Arrange 
            Room room = new Room() { TypeId = 1, HotelId = 1, Images = "dfgdgdf" };
            var exception = Assert.Throws<ObjectAlreadyExistsException>(() => roomRepository.Add(room));
            //Assert
            Assert.AreEqual("Room Already Exists!", exception.Message);

        }

        [Test]
        public void DeleteRoomSuccessTest()
        {
            //Action
            var result = roomRepository.Delete(1).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void DeleteRoomFailTest()
        {
            //Action
            var result = roomRepository.Delete(-1).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void DeleteRoomExceptionTest()
        {
            //Action
            var exception = Assert.Throws<ObjectNotAvailableException>(() => roomRepository.Delete(2));
            //Assert
            Assert.AreEqual("Room Not Available!", exception.Message);
        }

        [Test]
        public void GetRoomSuccessTest()
        {
            Room room = new Room() { TypeId = 1, HotelId = 1, Images = "dfkhkgjjghff" };
            roomRepository.Add(room);
           //Action
            var result = roomRepository.Get(2).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetRoomFailTest()
        {
            //Action
            var result = roomRepository.Get(2).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetRoomExceptionTest()
        {
            //Action
            var exception = Assert.Throws<ObjectNotAvailableException>(() => roomRepository.Get(3));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);

        }

        [Test]
        public void GetAllRoomSuccessTest()
        {

            //Action
            var result = roomRepository.Get().Result;
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void GetAllRoomFailTest()
        {

            //Action
            var result = roomRepository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateRoomSuccessTest()
        {
            var room = await roomRepository.Get(2);
            room.HotelId = 2;
            var result = roomRepository.Update(room).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateRoomExceptionTest()
        {
            var exception = Assert.Throws<ObjectNotAvailableException>(() => roomRepository.Get(3));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }
    }
}
