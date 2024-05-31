using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.BookingDTOs
{
    public class CancelRoomDTO
    {
        [Required(ErrorMessage ="RoomType cannot be null")]
        public string RoomType { get; set; }
        [Required(ErrorMessage ="No of rooms need to be provided")]
        public int NoOfRoomsToCancel { get; set; }
    }
}
