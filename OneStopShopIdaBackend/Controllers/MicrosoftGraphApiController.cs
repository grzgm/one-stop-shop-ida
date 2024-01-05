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
    private readonly IMemoryCache _memoryCache;
    private readonly IDatabaseService _databaseService;
    private readonly string FrontendUri;
    
    public MicrosoftGraphApiController(ILogger<MicrosoftGraphApiController> logger, IConfiguration config, IMemoryCache memoryCache,
        IMicrosoftGraphApiService microsoftGraphApiService, IDatabaseService databaseService) : base(microsoftGraphApiService)
    {
        _logger = logger;
        _databaseService = databaseService;
        _config = config;
        _memoryCache = memoryCache;
        FrontendUri = _config["FrontendUri"];
    }
}