using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace OneStopShopIdaBackend.Controllers;

[Route("push")]
[ApiController]
public class PushController : CustomControllerBase
{
    private readonly ILogger<PushController> _logger;
    private readonly IHostingEnvironment _env;
    private readonly IDatabaseService _databaseService;

    public PushController(ILogger<PushController> logger, IHostingEnvironment hostingEnvironment, IDatabaseService databaseService, IMicrosoftGraphApiService microsoftGraphApiService) : base(microsoftGraphApiService)
    {
        _logger = logger;
        _env = hostingEnvironment;
        _databaseService = databaseService;
    }

    [HttpGet("is-subscribed")]
    public async Task<ActionResult<bool>> GetIsSubscribe()
    {
        string accessToken = HttpContext.Session.GetString("accessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken)).MicrosoftId;

        return await _databaseService.IsSubscribe(microsoftId);
    }

    [HttpPost("subscribe")]
    public async Task<ActionResult<PushSubscription>> Subscribe([FromBody] PushSubscriptionFrontend model)
    {
        string accessToken = HttpContext.Session.GetString("accessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken)).MicrosoftId;

        var subscription = new PushSubscription
        {
            MicrosoftId = microsoftId,
            Endpoint = model.Endpoint,
            ExpirationTime = model.ExpirationTime,
            Auth = model.Keys.Auth,
            P256Dh = model.Keys.P256Dh
        };

        return await _databaseService.Subscribe(subscription, microsoftId);
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
}