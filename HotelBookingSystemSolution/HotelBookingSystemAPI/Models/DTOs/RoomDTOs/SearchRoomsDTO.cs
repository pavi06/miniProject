using HotelBookingSystemAPI.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.RoomDTOs
{
    public class SearchRoomsDTO
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="HotelId cannot be null")]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Date should be provided")]
        [CustomDateValidation(ErrorMessage = "Date should be greater than or equal to today")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Date should be provided")]
        [CustomDateValidation(ErrorMessage = "Date should be greater than or equal to today and greater than checkinDate")]
        [CustomCheckOutDateValidation("CheckInDate", ErrorMessage = "Date should be greater than checkinDate")]
        public DateTime CheckoutDate { get; set; }

        
    }
}
