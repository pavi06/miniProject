namespace HotelBookingSystemAPI.Models.DTOs.BookingDTOs
{
    public class MyBookingDTO
    {
        public int BookId { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string HotelLocation { get; set; }
        public int NoOfRoomsBooked { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountPercent { get; set; }
        public double FinalAmount { get; set; }
        public string BookingStatus { get; set; }

        public MyBookingDTO() { }
        public MyBookingDTO(int bookId,int hotelId, string hotelName, int noOfRoomsBooked, DateTime checkInDate, DateTime checkOutDate,  double totalAmount, double discountPercent, double finalAmount, string hotelLocation, string bookingStatus)
        {
            BookId = bookId;
            HotelId = hotelId;
            HotelName = hotelName;
            NoOfRoomsBooked = noOfRoomsBooked;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            TotalAmount = totalAmount;
            DiscountPercent = discountPercent;
            FinalAmount = finalAmount;
            HotelLocation = hotelLocation;
            BookingStatus = bookingStatus;
        }
    }
}
