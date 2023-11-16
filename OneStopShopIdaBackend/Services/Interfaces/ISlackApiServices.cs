namespace OneStopShopIdaBackend.Services
{
    public interface ISlackApiServices
    {
        Task<string> CallAuthCallback(string code, string state);
        string GenerateSlackAPIAuthUrl(string route);
        Task<HttpResponseMessage> SendMessage(string slackAccessToken, string message, string channel);
        Task<HttpResponseMessage> SetStatus(string slackAccessToken, string text = "", string emoji = "", string expiration = "0");
    }
}