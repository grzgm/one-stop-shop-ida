using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService
{
    public async Task<bool> GetLunchTodayIsRegistered(string microsoftId)
    {
        IsDbSetNull("LunchToday");

        var lunchTodayItem = await LunchToday.FindAsync(microsoftId);

        if (lunchTodayItem == null)
        {
            throw new KeyNotFoundException();
        }

        return lunchTodayItem.IsRegistered;
    }

    public async Task PutLunchTodayRegister(LunchTodayItem lunchTodayItem)
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
        try
        {
            await SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (LunchTodayItemExists(lunchTodayItem.MicrosoftId))
            {
                throw new DbUpdateException("Item with that Id already exists");
            }
            else
            {
                throw;
            }
        }
    }

    public async Task UpdateAllLunchTodayItems(bool isRegistered)
    {
        IsDbSetNull("LunchToday");

        var lunchTodayItems = await LunchToday.ToListAsync();

        foreach (var lunchTodayItem in lunchTodayItems)
        {
            lunchTodayItem.IsRegistered = isRegistered;
            Entry(lunchTodayItem).State = EntityState.Modified;
        }

        try
        {
            await SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
    }

    private bool LunchTodayItemExists(string microsoftId)
    {
        return (LunchToday?.Any(e => e.MicrosoftId == microsoftId)).GetValueOrDefault();
    }
}