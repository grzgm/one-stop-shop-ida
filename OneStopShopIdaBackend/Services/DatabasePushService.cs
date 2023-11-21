using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;
using WebPush;
using PushSubscription = OneStopShopIdaBackend.Models.PushSubscription;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService
{
    public string GetVapidPublicKey() => _vapidDetails.PublicKey;

    public async Task<bool> IsSubscribe(string microsoftId)
    {

        try
        {
            var subscription = await GetUserSubscription(microsoftId);
            if (subscription == null)
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    public async Task<PushSubscription> Subscribe(PushSubscription subscription, string microsoftId)
    {
        if (await PushSubscription.AnyAsync(s => s.P256Dh == subscription.P256Dh))
            return await PushSubscription.FindAsync(subscription.P256Dh);

        var existingSubscription = await GetUserSubscription(microsoftId);
        if (existingSubscription != null)
        {
            PushSubscription.Remove(existingSubscription);
        }

        await PushSubscription.AddAsync(subscription);
        try
        {
            await SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return subscription;
    }

    public async Task Unsubscribe(PushSubscription subscription)
    {
        if (!await PushSubscription.AnyAsync(s => s.P256Dh == subscription.P256Dh)) return;

        PushSubscription.Remove(subscription);
        await SaveChangesAsync();
    }

    //public async Task Send(string microsoftId, Notification notification)
    //{
    //    WebPushClient webPushClient = new();
    //    foreach (var subscription in await GetUserSubscriptions(microsoftId))
    //        try
    //        {
    //            webPushClient.SendNotification(subscription.ToWebPushSubscription(),
    //                JsonConvert.SerializeObject(notification), _vapidDetails);
    //        }
    //        catch (WebPushException e)
    //        {
    //            if (e.Message == "Subscription no longer valid")
    //            {
    //                PushSubscription.Remove(subscription);
    //                await SaveChangesAsync();
    //            }
    //            else
    //            {
    //                // Track exception with eg. AppInsights
    //            }
    //        }
    //}

    //public async Task Send(string microsoftId, string text)
    //{
    //    await Send(microsoftId, new Notification(text));
    //}

    public async Task SendNotificationsToUsers(Notification notification, List<UserItem> userItems)
    {
        WebPushClient webPushClient = new();
        foreach (var userItem in userItems)
        {
            try
            {
                var subscription = await GetUserSubscription(userItem.MicrosoftId);
                if (subscription != null)
                {
                    await webPushClient.SendNotificationAsync(subscription.ToWebPushSubscription(),
                        JsonConvert.SerializeObject(notification), _vapidDetails);
                }
            }
            catch (WebPushException ex)
            {
                if (ex.Message == "Subscription no longer valid")
                {
                    var subscription = await GetUserSubscription(userItem.MicrosoftId);
                    PushSubscription.Remove(subscription);
                    await SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{this.GetType().Name}\nError calling external API: {ex.Message}");
            }
        }
    }

    private async Task<List<PushSubscription>> GetUserSubscriptions(string microsoftId) =>
        await PushSubscription.Where(s => s.MicrosoftId == microsoftId).ToListAsync();

    private async Task<PushSubscription?> GetUserSubscription(string microsoftId) =>
        await PushSubscription.FirstOrDefaultAsync(s => s.MicrosoftId == microsoftId);
}