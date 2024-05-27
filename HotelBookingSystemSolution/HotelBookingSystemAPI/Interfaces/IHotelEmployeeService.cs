using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;
using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IHotelEmployeeService
    {
        public Task<List<GuestDetailsForCheckInDTO>> GetAllCheckInForToday();
        public Task<List<BookingDetailsForEmployeeDTO>> GetAllBookingRequestDoneToday();
        public Task<List<PaymentDetailsDTO>> GetAllPaymentsForBooking(int bookingId);

    }
}
