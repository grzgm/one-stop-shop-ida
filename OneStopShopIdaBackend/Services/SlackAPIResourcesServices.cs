using System.Net.Http.Headers;
using System.Text.Json;

namespace OneStopShopIdaBackend.Services;
public partial class SlackAPIServices
{
    public async Task<HttpResponseMessage> SendMessage(string slackAccessToken, string message, string channel)
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://slack.com/api/chat.postMessage"),
                Headers =
                    {
                        { "Authorization", $"Bearer {slackAccessToken}" },
                    },
                Content = new StringContent(JsonSerializer.Serialize(
                new
                {
                    text = message,
                    channel = channel
                }))
                {
                    Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                }
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return response;
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            throw;
        }
    }
    public async Task<HttpResponseMessage> SetStatus(string slackAccessToken, string text = "", string emoji = "", string expiration = "0")
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://slack.com/api/users.profile.set"),
                Headers =
                    {
                        { "Authorization", $"Bearer {slackAccessToken}" },
                    },
                Content = new StringContent(JsonSerializer.Serialize(
                new
                {
                    profile = new
                    {
                        status_text = text,
                        status_emoji = emoji,
                        status_expiration = expiration,
                    }
                }))
                {
                    Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                }
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return response;
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            throw;
        }
    }
}
