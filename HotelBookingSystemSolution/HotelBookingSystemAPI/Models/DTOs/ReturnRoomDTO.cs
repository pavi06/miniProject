using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystemAPI.Models.DTOs
{
    public class ReturnRoomDTO
    {
        public int RoomId { get; set; }
        public int TypeId { get; set; }
        public int HotelId { get; set; }
        public string? Images { get; set; }
        public bool IsAvailable { get; set; }

        public ReturnRoomDTO(int roomId, int typeId, int hotelId, string? images, bool isAvailable)
        {
            RoomId = roomId;
            TypeId = typeId;
            HotelId = hotelId;
            Images = images;
            IsAvailable = isAvailable;
        }
    }
}
