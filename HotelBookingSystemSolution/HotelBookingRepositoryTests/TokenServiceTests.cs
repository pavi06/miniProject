using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Net;
using System.Xml.Linq;

namespace HotelBookingRepositoryTests
{
    public class TokenServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateTokenPassTest()
        {
            //Arrange
            Mock<IConfigurationSection> configurationJWTSection = new Mock<IConfigurationSection>();
            configurationJWTSection.Setup(x => x.Value).Returns("This is the dummy key which is too long.");
            Mock<IConfigurationSection> configTokenSection = new Mock<IConfigurationSection>();
            configTokenSection.Setup(x => x.GetSection("JWT")).Returns(configurationJWTSection.Object);
            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection("TokenKey")).Returns(configTokenSection.Object);
            ITokenService service = new TokenService(mockConfig.Object);

            //Action
            var token = service.GenerateToken(new Guest() { 
                Name = "Pavi",
                Email = "pavi@gmail.com",
                PhoneNumber = "9876545674",
                Address = "No 3, Gandhi nagar, chennai, Tamil nadu"
                });

            //Assert
            Assert.IsNotNull(token);
        }
    }
}