namespace OneStopShopIdaBackend.Services;
public partial class SlackApiServices
{
    public string GenerateSlackApiAuthUrl(string route)
    {
        string authUrl =
        $"https://slack.com/oauth/v2/authorize?client_id={_slackClientId}&&scope=&user_scope={Scopes}&redirect_uri={_redirectUri}&state={route}";

        return authUrl;
    }
    public async Task<string> CallAuthCallback(string code, string state)
    {
        try
        {
            var formData = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "code", code },
                    { "client_id", _slackClientId },
                    { "client_secret", _slackClientSecret }
            });

            HttpResponseMessage response = await _httpClient.PostAsync("https://slack.com/api/oauth.v2.access", formData);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            dynamic responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData) ?? new Object();

            // Access the access_token property
            string slackAccessToken = responseObject.authed_user.access_token;

            return slackAccessToken;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{this.GetType().Name}\nError calling external API: {ex.Message}");
            throw;
        }
    }
}
