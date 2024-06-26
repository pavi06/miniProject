using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;
using HotelBookingSystemAPI.Repositories;
using HotelBookingSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HotelBookingSystemAPIServiceTests
{
    public class AdminHotelServiceTests
    {
        HotelBookingContext context;
        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                         .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            IRepository<int, Hotel> hotelRepository = new HotelRepository(context);
            IAdminHotelService hotelService = new AdminHotelService(hotelRepository);

            HotelRegisterDTO hotel = new HotelRegisterDTO() { Name = "ABC Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 5, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
            hotelService.RegisterHotel(hotel);
        }

        [Test]
        public async Task RegisterHotelPassTest()
        {
            IRepository<int, Hotel> hotelRepository = new HotelRepository(context);
            IAdminHotelService hotelService = new AdminHotelService(hotelRepository);

            //Action
            HotelRegisterDTO hotel = new HotelRegisterDTO() { Name = "ABC Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 5, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
            var result = hotelService.RegisterHotel(hotel);

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task RegisterHotelFailTest()
        {
            IRepository<int, Hotel> hotelRepository = new HotelRepository(context);
            IAdminHotelService hotelService = new AdminHotelService(hotelRepository);

            //Action
            HotelRegisterDTO hotel = new HotelRegisterDTO() { Name = "ABC Residency", Address = "No 3, Nehru street, chennai", City = "Chennai", TotalNoOfRooms = 5, Amenities = "Wifi, Parking", Restrictions = "No Pets" };
            var exception = Assert.ThrowsAsync<ObjectAlreadyExistsException>(() => hotelService.RegisterHotel(hotel));
            //Assert
            Assert.AreEqual("Hotel Already Exists!", exception.Message);

        }

        [Test]
        public async Task RemoveHotelPassTest()
        {
            IRepository<int, Hotel> hotelRepository = new HotelRepository(context);
            IAdminHotelService hotelService = new AdminHotelService(hotelRepository);

            //Action
            var result = hotelService.RemoveHotel(1);

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task RemoveHotelFailTest()
        {
            IRepository<int, Hotel> hotelRepository = new HotelRepository(context);
            IAdminHotelService hotelService = new AdminHotelService(hotelRepository);

            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => hotelService.RemoveHotel(2));
            //Assert
            Assert.AreEqual("Hotel Not available!", exception.Message);
        }

        //[Test]
        //public async Task UpdateHotelPassTest()
        //{
        //    IRepository<int, Hotel> hotelRepository = new HotelRepository(context);
        //    IAdminHotelService hotelService = new AdminHotelService(hotelRepository);

        //    //Action
        //    UpdateHotelDTO update = new UpdateHotelDTO() { HotelId = 1, AttributeName = "TotalnoofRooms", AttributeValue = "3" };
        //    var result = hotelService.UpdateHotelAttribute(update);

        //    //Assert
        //    Assert.That(result, Is.Not.Null);
        //}

        //[Test]
        //public async Task UpdateHotelFailTest()
        //{
        //    IRepository<int, Hotel> hotelRepository = new HotelRepository(context);
        //    IAdminHotelService hotelService = new AdminHotelService(hotelRepository);

        //    //Action
        //    UpdateHotelDTO update = new UpdateHotelDTO() { HotelId = 1, AttributeName = "Rating", AttributeValue = "3" };
        //    var exception = Assert.ThrowsAsync<Exception>(() => hotelService.UpdateHotelAttribute(update));
        //    //Assert
        //    Assert.AreEqual("No such attribute available!", exception.Message);
        //}

        //[Test]
        //public async Task UpdateHotelExceptionTest()
        //{
        //    IRepository<int, Hotel> hotelRepository = new HotelRepository(context);
        //    IAdminHotelService hotelService = new AdminHotelService(hotelRepository);

        //    //Action
        //    UpdateHotelDTO update = new UpdateHotelDTO() { HotelId = 2, AttributeName = "Rating", AttributeValue = "3" };
        //    var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => hotelService.UpdateHotelAttribute(update));
        //    //Assert
        //    Assert.AreEqual("Hotel Not available!", exception.Message);
        //}

        [Test]
        public async Task UpdateHotelAvailabilityPassTest()
        {
            IRepository<int, Hotel> hotelRepository = new HotelRepository(context);
            IAdminHotelService hotelService = new AdminHotelService(hotelRepository);

            //Action
            UpdateHotelStatusDTO update = new UpdateHotelStatusDTO() { HotelId = 1, Status=true };
            var result = await hotelService.UpdateHotelAvailabilityService(update);

            //Assert
            Assert.AreEqual( "Status updated successfully", result);
        }

        [Test]
        public async Task UpdateHotelAvailabilityFailTest()
        {
            IRepository<int, Hotel> hotelRepository = new HotelRepository(context);
            IAdminHotelService hotelService = new AdminHotelService(hotelRepository);

            //Action
            UpdateHotelStatusDTO update = new UpdateHotelStatusDTO() { HotelId = 2, Status = true };
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => hotelService.UpdateHotelAvailabilityService(update));
            //Assert
            Assert.AreEqual("Hotel Not available!", exception.Message);
        }

        [Test]
        public async Task GetHotelPassTest()
        {
            IRepository<int, Hotel> hotelRepository = new HotelRepository(context);
            IAdminHotelService hotelService = new AdminHotelService(hotelRepository);

            //Action
           var result = hotelService.GetHotelById(1);

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetHotelFailTest()
        {
            IRepository<int, Hotel> hotelRepository = new HotelRepository(context);
            IAdminHotelService hotelService = new AdminHotelService(hotelRepository);

            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => hotelService.GetHotelById(2));
            //Assert
            Assert.AreEqual("Hotel Not available!", exception.Message);
        }

        [Test]
        public async Task GetAllHotelPassTest()
        {
            IRepository<int, Hotel> hotelRepository = new HotelRepository(context);
            IAdminHotelService hotelService = new AdminHotelService(hotelRepository);

            //Action
            var result = hotelService.GetAllHotels();

            //Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetAllHotelFailTest()
        {
            IRepository<int, Hotel> hotelRepository = new HotelRepository(context);
            IAdminHotelService hotelService = new AdminHotelService(hotelRepository);

            //Action
            var exception = Assert.ThrowsAsync<ObjectsNotAvailableException>(() => hotelService.GetAllHotels());
            //Assert
            Assert.AreEqual("No hotels are available!", exception.Message);
        }

    }
}