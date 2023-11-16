using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[ApiController]
[Route("microsoft")]
public partial class MicrosoftGraphApiController : ControllerBase
{
    private readonly ILogger<MicrosoftGraphApiController> _logger;
    private readonly MicrosoftGraphApiService _microsoftGraphApiService;
    private readonly DatabaseService _databaseService;

    private const string FrontendUri = "http://localhost:5173";

    public MicrosoftGraphApiController(ILogger<MicrosoftGraphApiController> logger,
        MicrosoftGraphApiService microsoftGraphApiService, DatabaseService databaseService)
    {
        _logger = logger;
        _microsoftGraphApiService = microsoftGraphApiService;
        _databaseService = databaseService;
    }
}