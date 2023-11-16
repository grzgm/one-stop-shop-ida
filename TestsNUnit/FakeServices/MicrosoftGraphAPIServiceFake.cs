using Microsoft.Extensions.Logging;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace TestsNUnit.FakeServices;
public class MicrosoftGraphApiServiceFake : IMicrosoftGraphApiService
{
    public MicrosoftGraphApiServiceFake()
    {
    }

    public async Task<(string, string)> CallAuthCallback(string code, string state)
    {
        return ("test", "test");
    }

    public Task<(string, string)> CallAuthRefresh(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<HttpResponseMessage> CreateEvent(string accessToken, string address, string title, string startDate, string endDate, string description)
    {
        throw new NotImplementedException();
    }

    public string GenerateMicrosoftGraphAPIAuthUrl(string route)
    {
        throw new NotImplementedException();
    }

    public async Task<UserItem> GetMe(string accessToken)
    {
        return new UserItem() { MicrosoftId = "test", FirstName = "test", Surname = "test", Email = "test" };
    }

    public Task<HttpResponseMessage> RegisterLunchRecurring(string accessToken, string message)
    {
        throw new NotImplementedException();
    }

    public Task<HttpResponseMessage> RegisterLunchToday(string accessToken, string microsoftId, string message)
    {
        throw new NotImplementedException();
    }

    public Task<HttpResponseMessage> SendEmail(string accessToken, string message, string address)
    {
        throw new NotImplementedException();
    }
}
