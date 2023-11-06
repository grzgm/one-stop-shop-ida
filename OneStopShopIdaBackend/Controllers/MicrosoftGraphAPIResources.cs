using Azure.Core;
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
                        ContentType = "Text",
                    },
                    Subject = "Local application test",
                    ToRecipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = address,
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
        [HttpPost("resources/register-lunch-today")]
        public async Task<IActionResult> PostRegisterLunchToday([FromQuery] string message)
        {
            try
            {
                // Create E-mail
                Email email = new()
                {
                    Body = new Body
                    {
                        Content = message,
                        ContentType = "Text",
                    },
                    Subject = "Local application test",
                    ToRecipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = LunchEmailAddress,
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
        public async Task<IActionResult> PostCreateEvent([FromQuery] string address, [FromQuery] string title, [FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string description)
        {
            try
            {
                // Create event
                Event microsoftEvent = new()
                {
                    Subject = title,
                    Body = new Body
                    {
                        ContentType = "text",
                        Content = description,
                    },
                    Start = new Start
                    {
                        DateTime = startDate,
                        TimeZone = "Europe/Warsaw"
                    },
                    End = new End
                    {
                        DateTime = endDate,
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

        [HttpGet("resources/me")]
        public async Task<ActionResult<UserItem>> GetMe()
        {
            try
            {
                UserItem user = new();

                // Add the Authorization header to the request
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("accessToken"));
                HttpResponseMessage response = await _httpClient.GetAsync("https://graph.microsoft.com/v1.0/me");
                string responseData = await response.Content.ReadAsStringAsync();
                dynamic responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData);

                user.MicrosoftId = responseObject.id;
                user.FirstName = responseObject.givenName;
                user.Surname = responseObject.surname;
                user.Email = responseObject.mail;

                await _userItemsController.PostUserItem(user);
                HttpContext.Session.SetString("microsoftId", user.MicrosoftId);

                return user;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error calling external API: {ex.Message}");
                return StatusCode(500, $"Internal Server Error \n {ex.Message}");
            }
        }
    }
}
