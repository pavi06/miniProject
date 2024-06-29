using HotelBookingSystemAPI.Contexts;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;
using HotelBookingSystemAPI.Repositories;
using HotelBookingSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using HotelBookingSystemAPI.Models.DTOs.EmployeeDTOs;

namespace HotelBookingSystemAPIServiceTests
{
    public class UserServiceTests
    {
        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        HotelBookingContext context;
        IUserService userService;
        IRepository<int, Guest> guestRepo;
        IRepository<int, User> userRepo;
        ITokenService tokenService;
        IRepository<int, HotelEmployee> empRepo;
        ILogger<UserService> logger;
        ILogger<TokenService> logger2;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                                         .UseInMemoryDatabase("dummyDB");
            context = new HotelBookingContext(optionsBuilder.Options);
            userRepo = new UserRepository(context);
            guestRepo =  new GuestRepository(context);
            empRepo =  new EmployeeRepository(context);
            Mock<IConfigurationSection> configurationJWTSection = new Mock<IConfigurationSection>();
            configurationJWTSection.Setup(x => x.Value).Returns("This is the dummy key which is too long.");
            Mock<IConfigurationSection> configTokenSection = new Mock<IConfigurationSection>();
            configTokenSection.Setup(x => x.GetSection("JWT")).Returns(configurationJWTSection.Object);
            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection("TokenKey")).Returns(configTokenSection.Object);
            logger = Mock.Of<ILogger<UserService>>();
            logger2 = Mock.Of<ILogger<TokenService>>();
            tokenService = new TokenService(mockConfig.Object, logger2);
            userService = new UserService(userRepo, guestRepo, tokenService, empRepo, logger);
            await userService.RegisterEmployee(new RegisterEmployeeDTO() { Name = "Pavithra", Email = "pavi@gmail.com", Address = "NO 3, Gndhi nagar,TN", Password = "Pavi123", PhoneNumber = "9765345656", HotelId = 1 });
            //await userService.Register(new GuestRegisterDTO() { Name = "Pavithra", Email = "pavi@gmail.com", Address = "NO 3, Gndhi nagar,TN", Password = "Pavi123", PhoneNumber = "9765345656" });


        }

        [Test]
        public async Task UserRegisterPassTest()
        {
            var res = userService.Register(new GuestRegisterDTO() {Name = "Pavithra",Email = "pavi@gmail.com", Address="NO 3, Gndhi nagar,TN",Password = "Pavi123" , PhoneNumber="9765345656"});
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task RegisterExceptionTest()
        {
            var res = Assert.ThrowsAsync<ObjectAlreadyExistsException>(() => userService.Register(new GuestRegisterDTO() { Name = "Pavithra", Email = "pavi@gmail.com", Address = "NO 3, Gndhi nagar,TN", Password = "Pavi123", PhoneNumber = "9765345656" }));
            Assert.AreEqual("User Already Exists!", res.Message);
        }


        [Test]
        public async Task UserLoginPassTest()
        {
            var res = userService.Login(new UserLoginDTO () {Email ="pavi@gmail.com", Password="Pavi123" });
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task UserLoginActivationPassTest()
        {
            await userService.Register(new GuestRegisterDTO() { Name = "Pavithra", Email = "pavi@gmail.com", Address = "NO 3, Gndhi nagar,TN", Password = "Pavi123" , PhoneNumber="9875436567"});
            var res = Assert.ThrowsAsync<UserNotActiveException>(()=>userService.Login(new UserLoginDTO() { Email = "pavi@gmail.com", Password = "Pavi123" }));
            Assert.AreEqual("Your account is not activated", res.Message);
        }

        [Test]
        public async Task UserLoginFailTest()
        {
            IUserService userService = new UserService(userRepo, guestRepo, tokenService, empRepo, logger);
            await userService.Register(new GuestRegisterDTO() { Name = "Pavithra", Email = "pavi@gmail.com", Address = "NO 3, Gndhi nagar,TN", Password = "Pavi123", PhoneNumber="9876565647" });
            var res = Assert.ThrowsAsync<UnauthorizedUserException>(() => userService.Login(new UserLoginDTO() { Email = "pavi@gmail.com", Password = "pavi123" }));
            Assert.That("Invalid username or password", Is.EqualTo(res.Message));
        }

        [Test]
        public async Task UserLoginExceptionTest()
        {
            var res = Assert.ThrowsAsync<ObjectNotAvailableException>(() => userService.Login(new UserLoginDTO() { Email = "sai@gmail.com", Password = "sai123" }));
            Assert.AreEqual("User Not available!",res.Message);
        }

        [Test]
        public async Task UserActivatePassTest()
        {
            var res = userService.GetUserForActivation(new UserStatusDTO() { GuestId=1, Status="Active"});
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task UserActivatePassTest2()
        {
            var res = Assert.ThrowsAsync<ObjectNotAvailableException>(()=>userService.GetUserForActivation(new UserStatusDTO() { GuestId = 3, Status = "Active" }));
            Assert.AreEqual("User Not available!",res.Message);
        }

        [Test]
        public async Task UserActivatePassTest3()
        {
            IUserService userService = new UserService(userRepo, guestRepo, tokenService, empRepo, logger);
            await userService.Register(new GuestRegisterDTO() { Name = "Roke", Email = "roke@gmail.com", Address = "NO 3, Gndhi nagar,TN", Password = "Roke123", PhoneNumber = "9876565647" });
            await userService.GetUserForActivation(new UserStatusDTO() { GuestId = 1, Status = "Active" });
            var res = Assert.ThrowsAsync<Exception>(() => userService.GetUserForActivation(new UserStatusDTO() { GuestId = 1, Status = "Active" }));
            Assert.AreEqual("User Already Activated!", res.Message);
        }

        [Test]
        public async Task RegisterEmployeePassTest()
        {
            var res = userService.RegisterEmployee(new RegisterEmployeeDTO() { Name = "Pavithra", Email = "pavi@gmail.com", Address = "NO 3, Gndhi nagar,TN", Password = "Pavi123", PhoneNumber = "9765345656" , HotelId=1});
            Assert.IsNotNull(res);
        }
        [Test]
        public async Task RegisterEmployeeExceptionTest()
        {
            var res = Assert.ThrowsAsync<ObjectAlreadyExistsException>(()=>userService.RegisterEmployee(new RegisterEmployeeDTO() { Name = "Pavithra", Email = "pavi@gmail.com", Address = "NO 3, Gndhi nagar,TN", Password = "Pavi123", PhoneNumber = "9765345656", HotelId = 1 }));
            Assert.AreEqual("User Already Exists!",res.Message);
        }

        [Test]
        public async Task LoginEmployeePassTest()
        {
            var res = userService.EmployeeLogin(new UserLoginDTO() {  Email = "pavi@gmail.com", Password = "Pavi123"});
            Assert.IsNotNull(res);
        }
    }
}
