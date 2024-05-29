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
    public class RoomTypeRepositoryTests
    {
        //IRepository<int, RoomType> roomTypeRepository;
        //HotelBookingContext context;

        //[SetUp]
        //public void Setup()
        //{
        //    DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
        //                                                .UseInMemoryDatabase("dummyDB");
        //    context = new HotelBookingContext(optionsBuilder.Options);
        //    roomTypeRepository = new RoomTypeRepository(context);
        //    RoomType roomType = new RoomType("Standard", 4,3000,2,"Wifi, Parking", 0,1);
        //    roomTypeRepository.Add(roomType);   
        //}


        //[Test]
        //public void AddRoomTypeSuccessTest()
        //{
        //    RoomType roomType = new RoomType("Standard", 4, 3000, 2, "Wifi, Parking", 0, 1);
        //    var result = roomTypeRepository.Add(roomType);
        //    //Assert
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void AddRoomTypeFailTest()
        //{
        //    RoomType roomType = new RoomType("Standard", 4, 3000, 2, "Wifi, Parking", 0, 1);
        //    var result = roomTypeRepository.Add(roomType);
        //     var exception = Assert.ThrowsAsync<ObjectAlreadyExistsException>(() => roomTypeRepository.Add(roomType));
        //    //Assert
        //    Assert.AreEqual("RoomType Already Exists!", exception.Message);

        //}

        //[Test]
        //public void AddRoomTypeExceptionTest()
        //{
        //    //Arrange 
        //    RoomType roomType = new RoomType("Standard", 4, 3000, 2, "Wifi, Parking", 0, 1);
        //    var exception = Assert.Throws<ObjectAlreadyExistsException>(() => roomTypeRepository.Add(roomType));
        //    //Assert
        //    Assert.AreEqual("RoomType Already Exists!", exception.Message);

        //}

        //[Test]
        //public void DeleteRoomTypeSuccessTest()
        //{
        //    //Action
        //    var result = roomTypeRepository.Delete(1).Result;
        //    //Assert
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void DeleteRoomTypeFailTest()
        //{
        //    //Action
        //    var exception = Assert.Throws<ObjectNotAvailableException>(() => roomTypeRepository.Delete(2));
        //    //Assert
        //    Assert.AreEqual("RoomType Not Available!", exception.Message);
        //}

        //[Test]
        //public void DeleteRoomTypeExceptionTest()
        //{
        //    //Action
        //    var exception = Assert.Throws<ObjectNotAvailableException>(() => roomTypeRepository.Delete(2));
        //    //Assert
        //    Assert.AreEqual("RoomType Not Available!", exception.Message);
        //}

        //[Test]
        //public void GetRoomSuccessTest()
        //{
        //    RoomType roomType = new RoomType("Deluxe", 8, 3000, 4, "Wifi, Parking", 0, 1);
        //    roomTypeRepository.Add(roomType);
        //    //Action
        //    var result = roomTypeRepository.Get(2).Result;
        //    //Assert
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void GetRoomTypeFailTest()
        //{
        //    //Action
        //    var result = roomTypeRepository.Get(2).Result;
        //    //Assert
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public async Task GetRoomTypeExceptionTest()
        //{
        //    //Action
        //    var exception = Assert.Throws<ObjectNotAvailableException>(() => roomTypeRepository.Get(3));
        //    //Assert
        //    Assert.AreEqual("RoomType Not available!", exception.Message);

        //}

        //[Test]
        //public void GetAllRoomTypeSuccessTest()
        //{

        //    //Action
        //    var result = roomTypeRepository.Get().Result;
        //    //Assert
        //    Assert.AreEqual(1, result.Count());
        //}

        //[Test]
        //public void GetAllRoomTypeFailTest()
        //{

        //    //Action
        //    var result = roomTypeRepository.Get().Result;
        //    //Assert
        //    Assert.AreEqual(0, result.Count());
        //}

        //[Test]
        //public async Task UpdateRoomTypeSuccessTest()
        //{
        //    var room = await roomTypeRepository.Get(2);
        //    room.HotelId = 2;
        //    var result = roomTypeRepository.Update(room).Result;
        //    //Assert
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public async Task UpdateRoomTypeExceptionTest()
        //{
        //    var exception = Assert.Throws<ObjectNotAvailableException>(() => roomTypeRepository.Get(3));
        //    //Assert
        //    Assert.AreEqual("RoomType Not available!", exception.Message);
        //}
    }
}
