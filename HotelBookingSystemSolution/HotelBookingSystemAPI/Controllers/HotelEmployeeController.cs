using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models.DTOs.GuestDTOs;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelBookingSystemAPI.Models.DTOs.BookingDTOs;
using System.Security.Claims;
using HotelBookingSystemAPI.Models.DTOs.EmployeeDTOs;
using System.Diagnostics.CodeAnalysis;

namespace HotelBookingSystemAPI.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "HotelEmployee")]
    [Route("api/[controller]")]
    [ApiController]
    public class HotelEmployeeController : ControllerBase
    {
        private readonly IHotelEmployeeService _employeeService;
        private readonly IUserService _userService;
        private readonly IRepository<int, HotelEmployee> _employeeRepository;
        private readonly ILogger<HotelEmployeeController> _logger;

        public HotelEmployeeController(IHotelEmployeeService employeeService, IRepository<int,HotelEmployee> employeeRepository, IUserService userService, ILogger<HotelEmployeeController> logger)
        {
            _employeeService = employeeService; 
            _userService = userService;
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        #region EmployeeLogin
        [AllowAnonymous]
        [HttpPost("EmployeeLogin")]
        [ProducesResponseType(typeof(UserLoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserLoginReturnDTO>> Login([FromBody] UserLoginDTO empDTO)
        {
            try
            {
                var employee = await _userService.EmployeeLogin(empDTO);
                _logger.LogInformation("Empoyee logged in successfully");
                return Ok(employee);
            }
            catch (ObjectAlreadyExistsException e)
            {
                _logger.LogCritical("User not Authenticated");
                return BadRequest(new ErrorModel(400, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical("User not Authenticated");
                return BadRequest(new ErrorModel(400, ex.Message));
            }
        }
        #endregion

        #region EmployeeRegisteration
        [AllowAnonymous]
        [HttpPost("RegisterEmployee")]
        [ProducesResponseType(typeof(EmployeeRegisterReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeRegisterReturnDTO>> Register([FromBody] RegisterEmployeeDTO empDTO)
        {
            try
            {
                var employee = await _userService.RegisterEmployee(empDTO);
                _logger.LogInformation("Employee Registered successfully");
                return Ok(employee);
            }
            catch (ObjectAlreadyExistsException e)
            {
                _logger.LogCritical("User cannot register at this moment..User already Exists!");
                return BadRequest(new ErrorModel(400,e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }
        }
        #endregion

        #region GetAllBookingRequest
        [HttpGet("GetAllBookingRequest")]
        [ProducesResponseType(typeof(List<BookingDetailsForEmployeeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BookingDetailsForEmployeeDTO>>> GetAllBookings()
        {
            try
            {
                var loggedUserWorksFor = _employeeRepository.Get(Convert.ToInt32(User.FindFirstValue("UserId"))).Result.HotelId;
                var result = await _employeeService.GetAllBookingRequest(loggedUserWorksFor);
                _logger.LogInformation("Successfully retrieved bookings");
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
            {
                _logger.LogInformation(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message); 
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion

        #region GetFilteredBookings
        [HttpGet("GetAllBookingRequestByFiltering")]
        [ProducesResponseType(typeof(List<BookingDetailsForEmployeeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BookingDetailsForEmployeeDTO>>> GetAllBookingsByAttribute(string attribute, string attributeValue)
        {
            try
            {
                var loggedUserWorksFor = _employeeRepository.Get(Convert.ToInt32(User.FindFirstValue("UserId"))).Result.HotelId;
                var result = await _employeeService.GetAllBookingRequestByFilteration(loggedUserWorksFor, attribute,attributeValue );
                _logger.LogInformation("Bookings Retrieved successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion

        #region GetAllBookingRequestRaisedToday
        [HttpGet("GetAllBookingRequestRaisedToday")]
        [ProducesResponseType(typeof(List<BookingDetailsForEmployeeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BookingDetailsForEmployeeDTO>>> GetBookings()
        {
            try
            {
                var loggedUserWorksFor = _employeeRepository.Get(Convert.ToInt32(User.FindFirstValue("UserId"))).Result.HotelId;
                var result = await _employeeService.GetAllBookingRequestDoneToday(loggedUserWorksFor);
                _logger.LogInformation("Bookings retrieved successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion

        #region GetAllCheckInToday
        [HttpGet("GetAllCheckIn")]
        [ProducesResponseType(typeof(List<GuestDetailsForCheckInDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GuestDetailsForCheckInDTO>>> GetAllCheckIns()
        {
            try
            {
                var loggedUserWorksFor = _employeeRepository.Get(Convert.ToInt32(User.FindFirstValue("UserId"))).Result.HotelId;
                var result = await _employeeService.GetAllCheckInForToday(loggedUserWorksFor);
                _logger.LogInformation("GuestDetails retrieved successfully");
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }
        #endregion

    }
}
