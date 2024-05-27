namespace HotelBookingSystemAPI.Models.DTOs.BookingDTOs
{
    public class BookingReturnDTO
    {
        public int HotelId { get; set; }
        public int NoOfRoomsBooked { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountPercent { get; set; }
        public double FinalAmount { get; set; }
        public double AdvancePayment { get; set; }

        public BookingReturnDTO(int hotelId, int noOfRoomsBooked, DateTime checkinDate, DateTime checkoutDate,double totalAmount, double discountPercent, double finalAmount, double advancePayment)
        {
            HotelId = hotelId;
            NoOfRoomsBooked = noOfRoomsBooked;
            CheckInDate = checkinDate;
            CheckOutDate = checkoutDate;
            TotalAmount = totalAmount;
            DiscountPercent = discountPercent;
            FinalAmount = finalAmount;
            AdvancePayment = advancePayment;
        }
    }
}
