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
    public class PaymentRepositoryTests
    {
        IRepository<int, Payment> repository;
        HotelBookingContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            repository = new PaymentRepository(context);
            //    Payment payment = new Payment()
            //    {
            //        AmountPaid = 2500,
            //    PaymentStatus = "Completed",
            //    PaymentMode = "Online"

            //};
            //    await repository.Add(payment);

        }


        [Test]
        public async Task AddPaymentSuccessTest()
        {
            //Arrange 
            Payment payment = new Payment()
            {
                AmountPaid = 2500,
                PaymentStatus = "Completed",
                PaymentMode = "Online"

            };
            //Action
            var result = await repository.Add(payment);
            //Assert
            Assert.IsNotNull(result);
        }



        [Test]
        public async Task DeletePaymentSuccessTest()
        {
            //Action
            var result = repository.Delete(1).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeletePaymentFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Payment Not available!", exception.Message);
        }

        [Test]
        public async Task DeletePaymentExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("Payment Not available!", exception.Message);
        }

        [Test]
        public async Task GetPaymentSuccessTest()
        {
            //Arrange
            Payment payment = new Payment()
            {
                AmountPaid = 2500,
                PaymentStatus = "Completed",
                PaymentMode = "Online"

            };
            await repository.Add(payment);
            //Action
            var result = repository.Get(2).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetPaymentFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Payment Not available!", exception.Message);
        }

        [Test]
        public async Task GetPaymentExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Payment Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllPaymentSuccessTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllPaymentFailTest()
        {

            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdatePaymentSuccessTest()
        {
            Payment p = await repository.Get(1);
            p.PaymentStatus = "In progress";
            var result = repository.Update(p).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdatePaymentExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("Payment Not available!", exception.Message);
        }
    }
}
