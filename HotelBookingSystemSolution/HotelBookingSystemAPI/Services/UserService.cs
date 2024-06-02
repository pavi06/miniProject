using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using System.Security.Cryptography;
using System.Text;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;
using HotelBookingSystemAPI.Models.DTOs.EmployeeDTOs;
using System.Diagnostics.CodeAnalysis;

namespace HotelBookingSystemAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepo;
        private readonly IRepository<int, Guest> _guestRepo;
        private readonly IRepository<int, HotelEmployee> _empRepo;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserService> _logger;

        public UserService(IRepository<int, User> userRepo, IRepository<int, Guest> guestRepo, ITokenService tokenService, IRepository<int, HotelEmployee> empRepo, ILogger<UserService> logger)
        {
            _userRepo = userRepo;
            _guestRepo = guestRepo;
            _tokenService = tokenService;
            _empRepo = empRepo;
            _logger = logger;
        }

        #region Login
        public async Task<UserLoginReturnDTO> Login(UserLoginDTO loginDTO)
        {
            try
            {
                var guest = _guestRepo.Get().Result.SingleOrDefault(g => g.Email.ToLower() == loginDTO.Email.ToLower());
                if(guest == null)
                {
                    throw new ObjectNotAvailableException("User");
                }
                var userDB = await _userRepo.Get(guest.GuestId);
                if (userDB == null)
                {
                    _logger.LogCritical("Unauthorized access");
                    throw new UnauthorizedUserException();
                }
                HMACSHA512 hMACSHA = new HMACSHA512(userDB.PasswordHashKey);
                var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
                bool isPasswordSame = ComparePassword(encrypterPass, userDB.Password);
                if (isPasswordSame)
                {
                    if (userDB.Status == "Active")
                    {
                        UserLoginReturnDTO loginReturnDTO = await MapGuestToLoginReturn(guest);
                        _logger.LogInformation("User logged in successfully");
                        return loginReturnDTO;
                    }
                    _logger.LogError("User not avtivated yet!");
                    throw new UserNotActiveException();
                }
                _logger.LogCritical("Unauthorized access");
                throw new UnauthorizedUserException();
            }
            catch (ObjectsNotAvailableException)
            {
                _logger.LogCritical("User not available");
                throw new ObjectNotAvailableException("User");
            }
        }

        private async Task<UserLoginReturnDTO> MapGuestToLoginReturn(Guest guest)
        {
            UserLoginReturnDTO returnDTO = new UserLoginReturnDTO();
            returnDTO.UserName = guest.Name;
            returnDTO.Role = guest.Role ?? "User";
            returnDTO.AccessToken = _tokenService.GenerateToken(guest);
            var token = _tokenService.GenerateRefreshToken();
            returnDTO.RefreshToken = token.RfrshToken;
            var user = await _userRepo.Get(guest.GuestId);
            user.RefreshToken = token.RfrshToken;
            user.CreatedOn = token.Created;
            user.ExpiresOn = token.ExpiresOn;
            await _userRepo.Update(user);
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
                _logger.LogInformation("User registered successfuly");
                return addedGuest;
            }
            catch (ObjectAlreadyExistsException)
            {
                throw new ObjectAlreadyExistsException("User");
            }
            catch (Exception) { }
            if (guest != null)
                await RevertGuestInsert(guest);
            if (user != null && guest == null)
                await RevertUserInsert(user);
            _logger.LogInformation("Unable to register at this moment");
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
                    _logger.LogInformation("User activated");
                    return user;
                }
                catch (ObjectNotAvailableException)
                {
                    _logger.LogCritical("User not available");
                    throw new ObjectNotAvailableException("User");
                }
            }
            _logger.LogCritical("User not available");
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
                _logger.LogInformation("Employee registered successfully");
                return addedEmployee;
            }
            catch (ObjectAlreadyExistsException)
            {
                throw new ObjectAlreadyExistsException("User");
            }
            catch (Exception) {
               
            }
            _logger.LogCritical("Unable to register at this moment");
            throw new UnableToRegisterException();
        }

        #endregion

        #region EmployeeLogin
        public async Task<UserLoginReturnDTO> EmployeeLogin(UserLoginDTO loginDTO)
        {
            var emp = _empRepo.Get().Result.SingleOrDefault(g => g.Email.ToLower() == loginDTO.Email.ToLower());
            if (emp == null)
            {
                _logger.LogCritical("Unauthorized access");
                throw new UnauthorizedUserException();
            }
            HMACSHA512 hMACSHA = new HMACSHA512(emp.PasswordHashKey);
            var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            bool isPasswordSame = ComparePassword(encrypterPass, emp.Password);
            if (isPasswordSame)
            {
                if (emp.Status == "Active")
                {
                    UserLoginReturnDTO loginReturnDTO = await MapGuestToLoginReturn(emp);
                    _logger.LogInformation("User activated");
                    return loginReturnDTO;
                }
                _logger.LogError("User not avtivated yet!");
                throw new UserNotActiveException();
            }
            _logger.LogCritical("Unauthorized access");
            throw new UnauthorizedUserException();
        }

        private async Task<UserLoginReturnDTO> MapGuestToLoginReturn(HotelEmployee emp)
        {
            UserLoginReturnDTO returnDTO = new UserLoginReturnDTO();
            returnDTO.UserName = emp.Name;
            returnDTO.Role = emp.Role ?? "User";
            returnDTO.AccessToken = _tokenService.GenerateTokenForEmployee(emp);
            var token = _tokenService.GenerateRefreshToken();
            returnDTO.RefreshToken =token.RfrshToken;
            emp.RefreshToken = token.RfrshToken;
            emp.CreatedOn =token.Created;
            emp.ExpiresOn = token.ExpiresOn;
            await _empRepo.Update(emp);
            return returnDTO;
        }
        #endregion

    }
}
