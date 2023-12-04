using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services
{
    public interface IMicrosoftGraphApiService
    {
        Task<(string, string)> CallAuthCallback(string code);
        Task<(string, string)> CallAuthRefresh(string refreshToken);
        Task<HttpResponseMessage> CreateEvent(string accessToken, string address, string title, string startDate, string endDate, string description);
        string GenerateMicrosoftGraphAPIAuthUrl(string route);
        Task<UserItem> GetMe(string accessToken);
        Task<HttpResponseMessage> SendEmail(string accessToken, string address, string subject, string message);
    }
}