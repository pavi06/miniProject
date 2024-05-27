namespace HotelBookingSystemAPI.Models.DTOs.PaymentDTOs
{
    public class PaymentDetailsDTO
    {
        public int PayId { get; set; }
        public int GuestId { get; set; }
        public double AmountPaid { get; set; }
        public string PaymentMode { get; set; }
        public DateTime PaymentDate { get; set; }

        public PaymentDetailsDTO(int payId, int guestId, double amountPaid, string paymentMode, DateTime paymentDate)
        {
            PayId = payId;
            GuestId = guestId;
            AmountPaid = amountPaid;
            PaymentMode = paymentMode;
            PaymentDate = paymentDate;
        }
    }
}
