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
    public class GuestRepositoryTests
    {
        IRepository<int, Guest> guestRepository;
        HotelBookingContext context;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            guestRepository = new GuestRepository(context);
            Guest guest = new Guest("Pavi", "pavi@gamil.com", "9796756456", "Tamil Nadu, Chennai");
            guestRepository.Add(guest);

        }


        [Test]
        public void AddGuestSuccessTest()
        {
            Guest guest = new Guest("Pavi", "pavi@gamil.com", "9796756456", "Tamil Nadu, Chennai");
            var result = guestRepository.Add(guest);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void AddGuestFailTest()
        {
            Guest guest = new Guest("Pavi", "pavi@gamil.com", "9796756456", "Tamil Nadu, Chennai");
            var exception = Assert.Throws<ObjectAlreadyExistsException>(() => guestRepository.Add(guest));
            //Assert
            Assert.AreEqual("User Already Exists!", exception.Message);
        }

        [Test]
        public void AddGuestExceptionTest()
        {
            //Arrange 
            Guest guest = new Guest("Pavi", "pavi@gamil.com", "9796756456", "Tamil Nadu, Chennai");
            var exception = Assert.Throws<ObjectAlreadyExistsException>(() => guestRepository.Add(guest));
            //Assert
            Assert.AreEqual("User Already Exists!", exception.Message);

        }

        [Test]
        public void DeleteGuestSuccessTest()
        {
            //Action
            var result = guestRepository.Delete(1).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteGuestFailTest()
        {
            //Action  
            try
            {
                guestRepository.Delete(2);
            }
            catch (ObjectNotAvailableException exception)
            {
                Assert.AreEqual("User Not Available!", exception.Message);
            }

        }

        [Test]
        public async Task DeleteGuestExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => guestRepository.Delete(3));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);
        }

        [Test]
        public void GetGuestSuccessTest()
        {
            Guest guest = new Guest("Roke", "roke@gamil.com", "9796756456", "Tamil Nadu, Chennai");
            guestRepository.Add(guest);
            //Action
            var result = guestRepository.Get(2).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetGuestFailTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => guestRepository.Get(3));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);

        }

        [Test]
        public async Task GetGuestExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => guestRepository.Get(3));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);

        }

        [Test]
        public void GetAllGuestSuccessTest()
        {

            //Action
            var result = guestRepository.Get().Result;
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void GetAllGuestFailTest()
        {

            //Action
            var result = guestRepository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateGuetSuccessTest()
        {
            var guest = await guestRepository.Get(1);
            guest.Name = "Rokesh";
            var result = guestRepository.Update(guest).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateGuestExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => guestRepository.Get(2));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);
        }
    }
}
