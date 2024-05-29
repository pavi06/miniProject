using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class RoomType
    {
        [Key]
        public int RoomTypeId { get; set; }
        public  string Type { get; set; }
        public string Images { get; set; }
        public int Occupancy { get; set; }
        public double Amount { get; set; }
        public int CotsAvailable { get; set; }
        public string Amenities { get; set; }
        public double Discount { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public RoomType() { }
        public RoomType(string type, int occupancy, string images, double amount, int cotsAvailable, string amenities, double discount, int hotelId)
        {
            Type = type;
            Occupancy = occupancy;
            Images = images;
            Amount = amount;
            CotsAvailable = cotsAvailable;
            Amenities = amenities;
            Discount = discount;
            HotelId = hotelId;
        }
    }
}
