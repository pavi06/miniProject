using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public enum RoomTypes
    {
        Standard = 1,
        Deluxe = 2,
        SuperDeluxe = 3,
        Suite = 4,
        Single = 5,
        Double = 6
    }

    public class RoomType
    {
        [Key]
        public int RoomTypeId { get; set; }
        public  RoomTypes Type { get; set; }
        public int Occupancy { get; set; }
        public double Amount { get; set; }
        public int CotsAvailable { get; set; }
        public string? Amenities { get; set; }
        public double Discount { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public RoomType(RoomTypes type, int occupancy, double amount, int cotsAvailable, string amenities, double discount, int hotelId)
        {
            Type = type;
            Occupancy = occupancy;
            Amount = amount;
            CotsAvailable = cotsAvailable;
            Amenities = amenities;
            Discount = discount;
            HotelId = hotelId;
        }
    }
}
