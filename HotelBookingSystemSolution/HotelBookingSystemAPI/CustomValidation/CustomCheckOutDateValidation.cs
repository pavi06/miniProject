using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystemAPI.CustomValidation
{
    public class CustomCheckOutDateValidation : ValidationAttribute
    {
        private readonly string _checkInDate;

        public CustomCheckOutDateValidation(string checkInDatePropertyName)
        {
            _checkInDate = checkInDatePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var checkOutDate = (DateTime)value;
            var checkInDateProperty = validationContext.ObjectType.GetProperty(_checkInDate);

            if (checkInDateProperty == null)
            {
                throw new ArgumentException("Invalid property name.");
            }

            var checkInDate = (DateTime)checkInDateProperty.GetValue(validationContext.ObjectInstance);

            if (checkOutDate.Date <= checkInDate.Date)
            {
                return new ValidationResult(ErrorMessage ?? "Date should be greater than CheckInDate.");
            }

            return ValidationResult.Success;
        }
    }
}
