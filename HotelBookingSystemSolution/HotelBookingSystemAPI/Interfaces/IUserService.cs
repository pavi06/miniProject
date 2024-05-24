using HotelBookingSystemAPI.Models.DTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IUserService
    {
        public Task<UserLoginReturnDTO> Login(UserLoginDTO loginDTO);
        public Task<GuestReturnDTO> Register(GuestRegisterDTO guestDTO);
        public Task<UserActivationDTO> GetUserForActivation(UserActivationDTO user);
    }
}
