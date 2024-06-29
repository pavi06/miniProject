using HotelBookingSystemAPI.Models.DTOs.EmployeeDTOs;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;

namespace HotelBookingSystemAPI.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Method for login process. check if a person already an user or not
        /// </summary>
        /// <param name="loginDTO">Dto with email and password value</param>
        /// <returns>return dto which holds token along with user role</returns>
        public Task<UserLoginReturnDTO> Login(UserLoginDTO loginDTO);

        /// <summary>
        /// Method for employee login 
        /// </summary>
        /// <param name="loginDTO">dto with email and password for authentication</param>
        /// <returns>return dto which holds token along with user role</returns>
        public Task<UserLoginReturnDTO> EmployeeLogin(UserLoginDTO loginDTO);
        public Task<GuestReturnDTO> Register(GuestRegisterDTO guestDTO);
        public Task<EmployeeRegisterReturnDTO> RegisterEmployee(RegisterEmployeeDTO empDTO);
        public Task<UserStatusDTO> UpdateUserStatus(UserStatusDTO user);
        public Task<List<AllInActiveUsersDTO>> GetAllUsersForActivation();
    }
}
