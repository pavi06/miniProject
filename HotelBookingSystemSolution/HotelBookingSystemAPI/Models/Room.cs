using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public int TypeId { get; set; }
        [ForeignKey("TypeId")]
        public RoomType RoomType { get; set; } //navigation property
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } //navigation property
        public string? Images { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string Status { get; set; } //for soft delete

        public List<BookedRooms> roomBookings { get; set; }

    }
}
