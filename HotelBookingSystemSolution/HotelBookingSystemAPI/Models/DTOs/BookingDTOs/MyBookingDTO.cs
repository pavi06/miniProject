namespace HotelBookingSystemAPI.Models.DTOs.BookingDTOs
{
    public class MyBookingDTO
    {
        public int HotelId { get; set; }
        public int NoOfRoomsBooked { get; set; }
        public DateTime BookedDate { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountPercent { get; set; }
        public double FinalAmount { get; set; }

        public MyBookingDTO(int hotelId, int noOfRoomsBooked, DateTime bookedDate, double totalAmount, double discountPercent, double finalAmount)
        {
            HotelId = hotelId;
            NoOfRoomsBooked = noOfRoomsBooked;
            BookedDate = bookedDate;
            TotalAmount = totalAmount;
            DiscountPercent = discountPercent;
            FinalAmount = finalAmount;
        }
    }
}
