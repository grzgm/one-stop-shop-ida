namespace OneStopShopIdaBackend.Services;

public partial class MicrosoftGraphApiService : IMicrosoftGraphApiService
{
    private readonly ILogger<MicrosoftGraphApiService> _logger;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private readonly CodeChallengeGeneratorService _codeChallengeGeneratorService;
    private readonly string BackendUri;
    private readonly string RedirectUri;

    private readonly string MicrosoftClientId;
    private const string Tenant = "organizations";
    private const string Scopes = "offline_access user.read mail.read mail.send calendars.readwrite";

    public MicrosoftGraphApiService(ILogger<MicrosoftGraphApiService> logger, IConfiguration config,
        HttpClient httpClient, CodeChallengeGeneratorService codeChallengeGeneratorService)
    {
        _logger = logger;
        _config = config;
        _httpClient = httpClient;
        _codeChallengeGeneratorService = codeChallengeGeneratorService;
        BackendUri = _config["BackendUri"];
        RedirectUri = _config["BackendUri"] + "/microsoft/auth/callback";
        MicrosoftClientId = _config["Microsoft:MicrosoftClientId"];
    }
}