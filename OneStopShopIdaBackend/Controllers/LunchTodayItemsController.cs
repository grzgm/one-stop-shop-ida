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
        IMicrosoftGraphApiService microsoftGraphApiService, IDatabaseService databaseService) : base(
        microsoftGraphApiService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    private static string RegisterTodayMessage(string office, string name) =>
        "Hi,\n" +
        $"I would like to register for today's lunch at {office} Office.\n" +
        "Kind Regards,\n" +
        $"{name}";

    private static string DeregisterTodayMessage(string office, string name) =>
        "Hi,\n" +
        $"I would like to deregister from today's lunch list at {office} Office.\n" +
        "Kind Regards,\n" +
        $"{name}";

    [HttpGet("get-registration")]
    public async Task<ActionResult<LunchTodayItemFrontend>> GetLunchTodayIsRegistered()
    {
        string accessToken = HttpContext.Session.GetString("accessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;

        return new LunchTodayItemFrontend(await _databaseService.GetLunchTodayIsRegistered(microsoftId));
    }

    [HttpPut("put-registration")]
    public async Task<ActionResult<LunchTodayItemFrontend>> PutLunchTodayRegistration([FromQuery] bool registration, [FromQuery] string office)
    {
        string accessToken = HttpContext.Session.GetString("accessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;
        office = office.ToLower();
        var user = await _microsoftGraphApiService.GetMe(accessToken);

        string message;

        if (registration)
        {
            message = RegisterTodayMessage(office, $"{user.FirstName} {user.Surname}");
        }
        else
        {
            message = DeregisterTodayMessage(office, $"{user.FirstName} {user.Surname}");
        }

        HttpResponseMessage response = await
            _microsoftGraphApiService.SendEmail(accessToken, _lunchEmailAddress, "Lunch Registration", message);
        
        LunchTodayItem lunchTodayItem = new()
        {
            MicrosoftId = microsoftId,
            RegistrationDate = registration ? DateTime.Now : null,
            Office = registration ? office : null,
        };
        
        if (response.IsSuccessStatusCode)
        {
            await _databaseService.PutLunchTodayRegistration(lunchTodayItem);
        }

        return new LunchTodayItemFrontend(lunchTodayItem);
    }
}