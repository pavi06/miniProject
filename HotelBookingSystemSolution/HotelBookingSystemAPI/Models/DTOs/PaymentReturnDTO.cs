namespace HotelBookingSystemAPI.Models.DTOs
{
    public class PaymentReturnDTO
    {
        public int BookingId { get; set; }
        public string PaymentStatus { get; set; }

        public PaymentReturnDTO(int bookingId, string paymentStatus)
        {
            BookingId = bookingId;
            PaymentStatus = paymentStatus;
        }
    }
}
