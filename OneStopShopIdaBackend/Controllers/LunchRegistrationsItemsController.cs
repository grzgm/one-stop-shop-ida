using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[Authorize]
[Route("lunch/registrations")]
[ApiController]
public class LunchRegistrationsItemsController : CustomControllerBase
{
    private readonly ISlackApiServices _slackApiServices;
    private readonly IDatabaseService _databaseService;

    private readonly string _lunchEmailAddress;
    private readonly string _lunchSlackChannel;

    public LunchRegistrationsItemsController(IConfiguration config,
        IMemoryCache memoryCache, IMicrosoftGraphApiService microsoftGraphApiService,
        ISlackApiServices slackApiServices, IDatabaseService databaseService) : base(memoryCache,
        microsoftGraphApiService)
    {
        var config1 = config;
        _slackApiServices = slackApiServices;
        _databaseService = databaseService;

        _lunchEmailAddress = config1["LunchEmailAddress"] ?? string.Empty;
        _lunchSlackChannel = config1["LunchSlackChannel"] ?? string.Empty;
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
    public async Task<ActionResult<LunchRegistrationsItemFrontend>> GetLunchIsRegisteredToday()
    {
        string accessToken = MemoryCache.Get<string>($"{User.FindFirst("UserId")?.Value}AccessToken") ?? string.Empty;
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(MicrosoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;

        return new LunchRegistrationsItemFrontend(await _databaseService.GetLunchIsRegisteredToday(microsoftId));
    }

    [HttpPut("put-registration")]
    public async Task<ActionResult<LunchRegistrationsItemFrontend>> PutLunchRegistrationItem([FromQuery] bool registration,
        [FromQuery] string office)
    {
        string accessToken = MemoryCache.Get<string>($"{User.FindFirst("UserId")?.Value}AccessToken") ?? string.Empty;
        string microsoftId = (await ExecuteWithRetryMicrosoftGraphApi(MicrosoftGraphApiService.GetMe, accessToken))
            .MicrosoftId;
        office = office.ToLower();
        var user = await MicrosoftGraphApiService.GetMe(accessToken);

        string message;

        if (registration)
        {
            message = RegisterTodayMessage(office, $"{user.FirstName} {user.Surname}");
        }
        else
        {
            message = DeregisterTodayMessage(office, $"{user.FirstName} {user.Surname}");
        }

        HttpResponseMessage? response = null;

        LunchRegistrationsItem lunchRegistrationsItem = new()
        {
            MicrosoftId = microsoftId,
            RegistrationDate = registration ? DateTime.Now : null,
            Office = registration ? office : null,
        };

        if (office == "utrecht")
        {
            response = await
                MicrosoftGraphApiService.SendEmail(accessToken, _lunchEmailAddress, "Lunch Registration", message);
        }
        else if (office == "amsterdam")
        {
            string slackAccessToken = MemoryCache.Get<string>($"{User.FindFirst("UserId")?.Value}SlackAccessToken") ?? string.Empty;
            response = await _slackApiServices.SendMessage(slackAccessToken, message, _lunchSlackChannel);
        }

        if (response is { IsSuccessStatusCode: true })
        {
            await _databaseService.PutLunchRegistrationItem(lunchRegistrationsItem);
        }

        return new LunchRegistrationsItemFrontend(lunchRegistrationsItem);
    }
}