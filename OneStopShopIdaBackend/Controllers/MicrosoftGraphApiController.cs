using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

[ApiController]
[Route("microsoft")]
public partial class MicrosoftGraphApiController : CustomControllerBase
{
    private readonly ILogger<MicrosoftGraphApiController> _logger;
    private readonly IConfiguration _config;
    private readonly IDatabaseService _databaseService;
    private readonly string FrontendUri;
    
    public MicrosoftGraphApiController(ILogger<MicrosoftGraphApiController> logger, IConfiguration config,
        IMicrosoftGraphApiService microsoftGraphApiService, IDatabaseService databaseService) : base(microsoftGraphApiService)
    {
        _logger = logger;
        _databaseService = databaseService;
        _config = config;
        FrontendUri = _config["FrontendUri"];
    }
}