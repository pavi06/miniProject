using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;
using HotelBookingSystemAPI.Repositories;
using HotelBookingSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystemAPIBLTests
{
    public class GuestSearchServiceTests
    {
        HotelBookingContext context;
        IRepository<int, Hotel> hotelRepository;
        IRepository<int, Guest> guestRepository;
        IRepository<int, Room> roomRepository;
        IRepository<int, Booking> bookingRepository;
        IRepository<int, RoomType> roomTypeRepository;
        IRepositoryForCompositeKey<int, DateTime, HotelAvailabilityByDate> hotelAvailability;
        IGuestSearchService guestSearchService;
        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                         .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            hotelRepository = new HotelRepository(context);
            guestRepository =  new GuestRepository(context);
            roomRepository = new RoomRepository(context);
            bookingRepository = new BookingRepository(context);
            roomTypeRepository = new RoomTypeRepository(context);
            hotelAvailability = new HotelsAvailabilityRepository(context);
            guestSearchService = new GuestSearchService(hotelRepository, roomRepository, hotelAvailability, roomTypeRepository,guestRepository,bookingRepository);

        }

        [Test]
        public async Task GetHotelsByLocationPassTests()
        {
            var result = guestSearchService.GetHotelsByLocationAndDate(new SearchHotelDTO() { Location = "Ooty", Date = DateTime.Now.Date });
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetHotelsByLocationFailTests()
        {
            var exception = Assert.ThrowsAsync<ObjectsNotAvailableException>(() => guestSearchService.GetHotelsByLocationAndDate(new SearchHotelDTO() { Location = "Ooty", Date = DateTime.Now.Date }));
            Assert.AreEqual("No hotel are available!", exception.Message);
        }

        [Test]
        public async Task GetHotelsByRatingPassTest()
        {
            var result = guestSearchService.GetHotelsByRatings(new SearchHotelDTO() { Location = "Ooty", Date = DateTime.Now.Date });
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetHotelsByRatingFailTest()
        {
            var result = Assert.ThrowsAsync<ObjectsNotAvailableException>(() =>  guestSearchService.GetHotelsByRatings(new SearchHotelDTO() { Location = "Chennai", Date = DateTime.Now.Date }));
            Assert.AreEqual("No hotels are available!",result.Message);
        }

        [Test]
        public async Task GetHotelsByFeaturesPassTest()
        {
            var result = guestSearchService.GetHotelsByFeatures(new List<string>() { "Wifi"},new SearchHotelDTO() { Location = "Ooty", Date = DateTime.Now.Date });
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetHotelsByFeaturesFailTest()
        {
            var result = Assert.ThrowsAsync<ObjectsNotAvailableException>(() => guestSearchService.GetHotelsByFeatures(new List<string>() { "" }, new SearchHotelDTO() { Location = "Ooty", Date = DateTime.Now.Date }));
            Assert.AreEqual("No hotels are available!", result.Message);
        }

        [Test]  
        public async Task GetDetailsOfRoomTypePassTest()
        {
            var res = guestSearchService.GetDetailedDescriptionOfRoomType(1, "standard");
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task GetDetailsOfRoomTypeFailTest()
        {
            var result = Assert.ThrowsAsync<ObjectNotAvailableException>(() => guestSearchService.GetDetailedDescriptionOfRoomType(1, "Deluxe")); 
            Assert.AreEqual("RoomType Not available!", result.Message);
        }

        [Test]
        public async Task GetHotelRecommendationPassTest()
        {
            var res = guestSearchService.HotelRecommendations(1);
            Assert.IsNotNull(res);
        }


        [Test]
        public async Task GetAvailableRoomTypeByHotelPassTest()
        {
            var result = guestSearchService.GetAvailableRoomTypesByHotel(new SearchRoomsDTO() { HotelId=1,CheckInDate=DateTime.Now.Date, CheckoutDate=DateTime.Now.AddDays(2)});
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAvailableRoomTypeByHotelFailTest()
        {
            var result = Assert.ThrowsAsync<ObjectNotAvailableException>(() => guestSearchService.GetAvailableRoomTypesByHotel(new SearchRoomsDTO() { HotelId = 8, CheckInDate = DateTime.Now.Date, CheckoutDate = DateTime.Now.AddDays(2) }));
            Assert.AreEqual("Hotel Not available!", result.Message);
        }

    }
}
