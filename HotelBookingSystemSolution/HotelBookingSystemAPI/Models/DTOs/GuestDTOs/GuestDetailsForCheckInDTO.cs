namespace HotelBookingSystemAPI.Models.DTOs.GuestDTOs
{
    public class GuestDetailsForCheckInDTO
    {
        public string GuestName { get; set; }
        public string GuestPhoneNumber { get; set; }
        public Dictionary<string, int> RoomsBooked { get; set; }

        public GuestDetailsForCheckInDTO(string guestName, string guestPhoneNumber, Dictionary<string, int> roomsBooked)
        {
            GuestName = guestName;
            GuestPhoneNumber = guestPhoneNumber;
            RoomsBooked = roomsBooked;
        }
    }
}
