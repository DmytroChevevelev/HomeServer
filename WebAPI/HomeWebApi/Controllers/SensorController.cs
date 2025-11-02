using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/sensor")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Hello from MyController!" });
        }

        [HttpPost]
        public IActionResult Post([FromBody] object data)
        {
            return Ok(new { received = data });
        }

    }
}
