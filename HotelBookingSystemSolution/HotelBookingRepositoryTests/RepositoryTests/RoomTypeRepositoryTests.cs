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
    public class RoomTypeRepositoryTests
    {
        IRepository<int, RoomType> roomTypeRepository;
        HotelBookingContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            roomTypeRepository = new RoomTypeRepository(context);
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
            await roomTypeRepository.Add(roomType);

            Hotel hotel = new Hotel()
            {
                HotelId=1,
                Name = "ABC Residency",
                Address = "No 3, Nehru street, chennai",
                City = "Chennai",
                TotalNoOfRooms = 5,
                IsAvailable = true,
                Rating = 4.0,
                Amenities = "Wifi, Parking",
                Restrictions = "No Pets"
            };
            IRepository<int, Hotel> hotelRepo = new HotelRepository(context);
            await hotelRepo.Add(hotel);

        }


        [Test]
        public async Task AddRoomTypeSuccessTest()
        {
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
            var result = await roomTypeRepository.Add(roomType);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task AddRoomTypeExceptionTest()
        {
            //Arrange 
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
            var exception = Assert.Throws<ObjectAlreadyExistsException>(() => roomTypeRepository.Add(roomType));
            //Assert
            Assert.AreEqual("RoomType Already Exists!", exception.Message);

        }

        [Test]
        public async Task DeleteRoomTypeSuccessTest()
        {
            //Action
            var result = await roomTypeRepository.Delete(1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteRoomTypeFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => roomTypeRepository.Delete(2));
            //Assert
            Assert.AreEqual("RoomType Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteRoomTypeExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => roomTypeRepository.Delete(2));
            //Assert
            Assert.AreEqual("RoomType Not available!", exception.Message);
        }

        [Test]
        public async Task GetRoomSuccessTest()
        {
            RoomType roomType = new RoomType()
            {
                RoomTypeId = 2,
                Type = "Deluxe",
                Occupancy = 4,
                Images = "jhghfgh",
                Amount = 3000,
                CotsAvailable = 2,
                Amenities = "Wifi, Parking",
                Discount = 0,
                HotelId = 1
            };
            await roomTypeRepository.Add(roomType);
            //Action
            var result = await roomTypeRepository.Get(2);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetRoomTypeExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => roomTypeRepository.Get(3));
            //Assert
            Assert.AreEqual("RoomType Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllRoomTypeSuccessTest()
        {

            //Action
            var result = roomTypeRepository.Get().Result;
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllRoomTypeFailTest()
        {

            //Action
            var result = roomTypeRepository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateRoomTypeSuccessTest()
        {
            RoomType roomType = new RoomType()
            {
                RoomTypeId = 5,
                Type = "Standard deluxe",
                Occupancy = 4,
                Images = "jhghfgh",
                Amount = 3000,
                CotsAvailable = 2,
                Amenities = "Wifi, Parking",
                Discount = 0,
                HotelId = 1
            };
            var room = await roomTypeRepository.Add(roomType);
            room.HotelId = 2;
            var result = await roomTypeRepository.Update(room);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateRoomTypeExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => roomTypeRepository.Get(3));
            //Assert
            Assert.AreEqual("RoomType Not available!", exception.Message);
        }
    }
}
