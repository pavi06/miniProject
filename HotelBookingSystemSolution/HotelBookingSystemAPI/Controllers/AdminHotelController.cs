﻿using HotelBookingSystemAPI.CustomExceptions;
using HotelBookingSystemAPI.Interfaces;
using HotelBookingSystemAPI.Models;
using HotelBookingSystemAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelBookingSystemAPI.Models.DTOs.InsertDTOs;
using HotelBookingSystemAPI.Models.DTOs.HotelDTOs;

namespace HotelBookingSystemAPI.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminHotelController : ControllerBase
    {
        private readonly IAdminHotelService _hotelService;

        public AdminHotelController(IAdminHotelService hotelService) { 
            _hotelService = hotelService;
        }

        [HttpPost("RegisterHotel")]
        [ProducesResponseType(typeof(HotelReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelReturnDTO>> AddHotel([FromBody] HotelRegisterDTO hotel)
        {
            try
            {
                HotelReturnDTO result = await _hotelService.RegisterHotel(hotel);
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

        //[HttpPut("UpdateHotel")]
        //[ProducesResponseType(typeof(UpdateHotelReturnDTO), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<UpdateHotelReturnDTO>> UpdateHotel([FromBody] UpdateHotelDTO hotel)
        //{
        //    try
        //    {
        //        UpdateHotelReturnDTO result = await _hotelService.UpdateHotel(hotel);
        //        return Ok(result);
        //    }
        //    catch (ObjectNotAvailableException e)
        //    {
        //        return NotFound(new ErrorModel(404, e.Message));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ErrorModel(400, ex.Message));
        //    }

        //}

        [HttpPut("UpdateHotelAvailabilityStatus")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> UpdateHotelStatus([FromBody] UpdateHotelStatusDTO updateHotelStatusDTO)
        {
            try
            {
                bool result = await _hotelService.UpdateHotelAvailabilityService(updateHotelStatusDTO);
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

        //[HttpPut("UpdateHotelAvailabilityCount")]
        //[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<int>> UpdateHotelRoomAvailabilityCount([FromBody] int hotelId, [FromBody] int count)
        //{
        //    try
        //    {
        //        int result = await _hotelService.UpdateHotelRoomAvailabilityService(hotelId, count);
        //        return Ok(result);
        //    }
        //    catch (ObjectNotAvailableException e)
        //    {
        //        return NotFound(new ErrorModel(404, e.Message));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ErrorModel(400, ex.Message));
        //    }

        //}


    }
}