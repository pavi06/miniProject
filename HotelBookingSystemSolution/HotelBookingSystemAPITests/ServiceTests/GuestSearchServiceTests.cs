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

namespace HotelBookingSystemAPIServiceTests
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
        Hotel hotel;

        [SetUp]
        public async Task Setup()
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
            hotel = new Hotel()
            {
                HotelId=1,
                Name = "ABC Residency",
                Address = "No 3, Nehru street, ooty",
                City = "Ooty",
                TotalNoOfRooms = 5,
                IsAvailable = true,
                Rating = 4.0,
                Amenities = "Wifi, Parking",
                Restrictions = "No Pets"
            };
            await hotelRepository.Add(hotel);
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
            var exception = Assert.ThrowsAsync<ObjectsNotAvailableException>(() => guestSearchService.GetHotelsByLocationAndDate(new SearchHotelDTO() { Location = "Yelagiri", Date = DateTime.Now.Date }));
            Assert.AreEqual("No hotels are available!", exception.Message);
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
            var result = guestSearchService.GetHotelsByFeatures(new List<string>() { "Gym" }, new SearchHotelDTO() { Location = "Ooty", Date = DateTime.Now.Date });
            Assert.IsEmpty(result.Result);
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
            var result = Assert.ThrowsAsync<ObjectNotAvailableException>(() => guestSearchService.GetDetailedDescriptionOfRoomType(2, "Deluxe")); 
            Assert.AreEqual("RoomType Not available!", result.Message);
        }

        [Test]
        public async Task GetHotelRecommendationPassTest()
        {
            var res = guestSearchService.HotelRecommendations(1);
            Assert.IsEmpty(res.Result);
        }

        [Test]
        public async Task GetHotelRecommendationPassTest2()
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
            await roomTypeRepository.Add(roomType);
            var res = guestSearchService.HotelRecommendations(1);
            Assert.IsEmpty(res.Result);
        }

        [Test]
        public async Task GetHotelRecommendationPassTest3()
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
                Discount = 5,
                HotelId = 1
            };
            await roomTypeRepository.Add(roomType);
            var res = guestSearchService.HotelRecommendations(1);
            Assert.IsNotEmpty(res.Result);
        }

        [Test]
        public async Task GetHotelRecommendationPassTest4()
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
                Discount = 5,
                HotelId = 1
            };
            await roomTypeRepository.Add(roomType);
            BookedRooms br = new BookedRooms()
            {
                RoomId = 1,
                BookingId = 1,
                CheckInDate = Convert.ToDateTime("2024-05-31"),
                CheckOutDate = Convert.ToDateTime("2024-06-02")
            };
            IRepositoryForCompositeKey<int, int, BookedRooms> bookedRoomRepo = new BookedRoomsRepository(context);
            await bookedRoomRepo.Add(br);
            Room room = new Room() { RoomId = 1, TypeId = 1, HotelId = 1, Images = "dfkhkgjjghff", IsAvailable = true, RoomType = roomType, roomsBooked = new List<BookedRooms>() { br }, Hotel = hotel };
            await roomRepository.Add(room);
            Booking booking = new Booking()
            {
                BookId = 1,
                GuestId = 1,
                NoOfRooms = 2,
                TotalAmount = 3500,
                AdvancePayment = 1750,
                Discount = 0,
                BookingStatus = "Confirmed",
                PaymentId = 1,
                HotelId = 1,
                RoomsBooked=new List<BookedRooms>()
                {
                    br
                }
            };
            await bookingRepository.Add(booking);
            var res = guestSearchService.HotelRecommendations(1);
            Assert.IsNotEmpty(res.Result);
        }


        [Test]
        public async Task GetAvailableRoomTypeByHotelPassTest()
        {
            IRepository<int, RoomType> roomTypeRepo = new RoomTypeRepository(context);
            await roomTypeRepo.Add(new RoomType() { RoomTypeId = 1 ,Type = "Standard", Amenities = "Wifi, Tv", CotsAvailable = 2, Images = "ghjfdgfh", Occupancy = 2, HotelId=1 });
            var result = guestSearchService.GetAvailableRoomTypesByHotel(new SearchRoomsDTO() { HotelId = 1, CheckInDate = DateTime.Now.Date, CheckoutDate = DateTime.Now.AddDays(2) });
            Assert.IsNotEmpty(result.Result);
        }


        [Test]
        public async Task GetAvailableRoomTypeByHotelEmptyPassTest()
        {
            SearchRoomsDTO searchRoomsDTO = new SearchRoomsDTO() { HotelId = 1, CheckInDate = DateTime.Now.Date, CheckoutDate = DateTime.Now.AddDays(2) };
            var result = guestSearchService.GetAvailableRoomTypesByHotel(searchRoomsDTO);
            Assert.IsEmpty(result.Result);
        }

        [Test]
        public async Task GetAvailableRoomTypeByHotelFailTest()
        {
            var result = Assert.ThrowsAsync<AggregateException>(() => guestSearchService.GetAvailableRoomTypesByHotel(new SearchRoomsDTO() { HotelId = 8, CheckInDate = DateTime.Now.Date, CheckoutDate = DateTime.Now.AddDays(2) }));
            Assert.AreEqual("One or more errors occurred. (Hotel Not available!)", result.Message);
        }

    }
}
