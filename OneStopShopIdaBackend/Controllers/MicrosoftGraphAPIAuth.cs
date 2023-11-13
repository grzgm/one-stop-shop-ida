﻿using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;
using System.Text.Json;

namespace OneStopShopIdaBackend.Controllers;

public partial class MicrosoftGraphAPIController : ControllerBase
{
    // OAuth Step 1: Redirect users to microsoft's authorization URL
    [HttpGet("auth")]
    public async Task<IActionResult> GetAuth([FromQuery] string route)
    {
        try
        {
            return Redirect(_microsoftGraphApiService.GenerateMicrosoftGraphAPIAuthUrl(route));
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
    }

    // OAuth Step 2: Handle the OAuth callback
    [HttpGet("auth/callback")]
    public async Task<IActionResult> GetAuthCallback([FromQuery] string code, [FromQuery] string state)
    {
        try
        {
            // Access the access_token property
            (string accessToken, string refreshToken) = await _microsoftGraphApiService.CallAuthCallback(code, state);

            UserItem user = await GetMe(accessToken);

            if (!_databaseService.UserItemExists(user.MicrosoftId))
            {
                await _databaseService.PostUserItem(user);
                await _databaseService.PostLunchTodayItem(user.MicrosoftId);
                await _databaseService.PostLunchRecurringItem(user.MicrosoftId);
            }

            // Store Access Token, Refresh Token, Microsoft Id in the session
            HttpContext.Session.SetString("accessToken", accessToken);
            HttpContext.Session.SetString("refreshToken", refreshToken);
            HttpContext.Session.SetString("microsoftId", user.MicrosoftId);

            return Redirect(FrontendUri + state);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            return Redirect(FrontendUri +
                            $"/microsoft-auth?serverResponse={JsonSerializer.Serialize(StatusCode(500, $"Internal Server Error \n {ex.Message}"))}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            return Redirect(FrontendUri +
                            $"/microsoft-auth?serverResponse={JsonSerializer.Serialize(StatusCode(500, $"Internal Server Error \n {ex.Message}"))}");
        }
    }

    // OAuth Step 2: Handle the OAuth callback
    [HttpPut("auth/refresh")]
    public async Task<IActionResult> GetAuthRefresh()
    {
        try
        {
            // Access the access_token property
            (string accessToken, string refreshToken) =
                await _microsoftGraphApiService.CallAuthRefresh(HttpContext.Session.GetString("refreshToken"));

            // Store accessToken and refreshToken in the session
            HttpContext.Session.SetString("accessToken", accessToken);
            HttpContext.Session.SetString("refreshToken", refreshToken);

            return NoContent();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error calling external API: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            return StatusCode(500, $"Internal Server Error \n {ex.Message}");
        }
    }

    // GET: api/TodoItems
    [HttpGet("auth/check-token")]
    public async Task<ActionResult<Boolean>> GetCheckToken()
    {
        // Check if the accessToken and refreshToken are stored in session
        string accessToken = HttpContext.Session.GetString("accessToken");
        string refreshToken = HttpContext.Session.GetString("refreshToken");
        bool isToken = accessToken != null && refreshToken != null;

        return Ok(isToken);
    }

    //// GET: api/TodoItems
    //[HttpGet("auth/get-token")]
    //public async Task<ActionResult<Object>> GetGetToken()
    //{
    //    // Get token from session
    //    return Ok(new { accessToken = HttpContext.Session.GetString("accessToken") });
    //}
}