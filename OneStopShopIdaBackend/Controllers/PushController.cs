using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace OneStopShopIdaBackend.Controllers;

[Route("push")]
[ApiController]
public class PushController : ControllerBase
{
    private readonly ILogger<PushController> _logger;
    private readonly IHostingEnvironment _env;
    private readonly DatabaseService _databaseService;

    public PushController(ILogger<PushController> logger, IHostingEnvironment hostingEnvironment, DatabaseService databaseService)
    {
        _logger = logger;
        _env = hostingEnvironment;
        _databaseService = databaseService;
    }

    //[HttpGet, Route("vapidpublickey")]
    //public ActionResult<string> GetVapidPublicKey()
    //{
    //    return Ok(_databaseService.GetVapidPublicKey());
    //}

    [HttpGet("is-subscribed")]
    public async Task<ActionResult<bool>> GetIsSubscribe()
    {
        try
        {
            return await _databaseService.IsSubscribe(HttpContext.Session.GetString("microsoftId"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            return Conflict();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
    }

    [HttpPost("subscribe")]
    public async Task<ActionResult<PushSubscription>> Subscribe([FromBody] PushSubscriptionFrontend model)
    {
        var subscription = new PushSubscription
        {
            // MicrosoftId = Guid.NewGuid().ToString(), // You'd use your existing user id here
            // MicrosoftId = HttpContext.Session.GetString("microsoftId"), // You'd use your existing user id here
            MicrosoftId = "5e430c04-3186-4560-bdb2-6ecf691047a3", // You'd use your existing user id here
            Endpoint = model.Endpoint,
            ExpirationTime = model.ExpirationTime,
            Auth = model.Keys.Auth,
            P256Dh = model.Keys.P256Dh
        };

        return await _databaseService.Subscribe(subscription);
    }

    [HttpPost("unsubscribe")]
    public async Task<ActionResult<PushSubscription>> Unsubscribe([FromBody] PushSubscriptionFrontend model)
    {
        var subscription = new PushSubscription
        {
            Endpoint = model.Endpoint,
            ExpirationTime = model.ExpirationTime,
            Auth = model.Keys.Auth,
            P256Dh = model.Keys.P256Dh
        };

        await _databaseService.Unsubscribe(subscription);

        return subscription;
    }

    //[HttpPost("send/{userId}")]
    //public async Task<IActionResult> Send([FromRoute] string userId, [FromBody] Notification notification,
    //    [FromQuery] int? delay)
    //{
    //    if (!_env.IsDevelopment()) return Forbid();

    //    if (delay != null) Thread.Sleep((int)delay);

    //    await _databaseService.Send(userId, notification);

    //    return Accepted();
    //}
}