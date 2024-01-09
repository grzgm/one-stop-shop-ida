using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services
{
    public interface IDatabaseService
    {
        DbSet<LunchDaysItem> LunchDays { get; set; }
        DbSet<LunchRegistrationsItem> LunchRegistrations { get; set; }
        DbSet<PushSubscription> PushSubscription { get; set; }
        DbSet<UserItem> Users { get; set; }

        void CheckOrGenerateVapidDetails(string vapidSubject, string vapidPublicKey, string vapidPrivateKey);
        Task DeleteUserItem(string microsoftId);
        Task<LunchRegistrationsItem> GetLunchIsRegisteredToday(string microsoftId);
        Task<LunchDaysItem> GetRegisteredDays(string microsoftId);
        Task<UserItem> GetUserItem(string microsoftId);
        Task<IEnumerable<UserItem>> GetUserItems();
        string GetVapidPublicKey();
        Task<bool> IsSubscribe(string microsoftId);
        Task PostLunchDaysItem(LunchDaysItem lunchDaysItem);
        Task PostLunchRegistrationItem(LunchRegistrationsItem lunchRegistrationsItem);
        Task PostUserItem(UserItem userItem);
        Task PutLunchDaysItem(LunchDaysItem lunchDaysItem);
        Task PutLunchRegistrationItem(LunchRegistrationsItem lunchRegistrationsItem);
        Task PutUserItem(UserItem userItem);
        Task SendNotificationsToUsers(Notification notification, List<UserItem> userItems);
        Task<PushSubscription> Subscribe(PushSubscription subscription, string microsoftId);
        Task Unsubscribe(PushSubscription subscription);
        bool UserItemExists(string microsoftId);
    }
}