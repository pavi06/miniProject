using System.Diagnostics.CodeAnalysis;

namespace HotelBookingSystemAPI.Models
{
    [ExcludeFromCodeCoverage]
    public class ErrorModel
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }

        public ErrorModel(int statusCode, string description)
        {
            ErrorCode = statusCode;
            Message = description;
        }
    }
}
