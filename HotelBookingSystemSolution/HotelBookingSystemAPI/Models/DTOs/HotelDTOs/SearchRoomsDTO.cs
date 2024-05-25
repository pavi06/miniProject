namespace HotelBookingSystemAPI.Models.DTOs.HotelDTOs
{
    public class SearchRoomsDTO
    {
        public int HotelId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckoutDate { get; set; }
    }
}
