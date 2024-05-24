using System.Runtime.Serialization;

namespace HotelBookingSystemAPI.CustomExceptions
{
    [Serializable]
    internal class UserNotActiveException : Exception
    {
        public string msg = "";
        public UserNotActiveException()
        {
            msg = "Your account is not activated";
        }

        public override string Message => msg;

    }
}