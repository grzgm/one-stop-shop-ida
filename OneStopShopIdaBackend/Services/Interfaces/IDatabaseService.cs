using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services
{
    public interface IDatabaseService
    {
        DbSet<LunchRecurringItem> LunchRecurring { get; set; }
        DbSet<LunchRecurringRegistrationItem> LunchRecurringRegistration { get; set; }
        DbSet<LunchTodayItem> LunchToday { get; set; }
        DbSet<PushSubscription> PushSubscription { get; set; }
        DbSet<UserItem> Users { get; set; }

        void CheckOrGenerateVapidDetails(string vapidSubject, string vapidPublicKey, string vapidPrivateKey);
        Task DeleteUserItem(string microsoftId);
        Task<DateTime> GetLunchRecurringRegistrationLastRegistered(string microsoftId);
        Task<bool> GetLunchTodayIsRegistered(string microsoftId);
        Task<LunchRecurringItem> GetRegisteredDays(string microsoftId);
        Task<UserItem> GetUserItem(string microsoftId);
        Task<IEnumerable<UserItem>> GetUserItems();
        string GetVapidPublicKey();
        Task<bool> IsSubscribe(string microsoftId);
        Task PostLunchRecurringItem(string microsoftId);
        Task PostLunchRecurringRegistrationItem(string microsoftId);
        Task PostLunchTodayItem(string microsoftId);
        Task PostUserItem(UserItem userItem);
        Task PutLunchRecurringItem(LunchRecurringItem lunchRecurringItem);
        Task PutLunchRecurringRegistrationItem(LunchRecurringRegistrationItem lunchRecurringRegistrationItem);
        Task PutLunchTodayRegister(LunchTodayItem lunchTodayItem);
        Task PutUserItem(UserItem userItem);
        Task SendNotificationsToUsers(Notification notification, List<UserItem> userItems);
        Task<PushSubscription> Subscribe(PushSubscription subscription, string microsoftId);
        Task Unsubscribe(PushSubscription subscription);
        Task UpdateAllLunchRecurringRegistrationItems(DateTime lastRegistered);
        Task UpdateAllLunchTodayItems(bool isRegistered);
        bool UserItemExists(string microsoftId);
    }
}