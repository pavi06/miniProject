using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingRepositoryTests
{
    public class Tests
    {

        IRepository<int, Hotel> hotelRepository;
        HotelBookingContext context;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            hotelRepository = new HotelRepository(context);

        }


        [Test]
        public void AddHotelSuccessTest()
        {
            //Arrange 
            Hotel hotel = new Hotel() { Name = "ABC Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 5,  IsAvailable = true, Rating = 4.0, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
            //Action
            var result = hotelRepository.Add(hotel).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void AddHotelFailTest()
        {
            //Arrange 
            Hotel hotel = new Hotel() { Name = "ABC Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 5,  IsAvailable = true, Rating = 4.0, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
            //Action
            var result = hotelRepository.Add(hotel).Result;
            //Assert
            Assert.IsNull(result);
        }

        //[Test]
        //public void AddHotelExceptionTest()
        //{
        //    //Arrange 
        //    Hotel hotel = new Hotel() { Name = "ABC Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 5, NoOfRoomsAvailable = 5, IsAvailable = true, Rating = 4.0, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
        //    //Action
        //    var exception = Assert.Throws<ObjectAlreadyExistsException>(() => hotelRepository.Add(hotel));
        //    //Assert
        //    Assert.AreEqual("Hotel Already Exists!", exception.Message);

        //}

        [Test]
        public void DeleteHotelSuccessTest()
        {
            //Action
            var result = hotelRepository.Delete(1).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void DeleteHotelFailTest()
        {
            //Action
            var result = hotelRepository.Delete(-1).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void DeleteHotelExceptionTest()
        {
            //Action
            var exception = Assert.Throws<ObjectNotAvailableException>(() => hotelRepository.Delete(3));
            //Assert
            Assert.AreEqual("Hotel Not Available!", exception.Message);
        }

        [Test]
        public void GetHotelSuccessTest()
        {
            //Arrange
            Hotel hotel = new Hotel() { Name = "XYZ Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 5,IsAvailable = true, Rating = 4.0, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
            hotelRepository.Add(hotel);
            //Action
            var result = hotelRepository.Get(2).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetHotelFailTest()
        {
            //Action
            var result = hotelRepository.Get(2).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetHotelExceptionTest()
        {
            //Action
            var exception = Assert.Throws<ObjectNotAvailableException>(() => hotelRepository.Get(3));
            //Assert
            Assert.AreEqual("Hotel Not available!", exception.Message);

        }

        [Test]
        public void GetAllHotelSuccessTest()
        {

            //Action
            var result = hotelRepository.Get().Result;
            //Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void GetAllHotelFailTest()
        {

            //Action
            var result = hotelRepository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void UpdateHotelSuccessTest()
        {
            Hotel hotel = new Hotel() { HotelId = 1, Name = "XYZ Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 9, IsAvailable = true, Rating = 4.0, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
            //Action
            var result = hotelRepository.Update(hotel).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void UpdateHotelExceptionTest()
        {
            Hotel hotel = new Hotel() { HotelId= 4, Name = "XYZ Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 9,  IsAvailable = true, Rating = 4.0, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
            //Action
            var exception = Assert.Throws<ObjectNotAvailableException>(() => hotelRepository.Update(hotel));
            //Assert
            Assert.AreEqual("Hotel Not available!", exception.Message);
        }

    }
}