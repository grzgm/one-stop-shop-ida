using Microsoft.Extensions.Logging;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace TestsNUnit.FakeServices;
public class MicrosoftGraphApiServiceFake : IMicrosoftGraphApiService
{
    public MicrosoftGraphApiServiceFake()
    {
    }

    public async Task<(string, string)> CallAuthCallback(string code)
    {
        return ("testAccessToken", "testRefreshToken");
    }

    public async Task<(string, string)> CallAuthRefresh(string refreshToken)
    {
        return ("testAccessToken", "testRefreshToken");
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
        return ModelsObjectsFake.testUserItem;
    }

    public Task<HttpResponseMessage> SendEmail(string accessToken, string address, string subject, string message)
    {
        throw new NotImplementedException();
    }
}
