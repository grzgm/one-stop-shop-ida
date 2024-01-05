using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
    
    public MicrosoftGraphApiController(ILogger<MicrosoftGraphApiController> logger, IConfiguration config, IMemoryCache memoryCache,
        IMicrosoftGraphApiService microsoftGraphApiService, IDatabaseService databaseService) : base(memoryCache, microsoftGraphApiService)
    {
        _logger = logger;
        _config = config;
        _databaseService = databaseService;
        
        FrontendUri = _config["FrontendUri"];
    }
}