using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repositories;
using HotelBookingSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using System.Runtime.InteropServices;

namespace HotelBookingSystemAPIServiceTests
{
    public class AdminRoomServiceTests
    {
        HotelBookingContext context;
        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                         .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, RoomType> roomTypeRepository = new RoomTypeRepository(context);
            IAdminRoomService roomService = new AdminRoomService(roomRepository, roomTypeRepository);

            //Action
            RoomTypeDTO roomType = new RoomTypeDTO()
            { 
                Type = "Standard",
                Occupancy = 4,
                Images = "jhghfgh",
                Amount = 3000,
                CotsAvailable = 2,
                Amenities = "Wifi, Parking",
                Discount = 0,
                HotelId = 1
            };
            await roomService.RegisterRoomTypeForHotel(roomType);

            AddRoomDTO room = new AddRoomDTO() { TypeId = 1, HotelId = 1 };
            await roomService.RegisterRoomForHotel(room);
        }

        [Test]
        public async Task RegisterRoomPassTest()
        {
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, RoomType> roomTypeRepository = new RoomTypeRepository(context);
            IAdminRoomService roomService = new AdminRoomService(roomRepository, roomTypeRepository);

            //Action
            AddRoomDTO room = new AddRoomDTO() { TypeId=1,HotelId=1,Images="jkhfhgbnnm" };
            var result = roomService.RegisterRoomForHotel(room);

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task RegisterRoomPassTest2()
        {
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, RoomType> roomTypeRepository = new RoomTypeRepository(context);
            IAdminRoomService roomService = new AdminRoomService(roomRepository, roomTypeRepository);

            //Action
            AddRoomDTO room = new AddRoomDTO() { TypeId = 1, HotelId = 1 };
            var result = roomService.RegisterRoomForHotel(room);
            Assert.That(result, Is.Not.Null);

        }

        [Test]
        public async Task RegisterRoomTypePassTest()
        {
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, RoomType> roomTypeRepository = new RoomTypeRepository(context);
            IAdminRoomService roomService = new AdminRoomService(roomRepository, roomTypeRepository);

            //Action
            RoomTypeDTO roomType = new RoomTypeDTO()
            {
                Type = "Standard",
                Occupancy = 4,
                Images = "jhghfgh",
                Amount = 3000,
                CotsAvailable = 2,
                Amenities = "Wifi, Parking",
                Discount = 0,
                HotelId = 1
            };
            var result = roomService.RegisterRoomTypeForHotel(roomType);

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task RegisterRoomTypeFailTest()
        {
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, RoomType> roomTypeRepository = new RoomTypeRepository(context);
            IAdminRoomService roomService = new AdminRoomService(roomRepository, roomTypeRepository);

            //Action
            RoomTypeDTO roomType = new RoomTypeDTO()
            {
                Type = "Standard",
                Occupancy = 4,
                Images = "jhghfgh",
                Amount = 3000,
                CotsAvailable = 2,
                Amenities = "Wifi, Parking",
                Discount = 0,
                HotelId = 1
            };
            var exception = Assert.ThrowsAsync<ObjectAlreadyExistsException>(() => roomService.RegisterRoomTypeForHotel(roomType));
            //Assert
            Assert.AreEqual("RoomType Already Exists!", exception.Message);

        }

        [Test]
        public async Task UpdateRoomImagesPassTest()
        {
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, RoomType> roomTypeRepository = new RoomTypeRepository(context);
            IAdminRoomService roomService = new AdminRoomService(roomRepository, roomTypeRepository);

            //Action
            var result = await roomService.UpdateRoomImages(1, "jghhvhbj");

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateRoomImagesFailTest()
        {
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, RoomType> roomTypeRepository = new RoomTypeRepository(context);
            IAdminRoomService roomService = new AdminRoomService(roomRepository, roomTypeRepository);

            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => roomService.UpdateRoomImages(2, "jghhvhbj"));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }

        [Test]
        public async Task UpdateRoomStatusPassTest()
        {
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, RoomType> roomTypeRepository = new RoomTypeRepository(context);
            IAdminRoomService roomService = new AdminRoomService(roomRepository, roomTypeRepository);

            //Action
            var result = await roomService.UpdateRoomStatusForHotel(1);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateRoomStatusFailTest()
        {
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, RoomType> roomTypeRepository = new RoomTypeRepository(context);
            IAdminRoomService roomService = new AdminRoomService(roomRepository, roomTypeRepository);

            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => roomService.UpdateRoomStatusForHotel(2));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }

        
        [Test]
        public async Task UpdateRoomTypePassTest()
        {
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, RoomType> roomTypeRepository = new RoomTypeRepository(context);
            IAdminRoomService roomService = new AdminRoomService(roomRepository, roomTypeRepository);
            //Action
            UpdateRoomTypeDTO update = new UpdateRoomTypeDTO() { HotelId = 1, RoomTypeId = 1, AttributeName = "Amenities", AttributeValue = "Wifi,TV" };
            var result = roomService.UpdateRoomTypeByAttribute(update);

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task UpdateRoomTypeFailTest()
        {
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, RoomType> roomTypeRepository = new RoomTypeRepository(context);
            IAdminRoomService roomService = new AdminRoomService(roomRepository, roomTypeRepository);
            Hotel hotel = new Hotel()
            {
                HotelId = 1,
                Name = "hjgjb",
                Address = "kghjk",
                Amenities = "jghkhj",
                IsAvailable=true,
                City="chennai",
                Restrictions="tfjghkjkl"

            };
            IRepository<int, Hotel> hotelRepo = new HotelRepository(context);
            await hotelRepo.Add(hotel);
            RoomType roomType = new RoomType()
            {
                Type="Standard",
                RoomTypeId=3,
                Amenities="kughjb",
                CotsAvailable=1,
                Amount=350,
                Discount=0,
                HotelId=1,
                Images="itdfghghb",
                Hotel=hotel
            };
            await roomTypeRepository.Add(roomType);
            //Action
            UpdateRoomTypeDTO update = new UpdateRoomTypeDTO() { HotelId = 1, RoomTypeId = 3, AttributeName = "bookingId", AttributeValue = "6" };
            var exception = Assert.ThrowsAsync<Exception>(() => roomService.UpdateRoomTypeByAttribute(update));
            //Assert
            Assert.AreEqual("No such attribute available!", exception.Message);
        }

        [Test]
        public async Task UpdateHotelExceptionTest()
        {
            IRepository<int, Room> roomRepository = new RoomRepository(context);
            IRepository<int, RoomType> roomTypeRepository = new RoomTypeRepository(context);
            IAdminRoomService roomService = new AdminRoomService(roomRepository, roomTypeRepository);

            //Action
            UpdateRoomTypeDTO update = new UpdateRoomTypeDTO() { HotelId = 1, RoomTypeId = 2, AttributeName = "Amenities", AttributeValue = "Wifi,TV" };
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => roomService.UpdateRoomTypeByAttribute(update));
            //Assert
            Assert.AreEqual("RoomType Not available!", exception.Message);
        }

        
    }
}
