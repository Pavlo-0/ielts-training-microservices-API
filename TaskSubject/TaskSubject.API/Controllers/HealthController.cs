using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TaskSubject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public void Get()
        {
            _logger.LogInformation("Health check");
        }
    }
}
