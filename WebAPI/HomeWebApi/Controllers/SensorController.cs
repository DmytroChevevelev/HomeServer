using HomeWebApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/sensor")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly HomeSensorsContext _dbContext;
        public SensorController(HomeSensorsContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var sensors = _dbContext.Sensors.ToList();
            return Ok(sensors);
        }

        [HttpPost]
        public IActionResult Post([FromBody] object data)
        {
            return Ok(new { received = data });
        }

    }
}
