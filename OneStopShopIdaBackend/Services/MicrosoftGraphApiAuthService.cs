namespace OneStopShopIdaBackend.Services;

public partial class MicrosoftGraphApiService
{
    // OAuth Step 1: Redirect users to microsoft's authorization URL
    public string GenerateMicrosoftGraphAPIAuthUrl(string route)
    {
        string authUrl =
            $"https://login.microsoftonline.com/{Tenant}/oauth2/v2.0/authorize?" +
            $"client_id={_microsoftClientId}" +
            $"&response_type=code" +
            $"&redirect_uri={_redirectUri}" +
            $"&response_mode=query" +
            $"&scope={Scopes}" +
            $"&state={route}" +
            $"&code_challenge={_codeChallengeGeneratorService.CodeChallenge}" +
            $"&code_challenge_method=S256";

        return authUrl;
    }

    // OAuth Step 2: Handle the OAuth callback
    public async Task<(string, string)> CallAuthCallback(string code)
    {
        var data = new Dictionary<string, string>
        {
            { "client_id", _microsoftClientId },
            { "scope", Scopes },
            { "code", code },
            { "redirect_uri", _redirectUri },
            { "grant_type", "authorization_code" },
            { "code_verifier", _codeChallengeGeneratorService.CodeVerifier }
        };

        var content = new FormUrlEncodedContent(data);
        content.Headers.Clear();
        content.Headers.Add("content-type", "application/x-www-form-urlencoded");
        content.Headers.Add("Origin", _backendUri);

        HttpResponseMessage response =
            await _httpClient.PostAsync("https://login.microsoftonline.com/organizations/oauth2/v2.0/token",
                content);
        response.EnsureSuccessStatusCode();

        string responseData = await response.Content.ReadAsStringAsync();
        dynamic responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData) ?? new Object();

        // Access the access_token property
        string accessToken = responseObject.access_token;
        string refreshToken = responseObject.refresh_token;

        return (accessToken, refreshToken);
    }

    public async Task<(string, string)> CallAuthRefresh(string refreshToken)
    {
        try
        {
            var data = new Dictionary<string, string>
            {
                { "client_id", _microsoftClientId },
                { "scope", Scopes },
                { "refresh_token", refreshToken },
                { "redirect_uri", _redirectUri },
                { "grant_type", "refresh_token" },
            };

            var content = new FormUrlEncodedContent(data);
            content.Headers.Clear();
            content.Headers.Add("content-type", "application/x-www-form-urlencoded");
            content.Headers.Add("Origin", _backendUri);

            HttpResponseMessage response =
                await _httpClient.PostAsync("https://login.microsoftonline.com/organizations/oauth2/v2.0/token",
                    content);
            response.EnsureSuccessStatusCode();

            string responseData = await response.Content.ReadAsStringAsync();
            dynamic responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData) ?? new Object();

            // Access the access_token property
            string newAccessToken = responseObject.access_token;
            string newRefreshToken = responseObject.refresh_token;

            return (newAccessToken, newRefreshToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error calling Microsoft Refresh Token API: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calling Microsoft Refresh Token API: {ex.Message}");
            throw;
        }
    }
}