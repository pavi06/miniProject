using System.Runtime.Serialization;

namespace HotelBookingSystemAPI.CustomExceptions
{
    [Serializable]
    internal class ObjectNotAvailableException : Exception
    {
        public ObjectNotAvailableException()
        {
        }

        public ObjectNotAvailableException(string? message) : base(message)
        {
        }

        public ObjectNotAvailableException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ObjectNotAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}