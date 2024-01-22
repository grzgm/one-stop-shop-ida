using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace TestsNUnit.FakeServices;
internal class FakeDatabaseService : DbContext, IDatabaseService
{
    public FakeDatabaseService(DbContextOptions<FakeDatabaseService> options): base(options)
    {
    }

    public DbSet<LunchDaysItem> LunchDays { get; set; }
    public DbSet<LunchRegistrationsItem> LunchRegistrations { get; set; }
    public DbSet<PushSubscription> PushSubscription { get; set; }
    public DbSet<OfficeDeskLayoutsItem> OfficeDeskLayouts { get; set; }
    public DbSet<DeskReservationItem> DeskReservation { get; set; }
    public DbSet<UserItem> Users { get; set; }

    public void CheckOrGenerateVapidDetails(string vapidSubject, string vapidPublicKey, string vapidPrivateKey)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserItem(string microsoftId)
    {
        throw new NotImplementedException();
    }

    public Task<LunchRegistrationsItem> GetLunchIsRegisteredToday(string microsoftId)
    {
        throw new NotImplementedException();
    }

    public Task<List<OfficeDeskLayoutsItem>> GetOfficeDeskLayoutForOffice(string office)
    {
        throw new NotImplementedException();
    }

    public async Task<LunchDaysItem> GetRegisteredDays(string microsoftId)
    {
        LunchDaysItem lunchDaysItem = await LunchDays.FindAsync(microsoftId);
        return lunchDaysItem;
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

    public Task PostLunchDaysItem(LunchDaysItem lunchDaysItem)
    {
        LunchDays.Add(lunchDaysItem);
        SaveChanges();
        return Task.CompletedTask;
    }

    public Task PostLunchRegistrationItem(LunchRegistrationsItem lunchRegistrationsItem)
    {
        LunchRegistrations.Add(lunchRegistrationsItem);
        SaveChanges();
        return Task.CompletedTask;
    }

    public Task PostUserItem(UserItem userItem)
    {
        Users.Add(userItem);
        SaveChanges();
        return Task.CompletedTask;
    }

    public Task PutLunchDaysItem(LunchDaysItem lunchDaysItem)
    {
        throw new NotImplementedException();
    }

    public Task PutLunchRegistrationItem(LunchRegistrationsItem lunchRegistrationsItem)
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

    public Task SendNotificationsToUser(Notification notification, UserItem userItem)
    {
        throw new NotImplementedException();
    }

    public Task<List<DeskReservationItem>> GetDeskReservationForOfficeDate(string office, DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public Task<List<DeskReservationItem>> GetDeskReservationsOfUser(string microsoftId, string office, DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public Task PutDeskReservation(DeskReservationItem deskReservationItem)
    {
        throw new NotImplementedException();
    }

    public Task PostDeskReservations(List<DeskReservationItem> deskReservationItems)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDeskReservations(List<DeskReservationItem> deskReservationItems)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AreDeskResservationTimeslotsDifferent(string microsoftId, string office, DateTime startDate, DateTime endDate,
        List<DeskReservationItem> newDeskReservationItems)
    {
        throw new NotImplementedException();
    }

    public Task<List<OfficeFeaturesItem>> GetAllOfficeFeaturesItem()
    {
        throw new NotImplementedException();
    }

    public Task<OfficeFeaturesItem> GetOfficeFeaturesItem(string officeName)
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
