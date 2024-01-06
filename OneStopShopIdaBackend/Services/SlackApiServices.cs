using OneStopShopIdaBackend.Controllers;

namespace OneStopShopIdaBackend.Services;

public partial class SlackApiServices : ISlackApiServices
{
    private readonly ILogger<SlackApiController> _logger;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private readonly CodeChallengeGeneratorService _codeChallengeGeneratorService;
    
    private readonly string RedirectUri;
    private readonly string SlackClientId;
    private readonly string SlackClientSecret;

    //private const string Scopes = "channels%3Ahistory%2Cchannels%3Aread%2Cchat%3Awrite%2Cgroups%3Aread%2Cim%3Ahistory%2Cim%3Aread%2Cim%3Awrite%2Cmpim%3Ahistory%2Cmpim%3Aread%2Cusers.profile%3Aread%2Cusers.profile%3Awrite";
    private const string Scopes =
        "channels:history,channels:read," +
        "chat:write," +
        "groups:read," +
        "im:history,im:read,im:write," +
        "mpim:history,mpim:read," +
        "users.profile:read,users.profile:write";

    public SlackApiServices(ILogger<SlackApiController> logger, HttpClient httpClient,
        CodeChallengeGeneratorService codeChallengeGeneratorService, IConfiguration config)
    {
        _logger = logger;
        _config = config;
        _httpClient = httpClient;
        _codeChallengeGeneratorService = codeChallengeGeneratorService;
        
        RedirectUri = _config["BackendUri"] + "/slack/auth/callback";
        SlackClientId = _config["Slack:SlackClientId"];
        SlackClientSecret = _config["Slack:SlackClientSecret"];
    }
}