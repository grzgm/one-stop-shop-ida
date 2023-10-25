using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;
using System.Diagnostics;
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
                        { "Authorization", $"Bearer {SlackAccessToken}" },
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
        public async Task<IActionResult> PutSetStatus([FromQuery] string text = "", [FromQuery] string emoji = "", [FromQuery] string expiration = "0")
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://slack.com/api/users.profile.set"),
                    Headers =
                    {
                        { "Authorization", $"Bearer {SlackAccessToken}" },
                    },
                    Content = new StringContent(JsonSerializer.Serialize(
                    new { 
                        profile = new {
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
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error calling external API: {ex.Message}");
                return StatusCode(500, $"Internal Server Error \n {ex.Message}");
            }
        }
    }
}