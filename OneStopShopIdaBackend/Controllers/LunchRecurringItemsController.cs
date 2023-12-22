using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[Route("lunch/recurring")]
[ApiController]
public class LunchRecurringItemsController : CustomControllerBase
{
    private readonly ILogger<LunchRecurringItemsController> _logger;
    private readonly IDatabaseService _databaseService;

    public LunchRecurringItemsController(ILogger<LunchRecurringItemsController> logger,
        IDatabaseService databaseService, IMicrosoftGraphApiService microsoftGraphApiService) : base(
        microsoftGraphApiService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    [HttpGet("get-registered-days")]
    public async Task<ActionResult<LunchRecurringItemFrontend>> GetRegisteredDays()
    {
        string accessToken = HttpContext.Session.GetString("accessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;
        return new LunchRecurringItemFrontend(await _databaseService.GetRegisteredDays(microsoftId));
    }

    [HttpPut("update-registered-days")]
    public async Task<IActionResult> PutLunchRecurringItem(LunchRecurringItemFrontend lunchRecurringItemFrontend)
    {
        string accessToken = HttpContext.Session.GetString("accessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;

        LunchRecurringItem lunchRecurringItem =
            new(microsoftId, lunchRecurringItemFrontend);
        await _databaseService.PutLunchRecurringItem(lunchRecurringItem);
        return NoContent();
    }
}