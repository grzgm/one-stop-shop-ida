﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace OneStopShopIdaBackend.Controllers;
public partial class SlackApiController : ControllerBase
{
    // OAuth Step 1: Redirect users to Slack's authorization URL
    [Authorize]
    [HttpGet("auth/url")]
    public async Task<ActionResult<string>> GetAuth()
    {
        return _slackApiServices.GenerateSlackAPIAuthUrl(User.FindFirst("UserId").Value);
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
            
            return Redirect(FrontendUri + $"/popup-login?serverResponse={JsonSerializer.Serialize(StatusCode(200))}");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"{GetType().Name}\nError calling external API: {ex.Message}");
            return Redirect(FrontendUri +
                            $"/popup-login?serverResponse={JsonSerializer.Serialize(StatusCode(500))}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{GetType().Name}\nError: {ex.Message}");
            return Redirect(FrontendUri +
                            $"/popup-login?serverResponse={JsonSerializer.Serialize(StatusCode(500))}");
        }
    }

    [Authorize]
    [HttpGet("auth/is-auth")]
    public async Task<ActionResult<bool>> GetIsAuth()
    {
        try
        {
            string slackAccessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}SlackAccessToken");
            bool isToken = slackAccessToken != null;

            return isToken;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}