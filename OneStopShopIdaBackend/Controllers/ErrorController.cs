using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OneStopShopIdaBackend.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    [Route("/error-development")]
    public IActionResult HandleErrorDevelopment(
        [FromServices] IHostEnvironment hostEnvironment)
    {
        if (!hostEnvironment.IsDevelopment())
        {
            return NotFound();
        }

        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        var exceptionType = exceptionHandlerFeature.Error.GetType();

        if (exceptionType == typeof(HttpRequestException))
        {
            if (((HttpRequestException)exceptionHandlerFeature.Error).StatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized();
        }
        else if (exceptionType == typeof(InvalidOperationException))
        {
            return Conflict();
        }
        else if (exceptionType == typeof(KeyNotFoundException))
        {
            return NotFound();
        }
        return StatusCode(500);
        // return Problem(
        //     detail: exceptionHandlerFeature.Error.StackTrace,
        //     title: exceptionHandlerFeature.Error.GetType().ToString());
    }

    [Route("/error")]
    public IActionResult HandleError() =>
        Problem();
}