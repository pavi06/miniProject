namespace HotelBookingSystemAPI.CustomExceptions
{
    public class ObjectsNotAvailableException : Exception
    {
        public string msg = "";
        public ObjectsNotAvailableException(string? message)
        {
            msg = $"No {message} are available!";
        }

        public override string Message => msg;
    }
}
