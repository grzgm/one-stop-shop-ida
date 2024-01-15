using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace OneStopShopIdaBackend.Controllers;
public partial class SlackApiController
{
    // OAuth Step 1: Redirect users to Slack's authorization URL
    [Authorize]
    [HttpGet("auth/url")]
    public Task<ActionResult<string>> GetAuth()
    {
        return Task.FromResult<ActionResult<string>>(_slackApiServices.GenerateSlackAPIAuthUrl(User.FindFirst("UserId")?.Value ?? string.Empty));
    }

    // OAuth Step 2: Handle the OAuth callback
    [HttpGet("auth/callback")]
    public async Task<IActionResult> GetAuthCallback([FromQuery] string code, [FromQuery] string state)
    {
        try
        {
            // Access the access_token property
            string slackAccessToken = await _slackApiServices.CallAuthCallback(code, state);

            // Store accessToken in the memory cache
            _memoryCache.Set($"{state}SlackAccessToken", slackAccessToken, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) // Adjust expiration as needed
            });
            
            return Redirect(_frontendUri + $"/popup-login?serverResponse={JsonSerializer.Serialize(StatusCode(200))}");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"{GetType().Name}\nError calling external API: {ex.Message}");
            return Redirect(_frontendUri +
                            $"/popup-login?serverResponse={JsonSerializer.Serialize(StatusCode(500))}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{GetType().Name}\nError: {ex.Message}");
            return Redirect(_frontendUri +
                            $"/popup-login?serverResponse={JsonSerializer.Serialize(StatusCode(500))}");
        }
    }

    [Authorize]
    [HttpGet("auth/is-auth")]
    public Task<ActionResult<bool>> GetIsAuth()
    {
        try
        {
            string slackAccessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId")?.Value}SlackAccessToken") ?? string.Empty;
            bool isToken = slackAccessToken != string.Empty;

            return Task.FromResult<ActionResult<bool>>(isToken);
        }
        catch (Exception)
        {
            return Task.FromResult<ActionResult<bool>>(false);
        }
    }
}
