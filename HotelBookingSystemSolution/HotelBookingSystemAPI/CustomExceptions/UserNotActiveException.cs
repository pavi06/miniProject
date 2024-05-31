using System.Runtime.Serialization;

namespace HotelBookingSystemAPI.CustomExceptions
{
    public class UserNotActiveException : Exception
    {
        public string msg = "";
        public UserNotActiveException()
        {
            msg = "Your account is not activated";
        }

        public override string Message => msg;

    }
}