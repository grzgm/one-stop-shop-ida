using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace OneStopShopIdaBackend.Controllers;

[Authorize]
[Route("push")]
[ApiController]
public class PushController : CustomControllerBase
{
    private readonly ILogger<PushController> _logger;
    private readonly IHostingEnvironment _env;
    private readonly IDatabaseService _databaseService;

    public PushController(ILogger<PushController> logger, IMemoryCache memoryCache,
        IHostingEnvironment hostingEnvironment, IDatabaseService databaseService,
        IMicrosoftGraphApiService microsoftGraphApiService) : base(memoryCache, microsoftGraphApiService)
    {
        _logger = logger;
        _env = hostingEnvironment;
        _databaseService = databaseService;
    }

    [HttpGet("is-subscribed")]
    public async Task<ActionResult<bool>> GetIsSubscribe()
    {
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;

        return await _databaseService.IsSubscribe(microsoftId);
    }

    [HttpPost("subscribe")]
    public async Task<ActionResult<PushSubscription>> Subscribe([FromBody] PushSubscriptionFrontend model)
    {
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;

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