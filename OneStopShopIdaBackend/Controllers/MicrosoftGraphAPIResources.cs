using Microsoft.AspNetCore.Mvc;
using OneStopShopIdaBackend.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OneStopShopIdaBackend.Controllers
{
    public partial class MicrosoftGraphAPIController : ControllerBase
    {
        // OAuth Step 1: Redirect users to microsoft's authorization URL
        [HttpGet("resources/send-email")]
        public async Task<IActionResult> PostSendEmail([FromQuery] string message, [FromQuery] string address)
        {
            try
            {
                Email email = new()
                {
                    Body = new Body
                    {
                        Content = message,
                        ContentType = "Text"
                    },
                    Subject = "Local application test",
                    ToRecipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = "grzegorz.malisz@weareida.digital"
                            }
                        }
                    }
                };

                var data = JsonSerializer.Serialize(new {message = message});

                var content = new StringContent(data, Encoding.UTF8, "application/json");
                // Add the Authorization header to the request
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("accessToken"));

                HttpResponseMessage response = await _httpClient.PostAsync("https://graph.microsoft.com/v1.0/me/sendMail", content);
                string responseData = await response.Content.ReadAsStringAsync();
                dynamic responseObject = JsonSerializer.Deserialize<Object>(responseData);
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error calling external API: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
