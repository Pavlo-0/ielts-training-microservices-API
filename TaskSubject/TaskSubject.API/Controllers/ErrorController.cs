using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskSubject.Core.LogEvents;

namespace TaskSubject.API.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("api/error-local-development")]
        public IActionResult ErrorLocalDevelopment(
            [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            _logger.LogInformation(MyLogEvents.ErrorController, "Start error controller process in debug mode");

            if (webHostEnvironment.EnvironmentName != "Development")
            {
                _logger.LogWarning(MyLogEvents.ErrorController, "This shouldn't be invoked in non-development environments.");
                throw new InvalidOperationException(
                    "This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            _logger.LogError(MyLogEvents.ErrorController, "Details : {StackTrace} : {message: }", context?.Error?.StackTrace, context?.Error?.Message);

            return Problem(
                detail: context?.Error?.StackTrace,
                title: context?.Error?.Message);
        }

        [Route("api/error")]
        public IActionResult Error()
        {
            _logger.LogInformation(MyLogEvents.ErrorController, "Start error controller process");

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            _logger.LogDebug(MyLogEvents.ErrorController, "Details : {StackTrace} : {message: }", context?.Error?.StackTrace, context?.Error?.Message);

            return Problem();
        }
    }
}
