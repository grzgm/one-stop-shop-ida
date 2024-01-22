using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers
{
    [ApiController]
    [Route("slack")]
    public partial class SlackApiController
    {
        private readonly ILogger<SlackApiController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly ISlackApiServices _slackApiServices;
        private readonly string _frontendUri;

        public SlackApiController(ILogger<SlackApiController> logger, IConfiguration config, IMemoryCache memoryCache,
            ISlackApiServices slackApiServices)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _slackApiServices = slackApiServices;
            _frontendUri = config["FrontendUri"] ?? string.Empty;
        }
    }
}