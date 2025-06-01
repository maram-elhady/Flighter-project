using Flighter.Models;
using Flighter.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flighter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerservice;

        public OfferController( IOfferService offerservice)
        {
            _offerservice = offerservice;
        }

        [HttpGet("available-offers")]
        public async Task<IActionResult> GetOffers()
        {
            var result = await _offerservice.GetAvailableOffersAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("offers/{offerPercentage}")]
        public async Task<IActionResult> GetTicketsByOfferPercentage(string offerPercentage)
        {
            var result = await _offerservice.GetTicketsByOfferPercentageAsync(offerPercentage);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
