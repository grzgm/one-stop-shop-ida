using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[ApiController]
[Route("microsoft")]
public partial class MicrosoftGraphApiController : CustomControllerBase
{
    private readonly ILogger<MicrosoftGraphApiController> _logger;
    private readonly IDatabaseService _databaseService;

    private const string FrontendUri = "http://localhost:5173";

    public MicrosoftGraphApiController(ILogger<MicrosoftGraphApiController> logger,
        IMicrosoftGraphApiService microsoftGraphApiService, IDatabaseService databaseService) : base(microsoftGraphApiService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }
}