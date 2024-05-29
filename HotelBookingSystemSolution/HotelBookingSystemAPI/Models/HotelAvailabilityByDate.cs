using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class HotelAvailabilityByDate
    {
        public int HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
        public DateTime Date { get; set; }
        public int RoomsAvailableCount { get; set; }

        public HotelAvailabilityByDate() { }

        public HotelAvailabilityByDate(int hotelId, DateTime date, int roomsAvailableCount)
        {
            HotelId = hotelId;
            Date = date;
            RoomsAvailableCount = roomsAvailableCount;
        }
    }
}
