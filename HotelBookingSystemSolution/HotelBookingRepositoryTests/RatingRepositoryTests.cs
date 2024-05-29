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

namespace HotelBookingRepositoryTests
{
    public class RatingRepositoryTests
    {
        IRepository<int, Rating> repository;
        HotelBookingContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            repository = new RatingRepository(context);
            Rating r = new Rating() { RatingId = 1, ReviewRating = 4, Comments = "Excellent" };
            await repository.Add(r);
        }


        [Test]
        public async Task AddRatingSuccessTest()
        {
            //Arrange 
            Rating r = new Rating() { RatingId=1,ReviewRating=4,Comments="Excellent" };
            //Action
            var result = await repository.Add(r);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteRatingSuccessTest()
        {
            //Action
            var result = await repository.Delete(1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteRatingFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Rating Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteRatingExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Rating Not available!", exception.Message);
        }

        [Test]
        public async Task GetRatinglSuccessTest()
        {
            //Action
            var result = await repository.Get(1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetRatingFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Rating Not available!", exception.Message);
        }

        [Test]
        public async Task GetRatingExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Rating Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllRatingSuccessTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllRatingFailTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateRatingSuccessTest()
        {
            Rating r = await repository.Get(1);
            r.ReviewRating = 4.3;
            var result = repository.Update(r).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateRatingExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Rating Not available!", exception.Message);
        }
    }
}
