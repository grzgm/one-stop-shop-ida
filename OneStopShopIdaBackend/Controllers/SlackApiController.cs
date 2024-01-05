using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers
{
    [ApiController]
    [Route("slack")]
    public partial class SlackApiController : ControllerBase
    {
        private readonly ILogger<SlackApiController> _logger;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _memoryCache;
        private readonly ISlackApiServices _slackApiServices;
        private readonly string FrontendUri;

        public SlackApiController(ILogger<SlackApiController> logger, IConfiguration config, IMemoryCache memoryCache,
            ISlackApiServices slackApiServices)
        {
            _logger = logger;
            _config = config;
            _memoryCache = memoryCache;
            _slackApiServices = slackApiServices;
            FrontendUri = config["FrontendUri"];
        }
    }
}