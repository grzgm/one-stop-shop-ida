using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace OneStopShopIdaBackend.Controllers;
public partial class SlackApiController : ControllerBase
{
    // OAuth Step 1: Redirect users to Slack's authorization URL
    [HttpGet("auth")]
    public async Task<IActionResult> GetAuth([FromQuery] string route)
    {
        return Redirect(_slackApiServices.GenerateSlackAPIAuthUrl(route));
    }

    // OAuth Step 2: Handle the OAuth callback
    [HttpGet("auth/callback")]
    public async Task<IActionResult> GetAuthCallback([FromQuery] string code, [FromQuery] string state)
    {
        try
        {
            // Access the access_token property
            string slackAccessToken = await _slackApiServices.CallAuthCallback(code, state);

            // Store accessToken and refreshToken in the session
            HttpContext.Session.SetString("slackAccessToken", slackAccessToken);

            return Redirect(FrontendUri + state);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"{GetType().Name}\nError calling external API: {ex.StatusCode} {ex.Message}");
            return StatusCode((int)ex.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{this.GetType().Name}\nError calling external API: {ex.Message}");
            return Redirect(FrontendUri + $"/slack-auth?serverResponse={JsonSerializer.Serialize(StatusCode(500))}");
        }
    }

    // GET: api/TodoItems
    [HttpGet("auth/check-token")]
    public async Task<ActionResult<Boolean>> GetCheckToken()
    {
        // Check if the accessToken and refreshToken are stored in session
        string accessToken = HttpContext.Session.GetString("slackAccessToken");
        bool isToken = accessToken != null;

        return Ok(isToken);
    }
}
