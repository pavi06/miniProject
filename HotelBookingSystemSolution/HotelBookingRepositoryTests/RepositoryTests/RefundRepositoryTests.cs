using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystemAPITests.RepositoryTests
{
    public class RefundRepositoryTests
    {
        IRepository<int, Refund> repository;
        HotelBookingContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            repository = new RefundRepository(context);
            Refund refund = new Refund()
            {
                RefundId = 1,
                GuestId = 1,
                BookId = 1,
                RefundAmount = 1500
            };
            await repository.Add(refund);

        }


        [Test]
        public async Task AddRefundSuccessTest()
        {
            //Arrange 
            Refund refund = new Refund()
            {
                RefundId = 1,
                GuestId = 1,
                BookId = 1,
                RefundAmount = 1500
            };
            //Action
            var result = await repository.Add(refund);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteRefundSuccessTest()
        {
            //Action
            var result = repository.Delete(1).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteRefundFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Refund Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteRefundExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Refund Not available!", exception.Message);
        }

        [Test]
        public async Task GetRefundSuccessTest()
        {
            //Arrange
            Refund refund = new Refund()
            {
                RefundId = 2,
                GuestId = 1,
                BookId = 1,
                RefundAmount = 1500
            };
            await repository.Add(refund);
            //Action
            var result = repository.Get(2).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetRefundFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Refund Not available!", exception.Message);
        }

        [Test]
        public async Task GetRefundExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Refund Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllRefundSuccessTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllRefundFailTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateRefundSuccessTest()
        {
            Refund refund = await repository.Get(1);
            refund.RefundAmount = 150;
            var result = repository.Update(refund).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateRefundExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Refund Not available!", exception.Message);
        }
    }
}
