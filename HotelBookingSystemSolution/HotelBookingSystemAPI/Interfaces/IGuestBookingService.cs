using HotelBookingSystemAPI.Models.DTOs;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IGuestBookingService
    {
        public Task<BookingReturnDTO> BookRooms(List<BookDetailsDTO> bookDetails, int loggedUserId);
        public Task<int> MakePayment(double amount);
        public Task<string> ConfirmBooking(BookingReturnDTO bookingDetails, int payId, int loggedUser);
        public Task<string> CancelBooking(int bookId);
        public Task<string> ProvideRating(AddRatingDTO ratingDTO, int loggedUser);

    }
}
