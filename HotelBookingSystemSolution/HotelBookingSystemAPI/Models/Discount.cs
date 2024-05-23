using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Discount
    {
        public int HotelId { get; set; }

        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }

        public double DiscountPercent { get; set; }
        public RoomTypes RoomType { get; set; }

        [ForeignKey("RoomType")]
        public RoomType RType { get; set; }
    }
}
