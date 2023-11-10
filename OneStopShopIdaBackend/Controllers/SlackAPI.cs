using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers
{
    [ApiController]
    [Route("slack")]
    public partial class SlackAPIController : ControllerBase
    {
        private readonly ILogger<SlackAPIController> _logger;
        private readonly SlackAPIServices _slackAPIServices;

        private const string FrontendUri = "http://localhost:5173";

        public SlackAPIController(ILogger<SlackAPIController> logger, SlackAPIServices slackAPIServices)
        {
            _logger = logger;
            _slackAPIServices = slackAPIServices;
        }
    }
}
