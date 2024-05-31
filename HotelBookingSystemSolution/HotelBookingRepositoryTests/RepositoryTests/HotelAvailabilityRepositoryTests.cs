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
    public class HotelAvailabilityRepositoryTests
    {
        HotelBookingContext context;
        IRepositoryForCompositeKey<int, DateTime, HotelAvailabilityByDate> repository;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            repository = new HotelsAvailabilityRepository(context);
            //Hotel hotel = new Hotel()
            //{
            //    HotelId = 1,
            //    Name = "ABC Residency",
            //    Address = "No 3, Nehru street, chennai",
            //    City = "Chennai",
            //    TotalNoOfRooms = 5,
            //    IsAvailable = true,
            //    Rating = 4.0,
            //    Amenities = "Wifi, Parking",
            //    Restrictions = "No Pets"
            //};
            //IRepository<int, Hotel> hotelRepo = new HotelRepository(context);
            //await hotelRepo.Add(hotel);
            //HotelAvailabilityByDate br = new HotelAvailabilityByDate()
            //{
            //    HotelId = 1,
            //    Date = Convert.ToDateTime("2024-06-02"),
            //    RoomsAvailableCount = 1,
            //};
            //await repository.Add(br);
        }


        [Test]
        public async Task AddHotelSuccessTest()
        {
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
            HotelAvailabilityByDate br = new HotelAvailabilityByDate()
            {
                HotelId = 1,
                Date = Convert.ToDateTime("2024-06-02"),
                RoomsAvailableCount = 1,
            };
            var res = await repository.Add(br);
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task DeleteHotelSuccessTest()
        {
            //Action
            var result = await repository.Delete(1, Convert.ToDateTime("2024-06-02"));
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteHotelFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(1, new DateTime()));
            //Assert
            Assert.AreEqual("HotelAvailability Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteHotelExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(1, new DateTime()));
            //Assert
            Assert.AreEqual("HotelAvailability Not available!", exception.Message);
        }

        [Test]
        public async Task GetHotelSuccessTest()
        {
            var result = await repository.Get(1, Convert.ToDateTime("2024-06-02"));
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetHotelFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(1, new DateTime()));
            //Assert
            Assert.AreEqual("HotelAvailability Not available!", exception.Message);
        }

        [Test]
        public async Task GetHotelExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(1, new DateTime()));
            //Assert
            Assert.AreEqual("HotelAvailability Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllHotelSuccessTest()
        {
            //Action
            var result = await repository.Get();
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllHotelFailTest()
        {
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateHotelSuccessTest()
        {
            HotelAvailabilityByDate br = new HotelAvailabilityByDate()
            {
                HotelId = 1,
                Date = new DateTime(),
                RoomsAvailableCount = 1,
            };
            var retrievedRoom = await repository.Add(br);
            retrievedRoom.RoomsAvailableCount = 2;
            var result = await repository.Update(retrievedRoom);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateHotelExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(1, new DateTime()));
            //Assert
            Assert.AreEqual("HotelAvailability Not available!", exception.Message);
        }
    }
}
