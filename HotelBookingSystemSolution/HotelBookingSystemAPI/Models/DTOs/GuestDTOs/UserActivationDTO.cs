namespace HotelBookingSystemAPI.Models.DTOs.GuestDTOs
{
    public class UserActivationDTO
    {
        public int GuestId { get; set; }
        public string Status { get; set; }

        public UserActivationDTO(int guestId, string status)
        {
            GuestId = guestId;
            Status = status;
        }
    }
}
