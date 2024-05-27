using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystemAPI.Controllers
{
    [Authorize (Roles = "HotelEmployee")]
    [Route("api/[controller]")]
    [ApiController]
    public class HotelEmployeeController : ControllerBase
    {
    }
}
