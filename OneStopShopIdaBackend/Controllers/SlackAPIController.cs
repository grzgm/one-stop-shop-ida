using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers
{
    [ApiController]
    [Route("slack")]
    public partial class SlackApiController : ControllerBase
    {
        private readonly ILogger<SlackApiController> _logger;
        private readonly SlackApiServices _slackApiServices;

        private const string FrontendUri = "http://localhost:5173";

        public SlackApiController(ILogger<SlackApiController> logger, SlackApiServices slackApiServices)
        {
            _logger = logger;
            _slackApiServices = slackApiServices;
        }
    }
}
