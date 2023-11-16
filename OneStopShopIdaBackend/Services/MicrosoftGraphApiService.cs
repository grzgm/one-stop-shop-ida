namespace OneStopShopIdaBackend.Services;

public partial class MicrosoftGraphApiService : IMicrosoftGraphApiService
{
    private readonly ILogger<MicrosoftGraphApiService> _logger;
    private readonly HttpClient _httpClient;
    private readonly CodeChallengeGeneratorService _codeChallengeGeneratorService;

    private const string LunchEmailAddress = "grzegorz.malisz@weareida.digital";

    private const string MicrosoftClientId = "ff6757d9-6533-46f4-99c7-32db8a7d606d";
    private const string Tenant = "organizations";
    private const string Scopes = "offline_access user.read mail.read mail.send calendars.readwrite";

    private const string RedirectUri = "http://localhost:3002/microsoft/auth/callback";

    public MicrosoftGraphApiService(ILogger<MicrosoftGraphApiService> logger, HttpClient httpClient,
        CodeChallengeGeneratorService codeChallengeGeneratorService)
    {
        _logger = logger;
        _httpClient = httpClient;
        _codeChallengeGeneratorService = codeChallengeGeneratorService;
    }
}