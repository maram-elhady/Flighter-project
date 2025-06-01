using Flighter.DTO.FlightDto;
using Flighter.Models;
using Flighter.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flighter.Controllers
{
    [Route("Flight")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightservice;

        public FlightController( IFlightService flightservice)
        {
            _flightservice = flightservice;
        }

        [HttpGet("companies")]
        public async Task<ActionResult<List<GetcompanyDto>>> GetCompanies()
        {
            var companies = await _flightservice.GetAllCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("FlightTypes")]
        public async Task<ActionResult<List<GetFlighttypesDto>>> GetFlightTypes()
        {
            var flights = await _flightservice.GetAllFlightTypesAsync();
            return Ok(flights);
        }

        [HttpGet("ClassTypes")]
        public async Task<ActionResult<List<GetClasstypesDto>>> GetClassTypes()
        {
            var classes = await _flightservice.GetAllClassTypesAsync();
            return Ok(classes);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTickets([FromQuery] GetTicketDto ticketdto)
        {
            var result = await _flightservice.SearchTicketsAsync(ticketdto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("from")]
        public async Task<IActionResult> GetFromLocations()
        {
            var result = await _flightservice.GetFromLocationsAsync();
            return Ok(result);
        }

        [HttpGet("to")]
        public async Task<IActionResult> GetToLocations()
        {
            var result = await _flightservice.GetToLocationsAsync();
            return Ok(result);
        }

        [HttpGet("seats-status /{ticket_id}")]
        public async Task<IActionResult> SeatsStatus([FromRoute] int ticket_id)
        {
            var result = await _flightservice.SeatsStatusAsync(ticket_id);
            return result.Success ? Ok(result) : BadRequest(result);
        }


        [HttpPost("ticket-summary")]
        public async Task<IActionResult> GetTicketSummary([FromBody] TicketSummaryRequestDto request)
        {
            var result = await _flightservice.GetTicketSummaryAsync(request);

            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);

                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("user-bookings/{userid}")]
        public async Task<IActionResult> GetUserBookings([FromRoute] string userid)
        {
            var result = await _flightservice.GetUserBookingsAsync(userid);

            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);

                return BadRequest(result);
            }

            return Ok(result);
        }

        


    }
}