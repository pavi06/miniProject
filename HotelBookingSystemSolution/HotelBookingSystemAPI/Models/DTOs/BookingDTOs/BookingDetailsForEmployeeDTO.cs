namespace HotelBookingSystemAPI.Models.DTOs.BookingDTOs
{
    public class BookingDetailsForEmployeeDTO
    {
        public int BookingId { get; set; }
        public string GuestName { get; set; }
        public string PhoneNumber { get; set; }
        public Dictionary<string,int> RoomsBooked { get; set; }
        public string BookingStatus { get; set; }

        public BookingDetailsForEmployeeDTO(int bookingId, string guestName, string phoneNumber, Dictionary<string,int> rooms, string bookingStatus)
        {
            BookingId = bookingId;
            GuestName = guestName;
            PhoneNumber = phoneNumber;
            RoomsBooked = rooms;
            BookingStatus = bookingStatus;
        }
    }
}
