using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[Route("lunch/recurring")]
[ApiController]
public class LunchRecurringItemsController : ControllerBase
{
    private readonly ILogger<LunchRecurringItemsController> _logger;
    private readonly IDatabaseService _databaseService;
    private readonly IMicrosoftGraphApiService _microsoftGraphApiService;

    public LunchRecurringItemsController(ILogger<LunchRecurringItemsController> logger, IDatabaseService databaseService, IMicrosoftGraphApiService microsoftGraphApiService)
    {
        _logger = logger;
        _databaseService = databaseService;
        _microsoftGraphApiService = microsoftGraphApiService;
    }

    private static string RegisterRecurringMessage(string officeName, string name, LunchRecurringItem lunchRecurringItem) =>
    "Hi,\n" +
    $"I would like to register for lunch at {officeName} Office on {lunchRecurringItem}.\n" +
    "Kind Regards,\n" +
    $"{name}";

    [HttpGet("get-registered-days")]
    public async Task<ActionResult<LunchRecurringItemFrontend>> GetRegisteredDays()
    {
        string accessToken = HttpContext.Session.GetString("accessToken");
        string microsoftId = (await _microsoftGraphApiService.GetMe(accessToken)).MicrosoftId;
        return new LunchRecurringItemFrontend(await _databaseService.GetRegisteredDays(microsoftId));
    }

    [HttpPut("update-registered-days")]
    public async Task<IActionResult> PutLunchRecurringItem(LunchRecurringItemFrontend lunchRecurringItemFrontend)
    {
        string accessToken = HttpContext.Session.GetString("accessToken");
        string microsoftId = (await _microsoftGraphApiService.GetMe(accessToken)).MicrosoftId;

        LunchRecurringItem lunchRecurringItem =
            new(microsoftId, lunchRecurringItemFrontend);
        await _databaseService.PutLunchRecurringItem(lunchRecurringItem);
        return NoContent();
    }

    //[HttpPost("create-lunch-recurring")]
    //public async Task<IActionResult> PostLunchRecurringItem(string microsoftId)
    //{
    //        string accessToken = HttpContext.Session.GetString("accessToken");
    //        string microsoftId = (await _microsoftGraphApiService.GetMe(accessToken)).MicrosoftId;
    //
    //        await _databaseService.PostLunchRecurringItem(microsoftId);
    //        return NoContent();
    //}

    [HttpPut("register-for-lunch-recurring")]
    public async Task<IActionResult> PutLunchRecurringRegistrationItem([FromQuery] string officeName)
    {
        string accessToken = HttpContext.Session.GetString("accessToken");
        string microsoftId = (await _microsoftGraphApiService.GetMe(accessToken)).MicrosoftId;

        var user = await _microsoftGraphApiService.GetMe(accessToken);

        LunchRecurringItem lunchRecurringItem = await _databaseService.GetRegisteredDays(microsoftId);

        await _microsoftGraphApiService.RegisterLunchRecurring(accessToken, RegisterRecurringMessage(officeName, $"{user.FirstName} {user.Surname}", lunchRecurringItem));

        LunchRecurringRegistrationItem lunchRecurringRegistrationItem = new() { MicrosoftId = microsoftId, LastRegistered = DateTime.Now };
        await _databaseService.PutLunchRecurringRegistrationItem(lunchRecurringRegistrationItem);
        return NoContent();
    }
}