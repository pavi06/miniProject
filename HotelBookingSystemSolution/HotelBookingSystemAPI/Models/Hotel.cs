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
        public bool IsAvailable { get; set; } = true;
        public double? Rating { get; set; } = 0;
        public string Amenities { get; set; }
        public string Restrictions { get; set; }
        public string Status { get; set; } = "Active"; //for soft delete purpose

        public List<Room> Rooms { get; set; } //navigation property
        public List<Rating> Ratings { get; set; } //navigation property

    }
}
