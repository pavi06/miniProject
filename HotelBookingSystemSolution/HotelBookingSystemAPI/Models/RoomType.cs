using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public enum RoomTypes
    {
        Standard,
        Deluxe,
        SuperDeluxe,
        Suite,
        Single,
        Double
    }

    public class RoomType
    {
        [Key]
        public  RoomTypes Type { get; set; }
        public int Occupancy { get; set; }
        public double Amount { get; set; }
        public int CotsAvailable { get; set; }
        public string Amenities { get; set; }

        public int HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }

    }
}
