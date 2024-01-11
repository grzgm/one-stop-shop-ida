using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services;

public interface IDatabaseService
{
    DbSet<UserItem> Users { get; set; }
    DbSet<LunchDaysItem> LunchDays { get; set; }
    DbSet<LunchRegistrationsItem> LunchRegistrations { get; set; }
    DbSet<PushSubscription> PushSubscription { get; set; }
    DbSet<OfficeDeskLayoutsItem> OfficeDeskLayouts { get; set; }
    DbSet<DeskReservationItem> DeskReservation { get; set; }
    void CheckOrGenerateVapidDetails(string vapidSubject, string vapidPublicKey, string vapidPrivateKey);
    Task<IEnumerable<UserItem>> GetUserItems();
    Task<UserItem> GetUserItem(string microsoftId);
    Task PutUserItem(UserItem userItem);
    Task PostUserItem(UserItem userItem);
    Task DeleteUserItem(string microsoftId);
    bool UserItemExists(string microsoftId);
    Task<LunchRegistrationsItem> GetLunchIsRegisteredToday(string microsoftId);
    Task PutLunchRegistrationItem(LunchRegistrationsItem lunchRegistrationsItem);
    Task PostLunchRegistrationItem(LunchRegistrationsItem lunchRegistrationsItem);
    Task<List<OfficeDeskLayoutsItem>> GetOfficeDeskLayoutForOffice(string office);
    Task<LunchDaysItem> GetRegisteredDays(string microsoftId);
    Task PutLunchDaysItem(LunchDaysItem lunchDaysItem);
    Task PostLunchDaysItem(LunchDaysItem lunchDaysItem);
    string GetVapidPublicKey();
    Task<bool> IsSubscribe(string microsoftId);
    Task<PushSubscription> Subscribe(PushSubscription subscription, string microsoftId);
    Task Unsubscribe(PushSubscription subscription);
    Task SendNotificationsToUsers(Notification notification, List<UserItem> userItems);
    Task SendNotificationsToUser(Notification notification, UserItem userItem);

    Task<List<DeskReservationItem>> GetDeskReservationForOfficeDate(string office, DateTime startDate,
        DateTime endDate);

    Task<List<DeskReservationItem>> GetDeskReservationsOfUser(string microsoftId, string office,
        DateTime startDate, DateTime endDate);

    Task PutDeskReservation(DeskReservationItem deskReservationItem);
    Task PostDeskReservations(List<DeskReservationItem> deskReservationItems);
    Task DeleteDeskReservations(List<DeskReservationItem> deskReservationItems);

    Task<bool> AreDeskResservationTimeslotsDifferent(string microsoftId, string office, DateTime startDate,
        DateTime endDate, List<DeskReservationItem> newDeskReservationItems);
}