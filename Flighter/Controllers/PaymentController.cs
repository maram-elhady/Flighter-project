using CompanyDashboard.Pages;
using Flighter.DTO.FlightDto;
using Flighter.Helper;
using Flighter.Models;
using Flighter.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flighter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentservice;

        public PaymentController( IPaymentService paymentservice)
        {
            _paymentservice = paymentservice;
        }
        [HttpPost("paylater/paynow")]
        public async Task<IActionResult> CreateBooking([FromBody] PayDto request)
        {
            var result = await _paymentservice.CreateBookingAsync(request);

            if (!result.Success)
                return BadRequest(new ApiResponse<object> { Success = false, Message = result.Message });

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Data = result.Data,
                Message = result.Message
            });
        }

        [HttpPost("pay-history")]
        public async Task<IActionResult> historypay([FromBody] PayhistoryDto request)
        {
            var result = await _paymentservice.PaymenthistoryAsync(request);

            if (!result.Success)
                return BadRequest(new ApiResponse<object> { Success = false, Message = result.Message });

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Data = result.Data,
                Message = result.Message
            });
        }

        [HttpPost("Refund/{bookingid}")]
        public async Task<IActionResult> Refund([FromRoute] int bookingid)
        {
            var result = await _paymentservice.RefundAsync(bookingid);

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
