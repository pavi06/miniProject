﻿using System.Runtime.Serialization;

namespace HotelBookingSystemAPI.CustomExceptions
{
    [Serializable]
    public class UnauthorizedUserException : Exception
    {
        public string msg = "";
        public UnauthorizedUserException()
        {
            msg = "Invalid username or password";
        }
        public override string Message => msg;
    }
}