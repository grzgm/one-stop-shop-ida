namespace OneStopShopIdaBackend.Services;

public partial class MicrosoftGraphApiService : IMicrosoftGraphApiService
{
    private readonly ILogger<MicrosoftGraphApiService> _logger;
    private readonly HttpClient _httpClient;
    private readonly CodeChallengeGeneratorService _codeChallengeGeneratorService;
    private readonly string _backendUri;
    private readonly string _redirectUri;

    private readonly string _microsoftClientId;
    private const string Tenant = "organizations";
    private const string Scopes = "offline_access user.read mail.read mail.send calendars.readwrite";

    public MicrosoftGraphApiService(ILogger<MicrosoftGraphApiService> logger, IConfiguration config,
        HttpClient httpClient, CodeChallengeGeneratorService codeChallengeGeneratorService)
    {
        _logger = logger;
        _httpClient = httpClient;
        _codeChallengeGeneratorService = codeChallengeGeneratorService;
        _backendUri = config["BackendUri"] ?? string.Empty;
        _redirectUri = config["BackendUri"] + "/microsoft/auth/callback";
        _microsoftClientId = config["Microsoft:MicrosoftClientId"] ?? string.Empty;
    }
}