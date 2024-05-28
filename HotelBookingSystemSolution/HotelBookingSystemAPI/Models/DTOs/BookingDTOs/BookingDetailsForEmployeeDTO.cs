namespace HotelBookingSystemAPI.Models.DTOs.BookingDTOs
{
    public class BookingDetailsForEmployeeDTO
    {
        public int BookingId { get; set; }
        public string GuestName { get; set; }
        public string PhoneNumber { get; set; }
        public Dictionary<string,int> RoomsNeeded { get; set; }

        public BookingDetailsForEmployeeDTO(int bookingId, string guestName, string phoneNumber, Dictionary<string,int> roomsNeeded)
        {
            BookingId = bookingId;
            GuestName = guestName;
            PhoneNumber = phoneNumber;
            RoomsNeeded = roomsNeeded;
        }
    }
}
