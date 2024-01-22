using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService
{
    public async Task<LunchRegistrationsItem> GetLunchIsRegisteredToday(string microsoftId)
    {
        IsDbSetNull("LunchRegistrations");

        var lunchRegistrationsItem = await LunchRegistrations.FindAsync(microsoftId);

        if (lunchRegistrationsItem == null)
        {
            throw new KeyNotFoundException();
        }

        return lunchRegistrationsItem;
    }

    public async Task PutLunchRegistrationItem(LunchRegistrationsItem lunchRegistrationsItem)
    {
        IsDbSetNull("LunchRegistrations");

        if (!LunchRegistrationsItemExists(lunchRegistrationsItem.MicrosoftId))
        {
            throw new KeyNotFoundException();
        }

        Entry(lunchRegistrationsItem).State = EntityState.Modified;

        await SaveChangesAsync();
    }

    public async Task PostLunchRegistrationItem(LunchRegistrationsItem lunchRegistrationsItem)
    {
        IsDbSetNull("LunchRegistrations");

        LunchRegistrations.Add(lunchRegistrationsItem);

        await SaveChangesAsync();
    }

    private bool LunchRegistrationsItemExists(string microsoftId)
    {
        return (LunchRegistrations?.Any(e => e.MicrosoftId == microsoftId)).GetValueOrDefault();
    }
}