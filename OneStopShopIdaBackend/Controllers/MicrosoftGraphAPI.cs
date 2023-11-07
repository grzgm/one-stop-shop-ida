using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[ApiController]
[Route("microsoft")]
public partial class MicrosoftGraphAPIController : ControllerBase
{
    private readonly ILogger<MicrosoftGraphAPIController> _logger;
    private readonly HttpClient _httpClient;
    private readonly CodeChallengeGeneratorService _codeChallengeGeneratorService;
    private readonly MicrosoftGraphAPIService _microsoftGraphApiService;
    private readonly UserItemsController _userItemsController;
    private readonly LunchTodayItemsController _lunchTodayItemsController;

    private const string FrontendUri = "http://localhost:5173";

    public MicrosoftGraphAPIController(ILogger<MicrosoftGraphAPIController> logger, HttpClient httpClient,
        CodeChallengeGeneratorService codeChallengeGeneratorService,
        MicrosoftGraphAPIService microsoftGraphApiService, UserItemsController userItemsController,
        LunchTodayItemsController lunchTodayItemsController)
    {
        _logger = logger;
        _httpClient = httpClient;
        _codeChallengeGeneratorService = codeChallengeGeneratorService;
        _microsoftGraphApiService = microsoftGraphApiService;
        _userItemsController = userItemsController;
        _lunchTodayItemsController = lunchTodayItemsController;
    }
}