using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;
using WebPush;
using PushSubscription = OneStopShopIdaBackend.Models.PushSubscription;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService
{
    public string GetVapidPublicKey() => _vapidDetails.PublicKey;
    
    public async Task<PushSubscription> Subscribe(PushSubscription subscription)
    {
        if (await PushSubscription.AnyAsync(s => s.P256Dh == subscription.P256Dh))
            return await PushSubscription.FindAsync(subscription.P256Dh);

        await PushSubscription.AddAsync(subscription);
        try
        {
            await SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("---------------------------------");
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

    public async Task Send(string userId, Notification notification)
    {
        WebPushClient webPushClient = new();
        foreach (var subscription in await GetUserSubscriptions(userId))
            try
            {
                webPushClient.SendNotification(subscription.ToWebPushSubscription(),
                    JsonConvert.SerializeObject(notification), _vapidDetails);
            }
            catch (WebPushException e)
            {
                if (e.Message == "Subscription no longer valid")
                {
                    PushSubscription.Remove(subscription);
                    await SaveChangesAsync();
                }
                else
                {
                    // Track exception with eg. AppInsights
                }
            }
    }

    public async Task Send(string userId, string text)
    {
        await Send(userId, new Notification(text));
    }

    private async Task<List<PushSubscription>> GetUserSubscriptions(string userId) =>
        await PushSubscription.Where(s => s.MicrosoftId == userId).ToListAsync();
}