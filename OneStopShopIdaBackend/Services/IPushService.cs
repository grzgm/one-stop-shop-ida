using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services;

public interface IPushService
{
    void CheckOrGenerateVapidDetails(string subject, string vapidPublicKey, string vapidPrivateKey);
    string GetVapidPublicKey();
    Task<PushSubscription> Subscribe(PushSubscription subscription);
    Task Unsubscribe(PushSubscription subscription);
    Task Send(string userId, string text);
    Task Send(string userId, Notification notification);
}