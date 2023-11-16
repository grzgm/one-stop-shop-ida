namespace OneStopShopIdaBackend.Services;
public partial class SlackApiServices
{
    public string GenerateSlackAPIAuthUrl(string route)
    {
        string authUrl =
        $"https://slack.com/oauth/v2/authorize?client_id={SlackClientId}&&scope=&user_scope={Scopes}&redirect_uri={RedirectUri}&state={route}";

        return authUrl;
    }
    public async Task<string> CallAuthCallback(string code, string state)
    {
        try
        {
            var formData = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "code", code },
                    { "client_id", SlackClientId },
                    { "client_secret", SlackClientSecret }
            });

            var response = await _httpClient.PostAsync("https://slack.com/api/oauth.v2.access", formData);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            dynamic responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData);

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
