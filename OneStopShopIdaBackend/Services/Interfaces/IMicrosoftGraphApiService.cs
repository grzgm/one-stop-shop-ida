using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services
{
    public interface IMicrosoftGraphApiService
    {
        Task<(string, string)> CallAuthCallback(string code, string state);
        Task<(string, string)> CallAuthRefresh(string refreshToken);
        Task<HttpResponseMessage> CreateEvent(string accessToken, string address, string title, string startDate, string endDate, string description);
        string GenerateMicrosoftGraphAPIAuthUrl(string route);
        Task<UserItem> GetMe(string accessToken);
        Task<HttpResponseMessage> RegisterLunchRecurring(string accessToken, string message);
        Task<HttpResponseMessage> RegisterLunchToday(string accessToken, string microsoftId, string message);
        Task<HttpResponseMessage> SendEmail(string accessToken, string message, string address);
    }
}