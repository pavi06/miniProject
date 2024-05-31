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

namespace HotelBookingSystemAPIBLTests
{
    public class UserServiceTests
    {
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

            Guest guest = new Guest()
            {
                GuestId = 1,
                Name = "Pavi",
                PhoneNumber = "9745356785",
                Address = "jfghnkjmk",
                Email = "pavi@gmail.com",
                Role = "User"
            };
            await guestRepo.Add(guest);

            User user = new User()
            {
                GuestId = 1,
                Password = new byte[] { },
                PasswordHashKey = new byte[] { },
                Status= "Disabled"
            };
            await userRepo.Add(user);
        }

        [Test]
        public async Task UserRegisterPassTest()
        {
            var res = userService.Register(new GuestRegisterDTO() {Name = "Pavithra",Email = "pavi@gmail.com", Address="NO 3, Gndhi nagar,TN",Password = "Pavi123" });
            Assert.IsNotNull(res);
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
            var res = Assert.ThrowsAsync<UserNotActiveException>(()=>userService.Login(new UserLoginDTO() { Email = "pavi@gmail.com", Password = "Pavi123" }));
            Assert.AreEqual("Your account is not activated", res.Message);
        }

        [Test]
        public async Task UserLoginFailTest()
        {
            var res = Assert.ThrowsAsync<UnauthorizedUserException>(() => userService.Login(new UserLoginDTO() { Email = "pavi@gmail.com", Password = "pavi123" }));
            Assert.That("Invalid username or password", Is.EqualTo(res.Message));
        }

        [Test]
        public async Task UserLoginExceptionTest()
        {
            var res = Assert.ThrowsAsync<ObjectNotAvailableException>(() => userService.Login(new UserLoginDTO() { Email = "sai@gmail.com", Password = "sai123" }));
            Assert.AreEqual("User Not available!",res.Message);
        }
    }
}
