using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[Route("lunch/today")]
[ApiController]
public class LunchTodayItemsController : CustomControllerBase
{
    private readonly ILogger<LunchTodayItemsController> _logger;
    private readonly IDatabaseService _databaseService;

    private const string _lunchEmailAddress = "grzegorz.malisz@weareida.digital";

    public LunchTodayItemsController(ILogger<LunchTodayItemsController> logger,
        IMicrosoftGraphApiService microsoftGraphApiService, IDatabaseService databaseService) : base(microsoftGraphApiService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }
    private static string RegisterTodayMessage(string officeName, string name) =>
        "Hi,\n" +
        $"I would like to register for today's lunch at {officeName} Office.\n" +
        "Kind Regards,\n" +
        $"{name}";
    private static string DeregisterTodayMessage(string officeName, string name) =>
        "Hi,\n" +
        $"I would like to deregister from today's lunch list at {officeName} Office.\n" +
        "Kind Regards,\n" +
        $"{name}";

    [HttpGet("is-registered")]
    public async Task<ActionResult<bool>> GetLunchTodayIsRegistered()
    {
        string accessToken = HttpContext.Session.GetString("accessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken)).MicrosoftId;

        return await _databaseService.GetLunchTodayIsRegistered(microsoftId);
    }

    [HttpPut("lunch-today-registration")]
    public async Task<IActionResult> PutLunchTodayRegistration([FromQuery] string officeName, [FromQuery] bool registration)
    {
        string accessToken = HttpContext.Session.GetString("accessToken");

        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken)).MicrosoftId;

        var user = await _microsoftGraphApiService.GetMe(accessToken);

        string message;

        if (registration)
        {
            message = RegisterTodayMessage(officeName, $"{user.FirstName} {user.Surname}");
        }
        else
        {
            message = DeregisterTodayMessage(officeName, $"{user.FirstName} {user.Surname}");
        }

        HttpResponseMessage response = await
            _microsoftGraphApiService.SendEmail(accessToken, _lunchEmailAddress, "Lunch Registration", message);

        if (response.IsSuccessStatusCode)
        {
            LunchTodayItem lunchTodayItem = new()
            {
                MicrosoftId = microsoftId,
                IsRegistered = registration,
            };

            await _databaseService.PutLunchTodayRegistration(lunchTodayItem);
        }

        return StatusCode((int)response.StatusCode);
    }
}