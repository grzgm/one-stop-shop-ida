﻿using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;
using System.Text.Json;

namespace OneStopShopIdaBackend.Controllers;

public partial class MicrosoftGraphApiController
{
    // OAuth Step 1: Redirect users to microsoft's authorization URL
    [HttpGet("auth")]
    public async Task<IActionResult> GetAuth([FromQuery] string route)
    {
        return Redirect(_microsoftGraphApiService.GenerateMicrosoftGraphAPIAuthUrl(route));
    }

    // OAuth Step 2: Handle the OAuth callback
    [HttpGet("auth/callback")]
    public async Task<IActionResult> GetAuthCallback([FromQuery] string code, [FromQuery] string? state)
    {
        try
        {
            // Access the access_token property
            (string accessToken, string refreshToken) = await _microsoftGraphApiService.CallAuthCallback(code);

            UserItem user = await _microsoftGraphApiService.GetMe(accessToken);

            if (!_databaseService.UserItemExists(user.MicrosoftId))
            {
                await _databaseService.PostUserItem(user);

                LunchTodayItem lunchTodayItem = new LunchTodayItem()
                {
                    MicrosoftId = user.MicrosoftId,
                    IsRegistered = false
                };
                await _databaseService.PostLunchTodayItem(lunchTodayItem);

                LunchRecurringItem lunchRecurringItem = new()
                {
                    MicrosoftId = user.MicrosoftId,
                    Monday = false,
                    Tuesday = false,
                    Wednesday = false,
                    Thursday = false,
                    Friday = false,
                };
                await _databaseService.PostLunchRecurringItem(lunchRecurringItem);

                LunchRecurringRegistrationItem lunchRecurringRegistrationItem = new()
                {
                    MicrosoftId = user.MicrosoftId,
                    LastRegistered = DateTime.Now
                };
                await _databaseService.PostLunchRecurringRegistrationItem(lunchRecurringRegistrationItem);
            }

            // Store Access Token, Refresh Token, Microsoft Id in the session
            HttpContext.Session.SetString("accessToken", accessToken);
            HttpContext.Session.SetString("refreshToken", refreshToken);

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

    // OAuth Step 2: Handle the OAuth callback
    [HttpGet("auth/refresh")]
    public async Task<IActionResult> GetAuthRefresh()
    {
        // Access the access_token property
        (string accessToken, string refreshToken) =
            await _microsoftGraphApiService.CallAuthRefresh(HttpContext.Session.GetString("refreshToken"));

        // Store accessToken and refreshToken in the session
        HttpContext.Session.SetString("accessToken", accessToken);
        HttpContext.Session.SetString("refreshToken", refreshToken);

        return NoContent();
    }

    [HttpGet("auth/is-auth")]
    public async Task<ActionResult<bool>> GetIsAuth()
    {
        // Is User already authenticated?
        // Can User refresh the Access Token?
        try
        {
            string accessToken = HttpContext.Session.GetString("accessToken");

            await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    [HttpGet("auth/check-token")]
    public async Task<ActionResult<bool>> GetCheckToken()
    {
        // Check if the accessToken and refreshToken are stored in session
        string accessToken = HttpContext.Session.GetString("accessToken");
        string refreshToken = HttpContext.Session.GetString("refreshToken");
        bool isToken = accessToken != null && refreshToken != null;

        return Ok(isToken);
    }
}