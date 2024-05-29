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
        public double Rating { get; set; } = 0;
        public string Amenities { get; set; }
        public string Restrictions { get; set; }
        public bool IsAvailable { get; set; } 

        #region NavigationProperties
        public List<Room> Rooms { get; set; } 
        public List<Rating> Ratings { get; set; } 
        public List<RoomType> RoomTypes { get; set; }
        public List<Booking> bookingsForHotel { get; set; }
        public List<HotelAvailabilityByDate> hotelAvailabilityByDates { get; set; } 
        public List<HotelEmployee> employees { get; set; }
        #endregion

        public Hotel() { }

        public Hotel(string name, string address, string city, int totalNoOfRooms, double rating, string amenities, string restrictions, bool isAvailable)
        {
            Name = name;
            Address = address;
            City = city;
            TotalNoOfRooms = totalNoOfRooms;
            Rating = rating;
            Amenities = amenities;
            Restrictions = restrictions;
            IsAvailable = isAvailable;
        }

    }
}
