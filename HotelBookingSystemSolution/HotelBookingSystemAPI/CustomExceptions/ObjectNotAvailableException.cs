﻿using System.Runtime.Serialization;

namespace HotelBookingSystemAPI.CustomExceptions
{
    public class ObjectNotAvailableException : Exception
    {
        public string msg = "";
        public ObjectNotAvailableException(string? message)
        {
            msg = $"{message} Not available!";
        }

        public override string Message => msg;
    }
}