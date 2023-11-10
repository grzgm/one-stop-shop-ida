using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[ApiController]
[Route("microsoft")]
public partial class MicrosoftGraphAPIController : ControllerBase
{
    private readonly ILogger<MicrosoftGraphAPIController> _logger;
    private readonly MicrosoftGraphAPIService _microsoftGraphApiService;
    private readonly UserItemsController _userItemsController;
    private readonly LunchTodayItemsController _lunchTodayItemsController;
    private readonly LunchRecurringItemsController _lunchRecurringItemsController;

    private const string FrontendUri = "http://localhost:5173";

    public MicrosoftGraphAPIController(ILogger<MicrosoftGraphAPIController> logger,
        MicrosoftGraphAPIService microsoftGraphApiService, UserItemsController userItemsController,
        LunchTodayItemsController lunchTodayItemsController, LunchRecurringItemsController lunchRecurringItemsController)
    {
        _logger = logger;
        _microsoftGraphApiService = microsoftGraphApiService;
        _userItemsController = userItemsController;
        _lunchTodayItemsController = lunchTodayItemsController;
        _lunchRecurringItemsController = lunchRecurringItemsController;
    }
}