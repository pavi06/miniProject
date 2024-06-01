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
    public class RoomRepositoryTests
    {
        HotelBookingContext context;
        IRepository<int, Room> repository;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            repository = new RoomRepository(context);
            RoomType type = new RoomType("standard", 4, "hjgfhhg", 2500, 2, "Wif,TV", 3, 1);
            Room room = new Room() { RoomId = 5, TypeId = 1, HotelId = 1, Images = "dfkhkgjjghff", IsAvailable = true, RoomType = type, roomsBooked = new List<BookedRooms>(), Hotel = new Hotel("abc", "No 3, Gandhi street, Chennai", "Chennai", 2, 1.5, "Wifi, Tv", "No pets", true) };
            await repository.Add(room);
            type = new RoomType("deluxe", 4, "hjgfhhg", 2500, 2, "Wif,TV", 3, 1);
            room = new Room() { RoomId = 10, TypeId = 2, HotelId = 1, Images = "dfkhkgjjghff", IsAvailable = true, RoomType = type, roomsBooked = new List<BookedRooms>(), Hotel = new Hotel("abc", "No 3, Gandhi street, Chennai", "Chennai", 2, 1.5, "Wifi, Tv", "No pets", true) };
            await repository.Add(room);

        }


        [Test]
        public async Task AddRoomSuccessTest()
        {
            IRepository<int, Room> repository = new RoomRepository(context);
            Room room = new Room() { RoomId = 1, TypeId = 1, HotelId = 1, Images = "dfgdgdf", IsAvailable = true };
            var result = await repository.Add(room);
            Assert.That(result.RoomId, Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteRoomSuccessTest()
        {
            //Action
            var result = await repository.Delete(5);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteRoomFailTest()
        {
            IRepository<int, Room> repository = new RoomRepository(context);
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteRoomExceptionTest()
        {
            IRepository<int, Room> repository = new RoomRepository(context);
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }

        [Test]
        public async Task GetRoomSuccessTest()
        {
            //Action
            var result = await repository.Get(5);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetRoomFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(1));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }

        [Test]
        public async Task GetRoomExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllRoomSuccessTest()
        {
            Room room = new Room() { RoomId = 6, TypeId = 1, HotelId = 1, Images = "dfkhkgjjghff" };
            await repository.Add(room);
            room = new Room() { RoomId = 7, TypeId = 1, HotelId = 1, Images = "dfkhkgjjghff" };
            await repository.Add(room);
            //Action
            var result = await repository.Get();
            //Assert
            Assert.AreEqual(4, result.Count());
        }

        [Test]
        public async Task GetAllRoomFailTest()
        {
            IRepository<int, Room> repository = new RoomRepository(context);
            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateRoomSuccessTest()
        {
            BookedRooms bookedRoom = new BookedRooms()
            {
                BookingId = 1,
                RoomId = 8,
                CheckInDate = new DateTime(),
                CheckOutDate = new DateTime()
            };
            IRepositoryForCompositeKey<int, int, BookedRooms> br = new BookedRoomsRepository(context);
            await br.Add(bookedRoom);
            RoomType type = new RoomType(){ RoomTypeId =4, Type="standardDeluxe",CotsAvailable= 4, Images="hjgfhhg",Amount= 2500,Occupancy= 2, Amenities="Wif,TV", Discount=3 , HotelId=3};
            IRepository<int, RoomType> roomTypeRepo = new RoomTypeRepository(context);
            await roomTypeRepo.Add(type);
            Hotel hotel = new Hotel()
            {
                HotelId=3,
               Name= "abc", 
                Address= "No 3, Gandhi street, Chennai",
                City="Chennai",
                TotalNoOfRooms= 2,
                Rating=1.5,
                Amenities="Wifi, Tv", 
                Restrictions="No pets",
                IsAvailable= true
            };
            Room room = new Room() { RoomId = 8, TypeId = 4, HotelId = 3, Images = "dfkhkgjjghff", IsAvailable = true, RoomType = type, roomsBooked = new List<BookedRooms>() {bookedRoom }, Hotel = hotel };
            var res = await repository.Add(room);
            //Action
            var result = repository.Update(res).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateRoomExceptionTest()
        {
            IRepository<int, Room> repository = new RoomRepository(context);
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Room Not available!", exception.Message);
        }
    }
}
