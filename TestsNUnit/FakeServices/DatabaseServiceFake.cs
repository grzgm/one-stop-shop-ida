using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsNUnit.FakeServices;
internal class DatabaseServiceFake : IDatabaseService
{
    public DatabaseServiceFake()
    {
    }

    public DbSet<LunchRecurringItem> LunchRecurring { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DbSet<LunchRecurringRegistrationItem> LunchRecurringRegistration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DbSet<LunchTodayItem> LunchToday { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DbSet<PushSubscription> PushSubscription { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DbSet<UserItem> Users { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void CheckOrGenerateVapidDetails(string vapidSubject, string vapidPublicKey, string vapidPrivateKey)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserItem(string microsoftId)
    {
        throw new NotImplementedException();
    }

    public Task<DateTime> GetLunchRecurringRegistrationLastRegistered(string microsoftId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> GetLunchTodayIsRegistered(string microsoftId)
    {
        throw new NotImplementedException();
    }

    public Task<LunchRecurringItem> GetRegisteredDays(string microsoftId)
    {
        throw new NotImplementedException();
    }

    public Task<UserItem> GetUserItem(string microsoftId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserItem>> GetUserItems()
    {
        throw new NotImplementedException();
    }

    public string GetVapidPublicKey()
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsSubscribe(string microsoftId)
    {
        throw new NotImplementedException();
    }

    public Task PostLunchRecurringItem(string microsoftId)
    {
        throw new NotImplementedException();
    }

    public Task PostLunchRecurringRegistrationItem(string microsoftId)
    {
        throw new NotImplementedException();
    }

    public Task PostLunchTodayItem(string microsoftId)
    {
        throw new NotImplementedException();
    }

    public Task PostUserItem(UserItem userItem)
    {
        throw new NotImplementedException();
    }

    public Task PutLunchRecurringItem(LunchRecurringItem lunchRecurringItem)
    {
        throw new NotImplementedException();
    }

    public Task PutLunchRecurringRegistrationItem(LunchRecurringRegistrationItem lunchRecurringRegistrationItem)
    {
        throw new NotImplementedException();
    }

    public Task PutLunchTodayRegister(LunchTodayItem lunchTodayItem)
    {
        throw new NotImplementedException();
    }

    public Task PutUserItem(UserItem userItem)
    {
        throw new NotImplementedException();
    }

    public Task SendNotificationsToUsers(Notification notification, List<UserItem> userItems)
    {
        throw new NotImplementedException();
    }

    public Task<PushSubscription> Subscribe(PushSubscription subscription, string microsoftId)
    {
        throw new NotImplementedException();
    }

    public Task Unsubscribe(PushSubscription subscription)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAllLunchRecurringRegistrationItems(DateTime lastRegistered)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAllLunchTodayItems(bool isRegistered)
    {
        throw new NotImplementedException();
    }

    public bool UserItemExists(string microsoftId)
    {
        throw new NotImplementedException();
    }
}
