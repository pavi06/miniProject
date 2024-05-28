using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.CustomValidation
{
    public class CustomDateValidation : RangeAttribute
    {
        public CustomDateValidation()
              : base(typeof(DateTime),
                      DateTime.Now.ToShortDateString(),
                      DateTime.Now.AddYears(1).ToShortDateString()){ 
        } 
    }
 
}
