namespace HotelBookingSystemAPI.Models.DTOs.BookingDTOs
{
    public class BookingReturnDTO
    {
        public int NoOfRoomsBooked { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountPercent { get; set; }
        public double FinalAmount { get; set; }
        public double AdvancePayment { get; set; }

        public BookingReturnDTO(int noOfRoomsBooked, double totalAmount, double discountPercent, double finalAmount, double advancePayment)
        {
            NoOfRoomsBooked = noOfRoomsBooked;
            TotalAmount = totalAmount;
            DiscountPercent = discountPercent;
            FinalAmount = finalAmount;
            AdvancePayment = advancePayment;
        }
    }
}
