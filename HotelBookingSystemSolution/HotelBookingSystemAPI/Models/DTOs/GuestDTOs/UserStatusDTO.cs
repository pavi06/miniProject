namespace HotelBookingSystemAPI.Models.DTOs.GuestDTOs
{
    public class UserStatusDTO
    {
        public int GuestId { get; set; }
        public string Status { get; set; }

        public UserStatusDTO() { }

        public UserStatusDTO(int guestId, string status)
        {
            GuestId = guestId;
            Status = status;
        }
    }
}
