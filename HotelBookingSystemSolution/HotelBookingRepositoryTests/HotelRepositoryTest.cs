using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingRepositoryTests
{
    public class HotelRepositoryTests
    {

        IRepository<int, Hotel> hotelRepository;
        HotelBookingContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            hotelRepository = new HotelRepository(context);
            Hotel hotel = new Hotel() { Name = "ABC Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 5, IsAvailable = true, Rating = 4.0, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
            await hotelRepository.Add(hotel);

        }


        [Test]
        public async Task AddHotelSuccessTest()
        {
            //Arrange 
            Hotel hotel = new Hotel() { Name = "ABC Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 5, IsAvailable = true, Rating = 4.0, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
            //Action
            var result = await hotelRepository.Add(hotel);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task AddHotelFailTest()
        {
            //Arrange 
            Hotel hotel = new Hotel() { Name = "ABC Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 5, IsAvailable = true, Rating = 4.0, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
            //Action
            var exception = Assert.ThrowsAsync<ObjectAlreadyExistsException>(() => hotelRepository.Add(hotel));
            //Assert
            Assert.AreEqual("Hotel Already Exists!", exception.Message);
        }

        [Test]
        public async Task AddHotelExceptionTest()
        {
            //Arrange 
            Hotel hotel = new Hotel() { Name = "ABC Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 5, IsAvailable = true, Rating = 4.0, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
            //Action
            var exception = Assert.ThrowsAsync<ObjectAlreadyExistsException>(() => hotelRepository.Add(hotel));
            //Assert
            Assert.AreEqual("Hotel Already Exists!", exception.Message);

        }

        [Test]
        public async Task DeleteHotelSuccessTest()
        {
            //Action
            var result = hotelRepository.Delete(1).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteHotelFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => hotelRepository.Delete(2));
            //Assert
            Assert.AreEqual("Hotel Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteHotelExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => hotelRepository.Delete(2));
            //Assert
            Assert.AreEqual("Hotel Not available!", exception.Message);
        }

        [Test]
        public async Task GetHotelSuccessTest()
        {
            //Arrange
            Hotel hotel = new Hotel() { Name = "XYZ Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 5, IsAvailable = true, Rating = 4.0, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
            hotelRepository.Add(hotel);
            //Action
            var result = hotelRepository.Get(2).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetHotelFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => hotelRepository.Get(3));
            //Assert
            Assert.AreEqual("Hotel Not available!", exception.Message);
        }

        [Test]
        public async Task GetHotelExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => hotelRepository.Get(3));
            //Assert
            Assert.AreEqual("Hotel Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllHotelSuccessTest()
        {

            //Action
            var result = hotelRepository.Get().Result;
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllHotelFailTest()
        {

            //Action
            var result = hotelRepository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateHotelSuccessTest()
        {
            Hotel hotel = await hotelRepository.Get(1);
            hotel.Name = "XYZ Residency";
            var result = hotelRepository.Update(hotel).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateHotelExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => hotelRepository.Get(3));
            //Assert
            Assert.AreEqual("Hotel Not available!", exception.Message);
        }

    }
}