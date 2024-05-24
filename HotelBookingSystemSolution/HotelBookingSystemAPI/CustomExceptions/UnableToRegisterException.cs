using Microsoft.VisualBasic;
using System.Runtime.Serialization;

namespace HotelBookingSystemAPI.CustomExceptions
{
    internal class UnableToRegisterException : Exception
    {
        public string msg = "";
        public UnableToRegisterException()
        {
            msg = "Not able to register at this moment! Try again Later..";
        }

        public override string Message => msg;

    }
}