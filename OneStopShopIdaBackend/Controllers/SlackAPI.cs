using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers
{
    [ApiController]
    [Route("slack")]
    public partial class SlackAPIController : ControllerBase
    {
        private readonly ILogger<SlackAPIController> _logger;
        private readonly HttpClient _httpClient;
        private readonly CodeChallengeGeneratorService _codeChallengeGeneratorService;

        private const string SlackClientId = "12922529104.5894776759175";
        private const string SlackClientSecret = "cf00d1d6730ca5b040d79309530ecef6";
        //private const string Scopes = "channels%3Ahistory%2Cchannels%3Aread%2Cchat%3Awrite%2Cgroups%3Aread%2Cim%3Ahistory%2Cim%3Aread%2Cim%3Awrite%2Cmpim%3Ahistory%2Cmpim%3Aread%2Cusers.profile%3Aread%2Cusers.profile%3Awrite";
        private const string Scopes = "channels:history,channels:read,chat:write,groups:read,im:history,im:read,im:write,mpim:history,mpim:read,users.profile:read,users.profile:write";

        //private const string RedirectUri = "http://localhost:3002/slack/auth/callback";
        private const string RedirectUri = "https://4d9f-2a02-a442-e502-1-8121-e1d-2252-9c68.ngrok-free.app/slack/auth/callback";
        private const string FrontendUri = "http://localhost:5173";

        public SlackAPIController(ILogger<SlackAPIController> logger, HttpClient httpClient, CodeChallengeGeneratorService codeChallengeGeneratorService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _codeChallengeGeneratorService = codeChallengeGeneratorService;
        }
    }
}
