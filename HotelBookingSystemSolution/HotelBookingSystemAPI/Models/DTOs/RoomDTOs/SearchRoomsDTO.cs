using HotelBookingSystemAPI.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class SearchRoomsDTO
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="HotelId cannot be null")]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Date should be provided")]
        [CustomDateValidation(ErrorMessage = "Date is out of Range")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Date should be provided")]
        [CustomDateValidation(ErrorMessage = "Date is out of Range")]
        public DateTime CheckoutDate { get; set; }
    }
}
