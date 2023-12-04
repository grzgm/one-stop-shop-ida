using OneStopShopIdaBackend.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OneStopShopIdaBackend.Services;

public partial class MicrosoftGraphApiService
{
    public async Task<HttpResponseMessage> SendEmail(string accessToken, string address, string subject, string message)
    {
        // Create E-mail
        Email email = new()
        {
            Body = new Body
            {
                Content = message,
                ContentType = "Text",
            },
            Subject = subject,
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
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        HttpResponseMessage response =
            await _httpClient.PostAsync("https://graph.microsoft.com/v1.0/me/sendMail", content);
        response.EnsureSuccessStatusCode();

        return response;
    }

    public async Task<HttpResponseMessage> CreateEvent(string accessToken, string address, string title,
        string startDate, string endDate, string description)
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
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        HttpResponseMessage response =
            await _httpClient.PostAsync("https://graph.microsoft.com/v1.0/me/events", content);
        response.EnsureSuccessStatusCode();

        return response;
    }

    public async Task<UserItem> GetMe(string accessToken)
    {
        UserItem user = new();

        // Add the Authorization header to the request
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        HttpResponseMessage response = await _httpClient.GetAsync("https://graph.microsoft.com/v1.0/me");
        response.EnsureSuccessStatusCode();

        string responseData = await response.Content.ReadAsStringAsync();
        dynamic responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData);

        user.MicrosoftId = responseObject.id;
        user.FirstName = responseObject.givenName;
        user.Surname = responseObject.surname;
        user.Email = responseObject.mail;

        return user;
    }
}