using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.RatingDTOs;
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
    public class GuestRatingServiceTests
    {
        HotelBookingContext context;
        IRepository<int, Rating> ratingRepository;
        IRepository<int, Hotel> hotelRepository;
        IGuestRatingService ratingService;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                         .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            ratingRepository = new RatingRepository(context);
            hotelRepository =  new HotelRepository(context);
            ratingService = new GuestRatingService(ratingRepository,hotelRepository);
        }

        [Test]
        public async Task ProvideRatingPassTest()
        {
            var res = ratingService.ProvideRating(new AddRatingDTO() {HotelId=1, ReviewRating=4.5, Comments="Good services" }, 1);
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task ProvideRatingExceptionTest()
        {
            var res = Assert.ThrowsAsync<ObjectNotAvailableException>(() => ratingService.ProvideRating(new AddRatingDTO() { HotelId = 8, ReviewRating = 4.5, Comments = "Good services" }, 1));
            Assert.AreEqual("Hotel Not available!",res.Message);
        }

        [Test]
        public async Task DeleteRatingPassTest()
        {
            var res = ratingService.DeleteRatingProvided(1);
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task DeleteRatingExceptionTest()
        {
            var res = Assert.ThrowsAsync<ObjectNotAvailableException>(() => ratingService.DeleteRatingProvided(2));
            Assert.AreEqual("Rating Not available",res.Message);
        }

        //[Test]
        //public async Task UpdateOverAllRatingPassTest()
        //{
        //    ratingService.UpdateOverAllRating(new Rating() {HotelId=1, ReviewRating=4,Comments="Excellent" }, true);

        //}
    }
}
