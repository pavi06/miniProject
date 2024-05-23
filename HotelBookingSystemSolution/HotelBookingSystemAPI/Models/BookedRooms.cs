using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class BookedRooms
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
