using HotelBookingSystemAPI.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.Models.DTOs.HotelDTOs
{
    public class SearchHotelDTO
    {
        [Required(ErrorMessage = "Location should be specified")]
        public string Location { get; set; }


        [Required(ErrorMessage="Date should be provided")]
        [CustomDateValidation(ErrorMessage = "Date is out of Range")]
        public DateTime Date { get; set; }
    }
}
