using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace OneStopShopIdaBackend.Controllers;

[Route("push")]
[ApiController]
public class PushController : ControllerBase
{
    private readonly IHostingEnvironment _env;
    private readonly IPushService _pushService;

    public PushController(IHostingEnvironment hostingEnvironment, IPushService pushService)
    {
        _env = hostingEnvironment;
        _pushService = pushService;
    }

    [HttpGet, Route("vapidpublickey")]
    public ActionResult<string> GetVapidPublicKey()
    {
        return Ok(_pushService.GetVapidPublicKey());
    }

    [HttpPost("subscribe")]
    public async Task<ActionResult<PushSubscription>> Subscribe([FromBody] PushSubscriptionViewModel model)
    {
        var subscription = new PushSubscription
        {
            // MicrosoftId = Guid.NewGuid().ToString(), // You'd use your existing user id here
            MicrosoftId = "5e430c04-3186-4560-bdb2-6ecf691047a3", // You'd use your existing user id here
            Endpoint = model.Subscription.Endpoint,
            ExpirationTime = model.Subscription.ExpirationTime,
            Auth = model.Subscription.Keys.Auth,
            P256Dh = model.Subscription.Keys.P256Dh
        };

        return await _pushService.Subscribe(subscription);
    }

    [HttpPost("unsubscribe")]
    public async Task<ActionResult<PushSubscription>> Unsubscribe([FromBody] PushSubscriptionViewModel model)
    {
        var subscription = new PushSubscription
        {
            Endpoint = model.Subscription.Endpoint,
            ExpirationTime = model.Subscription.ExpirationTime,
            Auth = model.Subscription.Keys.Auth,
            P256Dh = model.Subscription.Keys.P256Dh
        };

        await _pushService.Unsubscribe(subscription);

        return subscription;
    }

    [HttpPost("send/{userId}")]
    public async Task<IActionResult> Send([FromRoute] string userId, [FromBody] Notification notification,
        [FromQuery] int? delay)
    {
        if (!_env.IsDevelopment()) return Forbid();

        if (delay != null) Thread.Sleep((int)delay);

        await _pushService.Send(userId, notification);

        return Accepted();
    }
}

public class PushSubscriptionViewModel
{
    public Subscription Subscription { get; set; }
}

public class Subscription
{
    public string Endpoint { get; set; }

    public double? ExpirationTime { get; set; }

    public Keys Keys { get; set; }

    public WebPush.PushSubscription ToWebPushSubscription() =>
        new WebPush.PushSubscription(Endpoint, Keys.P256Dh, Keys.Auth);
}

public class Keys
{
    public string P256Dh { get; set; }
    public string Auth { get; set; }
}