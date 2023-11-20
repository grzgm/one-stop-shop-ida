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
using Microsoft.EntityFrameworkCore.Internal;

namespace TestsNUnit.FakeServices;
internal class DatabaseServiceFake : DbContext, IDatabaseService
{
    public DatabaseServiceFake(DbContextOptions<DatabaseServiceFake> options): base(options)
    {
    }

    public DbSet<LunchRecurringItem> LunchRecurring { get; set; }
    public DbSet<LunchRecurringRegistrationItem> LunchRecurringRegistration { get; set; }
    public DbSet<LunchTodayItem> LunchToday { get; set; }
    public DbSet<PushSubscription> PushSubscription { get; set; }
    public DbSet<UserItem> Users { get; set; }

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

    public async Task<LunchRecurringItem> GetRegisteredDays(string microsoftId)
    {
        LunchRecurringItem lunchRecurringItem = await LunchRecurring.FindAsync(microsoftId);
        return lunchRecurringItem;
    }

    public async Task<UserItem> GetUserItem(string microsoftId)
    {
        UserItem userItem = await Users.FindAsync(microsoftId);
        return userItem;
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

    public Task PostLunchRecurringItem(LunchRecurringItem lunchRecurringItem)
    {
        LunchRecurring.Add(lunchRecurringItem);
        SaveChanges();
        return Task.CompletedTask;
    }

    public Task PostLunchRecurringRegistrationItem(LunchRecurringRegistrationItem lunchRecurringRegistrationItem)
    {
        LunchRecurringRegistration.Add(lunchRecurringRegistrationItem);
        SaveChanges();
        return Task.CompletedTask;
    }

    public Task PostLunchTodayItem(LunchTodayItem lunchTodayItem)
    {
        LunchToday.Add(lunchTodayItem);
        SaveChanges();
        return Task.CompletedTask;
    }

    public Task PostUserItem(UserItem userItem)
    {
        Users.Add(userItem);
        SaveChanges();
        return Task.CompletedTask;
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
        return (Users?.Any(e => e.MicrosoftId == microsoftId)).GetValueOrDefault();
    }
}
