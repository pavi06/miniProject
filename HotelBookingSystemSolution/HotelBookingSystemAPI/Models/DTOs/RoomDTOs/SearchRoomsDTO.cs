namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class SearchRoomsDTO
    {
        public int HotelId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckoutDate { get; set; }
    }
}
