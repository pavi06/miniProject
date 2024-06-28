namespace HotelBookingSystemAPI.Models.DTOs.BookingDTOs
{
    public class ModifyBookingDTO
    {
        public int BookingId { get; set; }
        public List<CancelRoomDTO> CancelRooms { get; set; }

        public ModifyBookingDTO(int bookingId,List<CancelRoomDTO> cancelRooms)
        {
            BookingId = bookingId;
            CancelRooms = cancelRooms;

        }
    }
}
