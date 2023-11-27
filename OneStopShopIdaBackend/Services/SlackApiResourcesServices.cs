using System.Net.Http.Headers;
using System.Text.Json;

namespace OneStopShopIdaBackend.Services;
public partial class SlackApiServices
{
    public async Task<HttpResponseMessage> SendMessage(string slackAccessToken, string message, string channel)
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

        using (HttpResponseMessage response = await _httpClient.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            return response;
        }
    }
    public async Task<HttpResponseMessage> SetStatus(string slackAccessToken, string text = "", string emoji = "", string expiration = "0")
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

        using (HttpResponseMessage response = await _httpClient.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            return response;
        }
    }
}
