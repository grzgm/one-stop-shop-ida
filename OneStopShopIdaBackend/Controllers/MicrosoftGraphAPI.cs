using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[ApiController]
[Route("microsoft")]
public partial class MicrosoftGraphAPIController : ControllerBase
{
    private readonly ILogger<MicrosoftGraphAPIController> _logger;
    private readonly MicrosoftGraphAPIService _microsoftGraphApiService;
    private readonly DatabaseService _databaseService;

    private const string FrontendUri = "http://localhost:5173";

    public MicrosoftGraphAPIController(ILogger<MicrosoftGraphAPIController> logger,
        MicrosoftGraphAPIService microsoftGraphApiService, DatabaseService databaseService)
    {
        _logger = logger;
        _microsoftGraphApiService = microsoftGraphApiService;
        _databaseService = databaseService;
    }
}