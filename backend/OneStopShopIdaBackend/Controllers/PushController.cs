using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[Authorize]
[Route("push")]
[ApiController]
public class PushController : CustomControllerBase
{
    private readonly IDatabaseService _databaseService;

    public PushController(IMemoryCache memoryCache, IDatabaseService databaseService,
        IMicrosoftGraphApiService microsoftGraphApiService) : base(memoryCache, microsoftGraphApiService)
    {
        _databaseService = databaseService;
    }

    [HttpGet("is-subscribed")]
    public async Task<ActionResult<bool>> GetIsSubscribe()
    {
        string accessToken = MemoryCache.Get<string>($"{User.FindFirst("UserId")?.Value}AccessToken") ?? string.Empty;
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(MicrosoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;

        return await _databaseService.IsSubscribe(microsoftId);
    }

    [HttpPost("subscribe")]
    public async Task<ActionResult<PushSubscription>> Subscribe([FromBody] PushSubscriptionFrontend model)
    {
        string accessToken = MemoryCache.Get<string>($"{User.FindFirst("UserId")?.Value}AccessToken") ?? string.Empty;
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(MicrosoftGraphApiService.GetMe, accessToken))
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