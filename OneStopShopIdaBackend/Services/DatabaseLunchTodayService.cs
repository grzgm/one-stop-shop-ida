using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService
{
    public async Task<LunchTodayItem> GetLunchTodayIsRegistered(string microsoftId)
    {
        IsDbSetNull("LunchToday");

        var lunchTodayItem = await LunchToday.FindAsync(microsoftId);

        if (lunchTodayItem == null)
        {
            throw new KeyNotFoundException();
        }

        return lunchTodayItem;
    }

    public async Task PutLunchTodayRegistration(LunchTodayItem lunchTodayItem)
    {
        IsDbSetNull("LunchToday");

        if (!LunchTodayItemExists(lunchTodayItem.MicrosoftId))
        {
            throw new KeyNotFoundException();
        }

        Entry(lunchTodayItem).State = EntityState.Modified;

        await SaveChangesAsync();
    }

    public async Task PostLunchTodayItem(LunchTodayItem lunchTodayItem)
    {
        IsDbSetNull("LunchToday");

        LunchToday.Add(lunchTodayItem);

        await SaveChangesAsync();
    }

    // public async Task UpdateAllLunchTodayItems(bool isRegistered)
    // {
    //     IsDbSetNull("LunchToday");
    //
    //     var lunchTodayItems = await LunchToday.ToListAsync();
    //
    //     foreach (var lunchTodayItem in lunchTodayItems)
    //     {
    //         lunchTodayItem.RegistrationDate = isRegistered;
    //         Entry(lunchTodayItem).State = EntityState.Modified;
    //     }
    //
    //     await SaveChangesAsync();
    // }

    private bool LunchTodayItemExists(string microsoftId)
    {
        return (LunchToday?.Any(e => e.MicrosoftId == microsoftId)).GetValueOrDefault();
    }
}