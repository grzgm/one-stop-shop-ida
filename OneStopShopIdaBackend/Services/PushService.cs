using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;
using WebPush;
using PushSubscription = OneStopShopIdaBackend.Models.PushSubscription;

namespace OneStopShopIdaBackend.Services;

public class PushService : IPushService
{
    private readonly WebPushClient _client;
    private readonly DatabaseService _context;
    private readonly VapidDetails _vapidDetails;

    public PushService(DatabaseService context, string vapidSubject, string vapidPublicKey, string vapidPrivateKey)
    {
        _context = context;
        _client = new WebPushClient();

        CheckOrGenerateVapidDetails(vapidSubject, vapidPublicKey, vapidPrivateKey);

        _vapidDetails = new VapidDetails(vapidSubject, vapidPublicKey, vapidPrivateKey);
    }

    public PushService(DatabaseService context, IConfiguration configuration)
    {
        _context = context;
        _client = new WebPushClient();

        var vapidSubject = configuration.GetValue<string>("Vapid:Subject");
        var vapidPublicKey = configuration.GetValue<string>("Vapid:PublicKey");
        var vapidPrivateKey = configuration.GetValue<string>("Vapid:PrivateKey");

        CheckOrGenerateVapidDetails(vapidSubject, vapidPublicKey, vapidPrivateKey);

        _vapidDetails = new VapidDetails(vapidSubject, vapidPublicKey, vapidPrivateKey);
    }

    public void CheckOrGenerateVapidDetails(string vapidSubject, string vapidPublicKey, string vapidPrivateKey)
    {
        if (string.IsNullOrEmpty(vapidSubject) ||
            string.IsNullOrEmpty(vapidPublicKey) ||
            string.IsNullOrEmpty(vapidPrivateKey))
        {
            var vapidKeys = VapidHelper.GenerateVapidKeys();

            // Prints 2 URL Safe Base64 Encoded Strings
            Console.WriteLine($"Public {vapidKeys.PublicKey}");
            Console.WriteLine($"Private {vapidKeys.PrivateKey}");

            throw new Exception(
                "You must set the Vapid:Subject, Vapid:PublicKey and Vapid:PrivateKey application settings or pass them to the service in the constructor. You can use the ones just printed to the debug console.");
        }
    }

    public string GetVapidPublicKey() => _vapidDetails.PublicKey;

    public async Task<PushSubscription> Subscribe(PushSubscription subscription)
    {
        if (await _context.PushSubscription.AnyAsync(s => s.P256Dh == subscription.P256Dh))
            return await _context.PushSubscription.FindAsync(subscription.P256Dh);

        await _context.PushSubscription.AddAsync(subscription);
        try
        {
            await _context.SaveChangesAsync();
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
        if (!await _context.PushSubscription.AnyAsync(s => s.P256Dh == subscription.P256Dh)) return;

        _context.PushSubscription.Remove(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task Send(string userId, Notification notification)
    {
        foreach (var subscription in await GetUserSubscriptions(userId))
            try
            {
                _client.SendNotification(subscription.ToWebPushSubscription(),
                    JsonConvert.SerializeObject(notification), _vapidDetails);
            }
            catch (WebPushException e)
            {
                if (e.Message == "Subscription no longer valid")
                {
                    _context.PushSubscription.Remove(subscription);
                    await _context.SaveChangesAsync();
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
        await _context.PushSubscription.Where(s => s.MicrosoftId == userId).ToListAsync();
}