namespace HotelBookingSystemAPI.Models.DTOs.BookingDTOs
{
    public class BookingDetailsForEmployeeDTO
    {
        public int BookingId { get; set; }
        public int GuestId { get; set; }
        public string PhoneNumber { get; set; }
        public List<BookDetailsDTO> RoomsNeeded { get; set; }

        public BookingDetailsForEmployeeDTO(int bookingId, int guestId, string phoneNumber, List<BookDetailsDTO> roomsNeeded)
        {
            BookingId = bookingId;
            GuestId = guestId;
            PhoneNumber = phoneNumber;
            RoomsNeeded = roomsNeeded;
        }
    }
}
