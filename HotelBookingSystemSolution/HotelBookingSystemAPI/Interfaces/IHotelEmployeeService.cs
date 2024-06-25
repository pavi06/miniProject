using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;
using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IHotelEmployeeService
    {
        /// <summary>
        /// To get all the booking request raised for the hotel.
        /// </summary>
        /// <param name="loggedUserWorksFor">id of the hotel for which the employee works for</param>
        /// <returns>returns list of bookings after mapping it with dto</returns>
        public Task<List<BookingDetailsForEmployeeDTO>> GetAllBookingRequest(int loggedUserWorksFor);


        /// <summary>
        /// To get all the booking request raised for the hotel based on the attribute value(date ->  month, date).
        /// </summary>
        /// <param name="loggedUserWorksFor">id of the hotel for which the employee works for</param>
        /// <returns>returns list of bookings after mapping it with dto</returns>
        public Task<List<BookingDetailsForEmployeeDTO>> GetAllBookingRequestByFilteration(int loggedUserWorksFor, FilterBookingDTO filterBookingDTO);

        /// <summary>
        /// Get those guest details who are checkin today
        /// </summary>
        /// <param name="loggedUserWorksFor">id of the hotel for which the employee works for</param>
        /// <returns>return list of customer</returns>
        public Task<List<GuestDetailsForCheckInDTO>> GetAllCheckInForToday(int loggedUserWorksFor);

        /// <summary>
        /// To get and display the bookings  raised by someone 
        /// </summary>
        /// <param name="loggedUserWorksFor">id of the hotel for which the employee works for</param>
        /// <returns></returns>
        public Task<List<BookingDetailsForEmployeeDTO>> GetAllBookingRequestDoneToday(int loggedUserWorksFor);

    }
}
