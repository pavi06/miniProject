using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;
using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;

namespace HotelBookingSystemAPI.Services
{
    public class HotelEmployeeService : IHotelEmployeeService
    {
        public Task<List<BookingDetailsForEmployeeDTO>> GetAllBookingRequestDoneToday()
        {
            throw new NotImplementedException();
        }

        public Task<List<GuestDetailsForCheckInDTO>> GetAllCheckInForToday()
        {
            throw new NotImplementedException();
        }

        public Task<List<PaymentDetailsDTO>> GetAllPaymentsForBooking(int bookingId)
        {
            throw new NotImplementedException();
        }
    }
}
