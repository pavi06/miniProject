using HotelBookingSystemAPI.Models.DTOs.EmployeeDTOs;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IUserService
    {
        public Task<UserLoginReturnDTO> Login(UserLoginDTO loginDTO);
        public Task<GuestReturnDTO> Register(GuestRegisterDTO guestDTO);
        public Task<EmployeeRegisterReturnDTO> RegisterEmployee(RegisterEmployeeDTO empDTO);
        public Task<UserActivationDTO> GetUserForActivation(UserActivationDTO user);
    }
}
