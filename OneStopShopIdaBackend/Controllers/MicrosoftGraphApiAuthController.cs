using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace OneStopShopIdaBackend.Controllers;

public partial class MicrosoftGraphApiController
{
    // OAuth Step 1: Redirect users to microsoft's authorization URL
    [Authorize]
    [HttpGet("auth/url")]
    public async Task<ActionResult<string>> GetAuth()
    {
        return _microsoftGraphApiService.GenerateMicrosoftGraphAPIAuthUrl(User.FindFirst("UserId").Value);
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

                LunchRegistrationsItem lunchRegistrationsItem = new LunchRegistrationsItem()
                {
                    MicrosoftId = user.MicrosoftId,
                    RegistrationDate = null,
                    Office = null,
                };
                await _databaseService.PostLunchRegistrationItem(lunchRegistrationsItem);

                LunchDaysItem lunchDaysItem = new()
                {
                    MicrosoftId = user.MicrosoftId,
                    Monday = false,
                    Tuesday = false,
                    Wednesday = false,
                    Thursday = false,
                    Friday = false,
                };
                await _databaseService.PostLunchDaysItem(lunchDaysItem);
            }

            // Store Access Token, Refresh Token in Memory Cache with GUID
            _memoryCache.Set($"{state}AccessToken", accessToken, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) // Adjust expiration as needed
            });
            _memoryCache.Set($"{state}RefreshToken", refreshToken, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) // Adjust expiration as needed
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

    // OAuth Step 2: Handle the OAuth callback
    [Authorize]
    [HttpGet("auth/refresh")]
    public async Task<IActionResult> GetAuthRefresh()
    {
        // Access the access_token property
        (string accessToken, string refreshToken) =
            await _microsoftGraphApiService.CallAuthRefresh(
                _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}RefreshToken"));

        // Store Access Token, Refresh Token in Memory Cache with GUID
        _memoryCache.Set($"{User.FindFirst("UserId").Value}AccessToken", accessToken, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) // Adjust expiration as needed
        });
        _memoryCache.Set($"{User.FindFirst("UserId").Value}RefreshToken", refreshToken, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) // Adjust expiration as needed
        });

        return NoContent();
    }

    [Authorize]
    [HttpGet("auth/is-auth")]
    public async Task<ActionResult<bool>> GetIsAuth()
    {
        // Is User already authenticated?
        // Can User refresh the Access Token?
        try
        {
            string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");

            await ExecuteWithRetryMicrosoftGraphApi(_microsoftGraphApiService.GetMe, accessToken);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    [Authorize]
    [HttpGet("auth/check-token")]
    public async Task<ActionResult<bool>> GetCheckToken()
    {
        // Check if the accessToken and refreshToken are stored in cache
        string accessToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}AccessToken");
        string refreshToken = _memoryCache.Get<string>($"{User.FindFirst("UserId").Value}RefreshToken");
        bool isToken = accessToken != null && refreshToken != null;

        return Ok(isToken);
    }
}