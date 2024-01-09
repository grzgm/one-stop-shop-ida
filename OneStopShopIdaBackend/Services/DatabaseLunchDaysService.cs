using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService
{
    public async Task<LunchDaysItem> GetRegisteredDays(string microsoftId)
    {
        IsDbSetNull("LunchDays");

        var lunchDaysItem = await LunchDays.FindAsync(microsoftId);

        if (lunchDaysItem == null)
        {
            throw new KeyNotFoundException();
        }

        return lunchDaysItem;
    }

    public async Task PutLunchDaysItem(LunchDaysItem lunchDaysItem)
    {
        IsDbSetNull("LunchDays");

        if (!LunchDaysItemExists(lunchDaysItem.MicrosoftId))
        {
            throw new KeyNotFoundException();
        }

        Entry(lunchDaysItem).State = EntityState.Modified;

        await SaveChangesAsync();
    }

    public async Task PostLunchDaysItem(LunchDaysItem lunchDaysItem)
    {
        IsDbSetNull("LunchDays");

        LunchDays.Add(lunchDaysItem);

        await SaveChangesAsync();
    }

    private bool LunchDaysItemExists(string microsoftId)
    {
        return (LunchDays?.Any(e => e.MicrosoftId == microsoftId)).GetValueOrDefault();
    }
}