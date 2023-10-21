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
        [HttpPost("resources/send-email")]
        public async Task<IActionResult> PostSendEmail([FromQuery] string message, [FromQuery] string address)
        {
            try
            {
                // Create E-mail
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
                                Address = address
                            }
                        }
                    }
                };

                var data = JsonSerializer.Serialize(new { message = email });

                var content = new StringContent(data, Encoding.UTF8, "application/json");
                // Add the Authorization header to the request
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("accessToken"));

                HttpResponseMessage response = await _httpClient.PostAsync("https://graph.microsoft.com/v1.0/me/sendMail", content);
                return StatusCode((int)response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error calling external API: {ex.Message}");
                return StatusCode(500, $"Internal Server Error \n {ex.Message}");
            }
        }

        [HttpPost("resources/create-event")]
        public async Task<IActionResult> PostCreateEvent([FromQuery] string address)
        {
            try
            {
                // Create event
                Event microsoftEvent = new()
                {
                    Subject = "Test event",
                    Body = new Body
                    {
                        ContentType = "text",
                        Content = "Testing if adding events with new participants works"
                    },
                    Start = new Start
                    {
                        DateTime = "2023-10-25T09:00:00",
                        TimeZone = "Europe/Warsaw"
                    },
                    End = new End
                    {
                        DateTime = "2023-10-25T10:00:00",
                        TimeZone = "Europe/Warsaw"
                    },
                    Attendees = new List<Attendee>
                    {
                        new Attendee
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = address
                            },
                            Type = "required"
                        },
                    }
                };

                var data = JsonSerializer.Serialize(microsoftEvent);

                var content = new StringContent(data, Encoding.UTF8, "application/json");
                // Add the Authorization header to the request
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("accessToken"));

                HttpResponseMessage response = await _httpClient.PostAsync("https://graph.microsoft.com/v1.0/me/events", content);
                return StatusCode((int)response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error calling external API: {ex.Message}");
                return StatusCode(500, $"Internal Server Error \n {ex.Message}");
            }
        }
    }
}
