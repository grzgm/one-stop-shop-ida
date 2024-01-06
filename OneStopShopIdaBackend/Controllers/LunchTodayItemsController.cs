using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[Authorize]
[Route("lunch/today")]
[ApiController]
public class LunchTodayItemsController : CustomControllerBase
{
    private readonly ILogger<LunchTodayItemsController> _logger;
    private readonly ISlackApiServices _slackApiServices;
    private readonly IDatabaseService _databaseService;

    private const string LunchEmailAddress = "grzegorz.malisz@weareida.digital";
    private const string LunchSlackChannel = "D05QWNGJMAR";

    public LunchTodayItemsController(ILogger<LunchTodayItemsController> logger, IMemoryCache memoryCache,
        IMicrosoftGraphApiService microsoftGraphApiService, ISlackApiServices slackApiServices,
        IDatabaseService databaseService) : base(memoryCache,
        microsoftGraphApiService)
    {
        _logger = logger;
        _slackApiServices = slackApiServices;
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
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;

        return new LunchTodayItemFrontend(await _databaseService.GetLunchTodayIsRegistered(microsoftId));
    }

    [HttpPut("put-registration")]
    public async Task<ActionResult<LunchTodayItemFrontend>> PutLunchTodayRegistration([FromQuery] bool registration,
        [FromQuery] string office)
    {
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
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

        HttpResponseMessage response = null;
        
        LunchTodayItem lunchTodayItem = new()
        {
            MicrosoftId = microsoftId,
            RegistrationDate = registration ? DateTime.Now : null,
            Office = registration ? office : null,
        };

        if (office == "utrecht")
        {
            response = await
                _microsoftGraphApiService.SendEmail(accessToken, LunchEmailAddress, "Lunch Registration", message);
        }
        else if (office == "amsterdam")
        {
            string slackAccessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}SlackAccessToken");
            response = await _slackApiServices.SendMessage(slackAccessToken, message, LunchSlackChannel);
        }

        if (response.IsSuccessStatusCode)
        {
            await _databaseService.PutLunchTodayRegistration(lunchTodayItem);
        }

        return new LunchTodayItemFrontend(lunchTodayItem);
    }
}