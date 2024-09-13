using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AreoDynamics.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/healthcheck")]
    public class HealthController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok("AreoDynamics API is in good shape.");
        }
    }
}
