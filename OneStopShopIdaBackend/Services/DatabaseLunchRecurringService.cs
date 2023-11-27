using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService
{
    public async Task<LunchRecurringItem> GetRegisteredDays(string microsoftId)
    {
        IsDbSetNull("LunchRecurring");

        var lunchRecurringItem = await LunchRecurring.FindAsync(microsoftId);

        if (lunchRecurringItem == null)
        {
            throw new KeyNotFoundException();
        }

        return lunchRecurringItem;
    }

    public async Task PutLunchRecurringItem(LunchRecurringItem lunchRecurringItem)
    {
        IsDbSetNull("LunchRecurring");

        if (!LunchRecurringItemExists(lunchRecurringItem.MicrosoftId))
        {
            throw new KeyNotFoundException();
        }

        Entry(lunchRecurringItem).State = EntityState.Modified;

        await SaveChangesAsync();
    }

    public async Task PostLunchRecurringItem(LunchRecurringItem lunchRecurringItem)
    {
        IsDbSetNull("LunchRecurring");

        LunchRecurring.Add(lunchRecurringItem);

        await SaveChangesAsync();
    }

    private bool LunchRecurringItemExists(string microsoftId)
    {
        return (LunchRecurring?.Any(e => e.MicrosoftId == microsoftId)).GetValueOrDefault();
    }
}