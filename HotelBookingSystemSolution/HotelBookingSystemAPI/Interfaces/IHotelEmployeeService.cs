using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;
using HotelBookingSystemAPI.Models.DTOs.PaymentDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IHotelEmployeeService
    {

        public Task<List<GuestDetailsForCheckInDTO>> GetAllCheckInForToday(int loggedUserWorksFor);
        public Task<List<BookingDetailsForEmployeeDTO>> GetAllBookingRequestDoneToday(int loggedUserWorksFor);

    }
}
