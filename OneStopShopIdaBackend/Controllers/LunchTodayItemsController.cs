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

    [HttpGet("is-registered")]
    public async Task<ActionResult<bool>> GetLunchTodayIsRegistered()
    {
        string accessToken = HttpContext.Session.GetString("accessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken)).MicrosoftId;

        return await _databaseService.GetLunchTodayIsRegistered(microsoftId);
    }

    [HttpPut("register-lunch-today")]
    public async Task<IActionResult> PutLunchTodayRegister([FromQuery] string officeName)
    {
        string accessToken = HttpContext.Session.GetString("accessToken");

        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken)).MicrosoftId;

        var user = await _microsoftGraphApiService.GetMe(accessToken);
        HttpResponseMessage response = await
            _microsoftGraphApiService.RegisterLunchToday(accessToken,
                microsoftId, RegisterTodayMessage(officeName, $"{user.FirstName} {user.Surname}"));

        if (response.IsSuccessStatusCode)
        {
            LunchTodayItem lunchTodayItem = new()
            {
                MicrosoftId = microsoftId,
                IsRegistered = true,
            };

            await _databaseService.PutLunchTodayRegister(lunchTodayItem);
        }

        return StatusCode((int)response.StatusCode);
    }
}