using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;
using HotelBookingSystemAPI.Models.DTOs.RoomDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IGuestBookingService
    {
        public Task<BookingReturnDTO> BookRooms(List<BookDetailsDTO> bookDetails, int loggedUserId, SearchRoomsDTO searchRooms);
        public Task<PaymentReturnDTO> MakePayment(double amount,int loggedUser,SearchRoomsDTO searchRooms);
        public Task<BookingConfirmationDTO> ConfirmBooking(BookingReturnDTO bookingDetails, int payId, int loggedUser,SearchRoomsDTO searchRooms);
        public Task<string> CancelBooking(int bookId);
        public Task<List<MyBookingDTO>> GetMyBookings(int loggedUserId);

    }
}
