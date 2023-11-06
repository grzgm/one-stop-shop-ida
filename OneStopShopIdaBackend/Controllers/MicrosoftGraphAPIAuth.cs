using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using OneStopShopIdaBackend.Models;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using System.Text.Json;

namespace OneStopShopIdaBackend.Controllers
{
    public partial class MicrosoftGraphAPIController : ControllerBase
    {
        // OAuth Step 1: Redirect users to microsoft's authorization URL
        [HttpGet("auth")]
        public async Task<IActionResult> GetAuth([FromQuery] string route)
        {
            try
            {
                string authUrl =
                $"https://login.microsoftonline.com/{Tenant}/oauth2/v2.0/authorize?" +
                $"client_id={MicrosoftClientId}" +
                $"&response_type=code" +
                $"&redirect_uri={RedirectUri}" +
                $"&response_mode=query" +
                $"&scope={Scopes}" +
                $"&state={route}" +
                $"&code_challenge={_codeChallengeGeneratorService.CodeChallenge}" +
                $"&code_challenge_method=S256";

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
                var data = new Dictionary<string, string>
                {
                    { "client_id", MicrosoftClientId },
                    { "scope", Scopes },
                    { "code", code },
                    { "redirect_uri", RedirectUri },
                    { "grant_type", "authorization_code" },
                    { "code_verifier", _codeChallengeGeneratorService.CodeVerifier }
                };

                var content = new FormUrlEncodedContent(data);
                content.Headers.Clear();
                content.Headers.Add("content-type", "application/x-www-form-urlencoded");
                content.Headers.Add("Origin", "http://localhost");

                HttpResponseMessage response = await _httpClient.PostAsync("https://login.microsoftonline.com/organizations/oauth2/v2.0/token", content);
                string responseData = await response.Content.ReadAsStringAsync();
                dynamic responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData);

                // Access the access_token property
                string accessToken = responseObject.access_token;
                string refreshToken = responseObject.refresh_token;

                // Store accessToken and refreshToken in the session
                HttpContext.Session.SetString("accessToken", accessToken);
                HttpContext.Session.SetString("refreshToken", refreshToken);

                UserItem user = await this.GetMe();

                if (! await _userItemsController.GetIsUserInDatabase(user.MicrosoftId))
                {
                    await _userItemsController.PostUserItem(user);
                    await _lunchTodayItemsController.PostLunchTodayItem(user.MicrosoftId);
                }

                return Redirect(FrontendUri + state);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error calling external API: {ex.Message}");
                return Redirect(FrontendUri + $"/microsoft-auth?serverResponse={JsonSerializer.Serialize(StatusCode(500, $"Internal Server Error \n {ex.Message}"))}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling external API: {ex.Message}");
                return Redirect(FrontendUri + $"/microsoft-auth?serverResponse={JsonSerializer.Serialize(StatusCode(500, $"Internal Server Error \n {ex.Message}"))}");
            }
        }

        // OAuth Step 2: Handle the OAuth callback
        [HttpPut("auth/refresh")]
        public async Task<IActionResult> GetAuthRefresh()
        {
            try
            {
                var data = new Dictionary<string, string>
                {
                    { "client_id", MicrosoftClientId },
                    { "scope", Scopes },
                    { "refresh_token", HttpContext.Session.GetString("refreshToken") },
                    { "redirect_uri", RedirectUri },
                    { "grant_type", "refresh_token" },
                };

                var content = new FormUrlEncodedContent(data);
                content.Headers.Clear();
                content.Headers.Add("content-type", "application/x-www-form-urlencoded");
                content.Headers.Add("Origin", "http://localhost");

                HttpResponseMessage response = await _httpClient.PostAsync("https://login.microsoftonline.com/organizations/oauth2/v2.0/token", content);
                string responseData = await response.Content.ReadAsStringAsync();
                dynamic responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData);

                // Access the access_token property
                string accessToken = responseObject.access_token;
                string refreshToken = responseObject.refresh_token;


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
        }
        // GET: api/TodoItems
        [HttpGet("auth/check-token")]
        public async Task<ActionResult<Boolean>> GetCheckToken()
        {
            // Check if the accessToken and refreshToken are stored in session
            string accessToken = HttpContext.Session.GetString("accessToken");
            string refreshToken = HttpContext.Session.GetString("refreshToken");
            Boolean isToken = false;
            if (accessToken != null && refreshToken != null)
            {
                isToken = true;
            }

            return Ok(isToken);
        }
        // GET: api/TodoItems
        [HttpGet("auth/get-token")]
        public async Task<ActionResult<Object>> GetGetToken()
        {
            // Get token from session
            return Ok(new { accessToken = HttpContext.Session.GetString("accessToken") });
        }
    }
}
