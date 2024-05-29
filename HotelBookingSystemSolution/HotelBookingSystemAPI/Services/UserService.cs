using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using System.Security.Cryptography;
using System.Text;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;
using HotelBookingSystemAPI.Models.DTOs.EmployeeDTOs;

namespace HotelBookingSystemAPI.Services
{
    public class UserService : IUserService
    {

        private readonly IRepository<int, User> _userRepo;
        private readonly IRepository<int, Guest> _guestRepo;
        private readonly IRepository<int, HotelEmployee> _empRepo;
        private readonly ITokenService _tokenService;

        public UserService(IRepository<int, User> userRepo, IRepository<int, Guest> guestRepo, ITokenService tokenService, IRepository<int, HotelEmployee> empRepo)
        {
            _userRepo = userRepo;
            _guestRepo = guestRepo;
            _tokenService = tokenService;
            _empRepo = empRepo;
        }

        #region Login
        public async Task<UserLoginReturnDTO> Login(UserLoginDTO loginDTO)
        {
            var guest = _guestRepo.Get().Result.SingleOrDefault(g => g.Email.ToLower() == loginDTO.Email.ToLower());
            var userDB = await _userRepo.Get(guest.GuestId);
            if (userDB == null)
            {
                throw new UnauthorizedUserException();
            }
            HMACSHA512 hMACSHA = new HMACSHA512(userDB.PasswordHashKey);
            var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            bool isPasswordSame = ComparePassword(encrypterPass, userDB.Password);
            if (isPasswordSame)
            {
                if (userDB.Status == "Active")
                {
                    UserLoginReturnDTO loginReturnDTO = MapGuestToLoginReturn(guest);
                    return loginReturnDTO;
                }

                throw new UserNotActiveException();
            }
            throw new UnauthorizedUserException();
        }

        private UserLoginReturnDTO MapGuestToLoginReturn(Guest guest)
        {
            UserLoginReturnDTO returnDTO = new UserLoginReturnDTO();
            returnDTO.UserName = guest.Name;
            returnDTO.Role = guest.Role ?? "User";
            returnDTO.Token = _tokenService.GenerateToken(guest);
            return returnDTO;
        }

        private bool ComparePassword(byte[] encrypterPass, byte[] password)
        {
            for (int i = 0; i < encrypterPass.Length; i++)
            {
                if (encrypterPass[i] != password[i])
                {
                    return false;
                }
            }
            return true;
        }

        #endregion


        #region Register
        public async Task<GuestReturnDTO> Register(GuestRegisterDTO guestDTO)
        {
            Guest guest = null;
            User user = null;
            try
            {
                guest = new Guest(guestDTO.Name, guestDTO.Email, guestDTO.PhoneNumber, guestDTO.Address);
                user = MapGuestDTOToUser(guestDTO);
                guest = await _guestRepo.Add(guest);
                user.GuestId = guest.GuestId;
                user = await _userRepo.Add(user);
                GuestReturnDTO addedGuest = new GuestReturnDTO(guest.Name, guest.Email, guest.Address, guest.PhoneNumber, guest.Role);
                return addedGuest;
            }
            catch (Exception) { }
            if (guest != null)
                await RevertGuestInsert(guest);
            if (user != null && guest == null)
                await RevertUserInsert(user);
            throw new UnableToRegisterException();
        }

        private async Task RevertUserInsert(User user)
        {
            await _userRepo.Delete(user.GuestId);
        }

        private async Task RevertGuestInsert(Guest guest)
        {

            await _guestRepo.Delete(guest.GuestId);
        }

        private User MapGuestDTOToUser(GuestRegisterDTO guestDTO)
        {
            User user = new User();
            user.Status = "Disabled";
            HMACSHA512 hMACSHA = new HMACSHA512();
            user.PasswordHashKey = hMACSHA.Key;
            user.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(guestDTO.Password));
            //user.RefreshToken = _tokenService.GenerateRefreshToken();
            //user.ExpiresOn = DateTime.Now.AddDays(1);
            return user;
        }
        #endregion 


        #region UserActivation
        public async Task<UserActivationDTO> GetUserForActivation(UserActivationDTO user)
        {
            var userRetrived = await _userRepo.Get(user.GuestId);
            if (userRetrived != null)
            {
                if (userRetrived.Status == "Active")
                    throw new Exception("User Already Activated!");
                userRetrived.Status = "Active";
                try
                {
                    var updatedUSer = await _userRepo.Update(userRetrived);
                    user.Status = updatedUSer.Status;
                    return user;
                }
                catch (ObjectNotAvailableException)
                {
                    throw new ObjectNotAvailableException("User");
                }
            }
            throw new ObjectNotAvailableException("User");

        }
        #endregion


        #region EmployeeRegister
        public async Task<EmployeeRegisterReturnDTO> RegisterEmployee(RegisterEmployeeDTO empDTO)
        {
            HotelEmployee employee = null;
            try
            {
                employee = new HotelEmployee(empDTO.HotelId,empDTO.Name,empDTO.Email,empDTO.PhoneNumber,empDTO.Address);
                employee.Status = "Disabled";
                HMACSHA512 hMACSHA = new HMACSHA512();
                employee.PasswordHashKey = hMACSHA.Key;
                employee.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(empDTO.Password));
                employee = await _empRepo.Add(employee);
                EmployeeRegisterReturnDTO addedEmployee = new EmployeeRegisterReturnDTO(employee.Name, employee.Email, employee.Address, employee.PhoneNumber, employee.Role);
                return addedEmployee;
            }
            catch (Exception) {
               
            }
            throw new UnableToRegisterException();
        }

        #endregion

        #region EmployeeLogin
        public async Task<UserLoginReturnDTO> EmployeeLogin(UserLoginDTO loginDTO)
        {
            var emp = _empRepo.Get().Result.SingleOrDefault(g => g.Email.ToLower() == loginDTO.Email.ToLower());
            if (emp == null)
            {
                throw new UnauthorizedUserException();
            }
            HMACSHA512 hMACSHA = new HMACSHA512(emp.PasswordHashKey);
            var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            bool isPasswordSame = ComparePassword(encrypterPass, emp.Password);
            if (isPasswordSame)
            {
                if (emp.Status == "Active")
                {
                    UserLoginReturnDTO loginReturnDTO = MapGuestToLoginReturn(emp);
                    return loginReturnDTO;
                }

                throw new UserNotActiveException();
            }
            throw new UnauthorizedUserException();
        }

        private UserLoginReturnDTO MapGuestToLoginReturn(HotelEmployee emp)
        {
            UserLoginReturnDTO returnDTO = new UserLoginReturnDTO();
            returnDTO.UserName = emp.Name;
            returnDTO.Role = emp.Role ?? "User";
            returnDTO.Token = _tokenService.GenerateTokenForEmployee(emp);
            return returnDTO;
        }
        #endregion
    }
}
