namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class UpdateRoomTypeDTO
    {
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }
}
