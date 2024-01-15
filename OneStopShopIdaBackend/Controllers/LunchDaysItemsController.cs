using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[Authorize]
[Route("lunch/days")]
[ApiController]
public class LunchDaysItemsController : CustomControllerBase
{
    private readonly IDatabaseService _databaseService;

    public LunchDaysItemsController(IMemoryCache memoryCache,
        IDatabaseService databaseService, IMicrosoftGraphApiService microsoftGraphApiService) : base(memoryCache, 
        microsoftGraphApiService)
    {
        _databaseService = databaseService;
    }

    [HttpGet("get-days")]
    public async Task<ActionResult<LunchDaysItemFrontend>> GetRegisteredDays()
    {
        string accessToken = MemoryCache.Get<string>($"{User.FindFirst("UserId")?.Value}AccessToken") ?? string.Empty;
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(MicrosoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;
        return new LunchDaysItemFrontend(await _databaseService.GetRegisteredDays(microsoftId));
    }

    [HttpPut("update-days")]
    public async Task<IActionResult> PutLunchDaysItem(LunchDaysItemFrontend lunchDaysItemFrontend)
    {
        string accessToken = MemoryCache.Get<string>($"{User.FindFirst("UserId")?.Value}AccessToken") ?? string.Empty;
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(MicrosoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;

        LunchDaysItem lunchDaysItem =
            new(microsoftId, lunchDaysItemFrontend);
        await _databaseService.PutLunchDaysItem(lunchDaysItem);
        return Ok($"Days saved: {lunchDaysItem}");
    }
}