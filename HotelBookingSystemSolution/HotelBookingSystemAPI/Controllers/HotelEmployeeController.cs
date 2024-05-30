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

        public HotelEmployeeController(IHotelEmployeeService employeeService, IRepository<int,HotelEmployee> employeeRepository, IUserService userService)
        {
            _employeeService = employeeService; 
            _userService = userService;
            _employeeRepository = employeeRepository;
        }

        [AllowAnonymous]
        [HttpPost("EmployeeLogin")]
        [ProducesResponseType(typeof(UserLoginReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserLoginReturnDTO>> Login(UserLoginDTO empDTO)
        {
            try
            {
                var employee = await _userService.EmployeeLogin(empDTO);
                return Ok(employee);
            }
            catch (ObjectAlreadyExistsException e)
            {
                return BadRequest(new ErrorModel(400, e.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost("RegisterEmployee")]
        [ProducesResponseType(typeof(EmployeeRegisterReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeRegisterReturnDTO>> Register(RegisterEmployeeDTO empDTO)
        {
            try
            {
                var employee = await _userService.RegisterEmployee(empDTO);
                return Ok(employee);
            }
            catch (ObjectAlreadyExistsException e)
            {
                return BadRequest(new ErrorModel(400,e.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }
        }

        [HttpGet("GetAllBookingRequestRaisedToday")]
        [ProducesResponseType(typeof(List<BookingDetailsForEmployeeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BookingDetailsForEmployeeDTO>>> GetBookings()
        {
            try
            {
                var loggedUserWorksFor = _employeeRepository.Get(Convert.ToInt32(User.FindFirstValue("UserId"))).Result.HotelId;
                var result = await _employeeService.GetAllBookingRequestDoneToday(loggedUserWorksFor);
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpGet("GetAllCheckIn")]
        [ProducesResponseType(typeof(List<GuestDetailsForCheckInDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GuestDetailsForCheckInDTO>>> GetAllCheckIns()
        {
            try
            {
                var loggedUserWorksFor = _employeeRepository.Get(Convert.ToInt32(User.FindFirstValue("UserId"))).Result.HotelId;
                var result = await _employeeService.GetAllCheckInForToday(loggedUserWorksFor);
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
            {
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

    }
}
