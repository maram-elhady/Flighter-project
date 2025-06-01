using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flighter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class securedController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetData()
        {
            return Ok("It works!");
        }
    }
}
