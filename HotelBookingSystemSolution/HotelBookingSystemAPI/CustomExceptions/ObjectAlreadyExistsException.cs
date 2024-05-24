using System.Runtime.Serialization;

namespace HotelBookingSystemAPI.CustomExceptions
{
    public class ObjectAlreadyExistsException : Exception
    {
        public string msg = "";
        public ObjectAlreadyExistsException(string? message) 
        {
            msg = $"{message} Already Exists!";
        }

        public override string Message => msg;
    }
}