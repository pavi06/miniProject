namespace HotelBookingSystemAPI.Models.DTOs.BookingDTOs
{
    public class BookingConfirmationDTO
    {
        public int BookingId { get; set; }
        public string Status { get; set; }

        public BookingConfirmationDTO(int bookingId, string status)
        {
            BookingId = bookingId;
            Status = status;
        }
    }
}
