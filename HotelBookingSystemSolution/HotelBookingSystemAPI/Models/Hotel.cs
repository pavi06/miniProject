using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int TotalNoOfRooms { get; set; }
        public int NoOfRoomsAvailable { get; set; }
        public bool IsAvailable { get; set; } = true;
        public double Rating { get; set; } = 0;
        public string Amenities { get; set; }
        public string Restrictions { get; set; }

        public List<Room> Rooms { get; set; } //navigation property
        public List<Rating> Ratings { get; set; } //navigation property
        public List<RoomType> RoomTypes { get; set; }

        public Hotel() { }

        public Hotel(string name, string address, string city, int totalNoOfRooms, int noOfRoomsAvailable, bool isAvailable, double rating, string amenities, string restrictions)
        {
            Name = name;
            Address = address;
            City = city;
            TotalNoOfRooms = totalNoOfRooms;
            NoOfRoomsAvailable = noOfRoomsAvailable;
            IsAvailable = isAvailable;
            Rating = rating;
            Amenities = amenities;
            Restrictions = restrictions;
        }

        public Hotel(string name, string address, string city, int totalNoOfRooms, int noOfRoomsAvailable, string amenities, string restrictions)
        {
            Name = name;
            Address = address;
            City = city;
            TotalNoOfRooms = totalNoOfRooms;
            NoOfRoomsAvailable = noOfRoomsAvailable;
            Amenities = amenities;
            Restrictions = restrictions;
        }

        //public Hotel(int hotelId, string name, int totalNoOfRooms, int noOfRoomsAvailable, double rating, string amenities, string restrictions)
        //{
        //    HotelId = hotelId;
        //    Name = name;
        //    TotalNoOfRooms = totalNoOfRooms;
        //    NoOfRoomsAvailable = noOfRoomsAvailable;
        //    Rating = rating;
        //    Amenities = amenities;
        //    Restrictions = restrictions;
        //}
    }
}
