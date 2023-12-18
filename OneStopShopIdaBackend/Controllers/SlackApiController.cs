using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers
{
    [ApiController]
    [Route("slack")]
    public partial class SlackApiController : ControllerBase
    {
        private readonly ILogger<SlackApiController> _logger;
        private readonly IConfiguration _config;
        private readonly ISlackApiServices _slackApiServices;
        private readonly string FrontendUri;

        public SlackApiController(ILogger<SlackApiController> logger, IConfiguration config,
            ISlackApiServices slackApiServices)
        {
            _logger = logger;
            _config = config;
            _slackApiServices = slackApiServices;
            FrontendUri = config["FrontendUri"];
        }
    }
}