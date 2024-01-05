using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[Authorize]
[Route("lunch/recurring")]
[ApiController]
public class LunchRecurringItemsController : CustomControllerBase
{
    private readonly ILogger<LunchRecurringItemsController> _logger;
    private readonly IDatabaseService _databaseService;

    public LunchRecurringItemsController(ILogger<LunchRecurringItemsController> logger, IMemoryCache memoryCache,
        IDatabaseService databaseService, IMicrosoftGraphApiService microsoftGraphApiService) : base(memoryCache, 
        microsoftGraphApiService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    [HttpGet("get-registered-days")]
    public async Task<ActionResult<LunchRecurringItemFrontend>> GetRegisteredDays()
    {
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;
        return new LunchRecurringItemFrontend(await _databaseService.GetRegisteredDays(microsoftId));
    }

    [HttpPut("update-registered-days")]
    public async Task<IActionResult> PutLunchRecurringItem(LunchRecurringItemFrontend lunchRecurringItemFrontend)
    {
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;

        LunchRecurringItem lunchRecurringItem =
            new(microsoftId, lunchRecurringItemFrontend);
        await _databaseService.PutLunchRecurringItem(lunchRecurringItem);
        return Ok($"Days saved: {lunchRecurringItem}");
    }
}