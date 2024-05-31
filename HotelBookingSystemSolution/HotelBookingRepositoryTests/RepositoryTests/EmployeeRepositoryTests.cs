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
using System.Xml.Linq;

namespace HotelBookingSystemAPITests.RepositoryTests
{
    public class EmployeeRepositoryTests
    {
        HotelBookingContext context;
        IRepository<int, HotelEmployee> repository;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                        .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            repository = new EmployeeRepository(context);
            HotelEmployee employee = new HotelEmployee()
            {
                EmpId = 1,
                HotelId = 2,
                Name = "Pavi",
                Email = "pavi@gmail.com",
                PhoneNumber = "9786756455",
                Address = "No 4, Gandhi street, Chennai",
                Password = new byte[] { },
                PasswordHashKey = new byte[] { },
                Status = "Active",
            };
            await repository.Add(employee);

        }


        [Test]
        public async Task AddEmployeeSuccessTest()
        {
            IRepository<int, HotelEmployee> repository = new EmployeeRepository(context);
            HotelEmployee employee = new HotelEmployee()
            {
                EmpId = 1,
                HotelId = 2,
                Name = "Pavi",
                Email = "pavi@gmail.com",
                PhoneNumber = "9786756455",
                Address = "No 4, Gandhi street, Chennai",
                Password = new byte[] { },
                PasswordHashKey = new byte[] { },
                Status = "Active",
            };
            var result = await repository.Add(employee);
            Assert.IsNotNull(result);
        }


        [Test]
        public async Task DeleteEmployeeSuccessTest()
        {
            //Action
            var result = await repository.Delete(1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteEmployeeFailTest()
        {
            IRepository<int, HotelEmployee> repository = new EmployeeRepository(context);
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);
        }

        [Test]
        public async Task DeleteEmployeeExceptionTest()
        {
            IRepository<int, HotelEmployee> repository = new EmployeeRepository(context);
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Delete(2));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);
        }

        [Test]
        public async Task GetEmployeeSuccessTest()
        {
            //Action
            var result = await repository.Get(1);
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetEmployeeFailTest()
        {
            IRepository<int, HotelEmployee> repository = new EmployeeRepository(context);
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(2));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);
        }

        [Test]
        public async Task GetEmployeeExceptionTest()
        {
            //Action
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);

        }

        [Test]
        public async Task GetAllEmployeeSuccessTest()
        {
            //Action
            var result = await repository.Get();
            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetAllEmployeeFailTest()
        {
            IRepository<int, HotelEmployee> repository = new EmployeeRepository(context);
            //Action
            var result = repository.Get().Result;
            //Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UpdateEmployeeSuccessTest()
        {
            var retrievedEmp = await repository.Get(1);
            retrievedEmp.HotelId = 2;
            var result = repository.Update(retrievedEmp).Result;
            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateEmployeeExceptionTest()
        {
            var exception = Assert.ThrowsAsync<ObjectNotAvailableException>(() => repository.Get(3));
            //Assert
            Assert.AreEqual("User Not available!", exception.Message);
        }
    }
}
