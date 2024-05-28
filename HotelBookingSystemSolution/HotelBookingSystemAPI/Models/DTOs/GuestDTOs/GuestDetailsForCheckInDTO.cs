namespace HotelBookingSystemAPI.Models.DTOs.GuestDTOs
{
    public class GuestDetailsForCheckInDTO
    {
        public string GuestName { get; set; }
        public string GuestPhoneNumber { get; set; }

        public GuestDetailsForCheckInDTO(string guestName, string guestPhoneNumber)
        {
            GuestName = guestName;
            GuestPhoneNumber = guestPhoneNumber;
        }
    }
}
