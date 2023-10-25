using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OneStopShopIdaBackend.Controllers
{
    public partial class SlackAPIController : ControllerBase
    {
        // OAuth Step 1: Redirect users to Slack's authorization URL
        [HttpPost("send-message")]
        public async Task<IActionResult> PostSendMessage([FromQuery] string message, [FromQuery] string channel)
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
                    new {
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
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error calling external API: {ex.Message}");
                return StatusCode(500, $"Internal Server Error \n {ex.Message}");
            }
        }

        // OAuth Step 2: Handle the OAuth callback
        [HttpPut("set-status")]
        public async Task<IActionResult> PutSetStatus([FromQuery] string code, [FromQuery] string state)
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

                // Store accessToken and refreshToken in the session
                HttpContext.Session.SetString("slackAccessToken", slackAccessToken);

                return Redirect(FrontendUri + state);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error calling external API: {ex.Message}");
                return Redirect(FrontendUri + $"/slack-auth?serverResponse={JsonSerializer.Serialize(StatusCode(500, $"Internal Server Error \n {ex.Message}"))}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling external API: {ex.Message}");
                return Redirect(FrontendUri + $"/slack-auth?serverResponse={JsonSerializer.Serialize(StatusCode(500, $"Internal Server Error \n {ex.Message}"))}");
            }
        }
    }
}