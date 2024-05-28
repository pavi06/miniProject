using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IGuestBookingService
    {
        /// <summary>
        /// Calculates the summary for the booking. Calculates the totalcost, discount, etc (similar to bill)
        /// </summary>
        /// <param name="bookDetails">Roomtype and no of rooms needed in thet type</param>
        /// <param name="loggedUserId">current user id</param>
        /// <param name="searchRooms">holds the detail of hotelId , checkin and checkout date</param>
        /// <returns></returns>
        public Task<BookingReturnDTO> BookRooms(List<BookDetailsDTO> bookDetails, int loggedUserId, SearchRoomsDTO searchRooms);

        /// <summary>
        /// method to confirm the booking and allocate rooms based on the payment made.
        /// If payment successful booking is confirmed else not confirmed.
        /// </summary>
        /// <param name="amount">Amount to be paid</param>
        /// <param name="loggedUser">logged user id</param>
        /// <param name="searchRooms">holds the detail of hotelId , checkin and checkout date</param>
        /// <returns></returns>
        public Task<PaymentReturnDTO> MakePayment(double amount,int loggedUser,SearchRoomsDTO searchRooms);


        /// <summary>
        /// After payment this method is called to allocate and update the availaility status.
        /// Rooms will be allocated and hotel rooms availability on that date is updated.
        /// </summary>
        /// <param name="bookingDetails">Bill generated for the booking</param>
        /// <param name="payId">payment id for the booking</param>
        /// <param name="loggedUser">logged user</param>
        /// <param name="searchRooms">details of hotel and checkin, checkout date</param>
        /// <returns></returns>
        public Task<BookingConfirmationDTO> ConfirmBooking(BookingReturnDTO bookingDetails, int payId, int loggedUser,SearchRoomsDTO searchRooms);


        /// <summary>
        /// To Cancel the booking made. Revert all the booked rooms and updates the availability status. 
        /// Calls the refund processing method to calculate the refund amount and to proceed with the transaction.
        /// </summary>
        /// <param name="bookId">bookingId</param>
        /// <param name="loggedUserId">loggedUser</param>
        /// <returns></returns>
        public Task<string> CancelBooking(int bookId, int loggedUserId);


        /// <summary>
        /// Displays all the bookings made by the user along with the booking details line no of rooms booked, amount , discount etc.
        /// </summary>
        /// <param name="loggedUserId">current userId</param>
        /// <returns></returns>
        public Task<List<MyBookingDTO>> GetMyBookings(int loggedUserId);

    }
}
