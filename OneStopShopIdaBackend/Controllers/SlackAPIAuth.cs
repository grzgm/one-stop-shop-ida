using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Services;
using System.Text.Json;
using System.Web;

namespace OneStopShopIdaBackend.Controllers
{
    public partial class SlackAPIController : ControllerBase
    {
        // OAuth Step 1: Redirect users to Slack's authorization URL
        [HttpGet("auth")]
        public async Task<IActionResult> GetAuth([FromQuery] string route)
        {
            try
            {
                string authUrl =
                $"https://slack.com/oauth/v2/authorize?client_id={SlackClientId}&&scope=&user_scope={Scopes}&redirect_uri={RedirectUri}&state={route}";

                return Redirect(authUrl);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error calling external API: {ex.Message}");
                return StatusCode(500, $"Internal Server Error \n {ex.Message}");
            }
        }

        // OAuth Step 2: Handle the OAuth callback
        [HttpGet("auth/callback")]
        public async Task<IActionResult> GetAuthCallback([FromQuery] string code, [FromQuery] string state)
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