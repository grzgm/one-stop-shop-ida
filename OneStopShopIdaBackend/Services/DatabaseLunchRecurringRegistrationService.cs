using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService
{
    public async Task<bool> GetLunchRecurringRegistrationIsRegistered(string microsoftId)
    {
        IsDbSetNull("LunchRecurringRegistration");

        var lunchRecurringRegistrationItem = await LunchRecurringRegistration.FindAsync(microsoftId);

        if (lunchRecurringRegistrationItem == null)
        {
            throw new KeyNotFoundException();
        }

        return lunchRecurringRegistrationItem.IsRegistered;
    }

    public async Task PutLunchRecurringRegistrationItem(LunchRecurringRegistrationItem lunchRecurringRegistrationItem)
    {
        IsDbSetNull("LunchRecurringRegistration");

        if (!LunchRecurringRegistrationItemExists(lunchRecurringRegistrationItem.MicrosoftId))
        {
            throw new KeyNotFoundException();
        }

        Entry(lunchRecurringRegistrationItem).State = EntityState.Modified;

        await SaveChangesAsync();
    }

    public async Task PostLunchRecurringRegistrationItem(string microsoftId)
    {
        IsDbSetNull("LunchRecurringRegistration");

        LunchRecurringRegistrationItem lunchRecurringRegistrationItem = new();
        lunchRecurringRegistrationItem.MicrosoftId = microsoftId;
        lunchRecurringRegistrationItem.IsRegistered = false;

        LunchRecurringRegistration.Add(lunchRecurringRegistrationItem);
        try
        {
            await SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (LunchRecurringRegistrationItemExists(lunchRecurringRegistrationItem.MicrosoftId))
            {
                throw new DbUpdateException("Item with that Id already exists");
            }
            else
            {
                throw;
            }
        }
    }

    public async Task UpdateAllLunchRecurringRegistrationItems(bool isRegistered)
    {
        IsDbSetNull("LunchRecurringRegistration");

        var lunchRecurringRegistrationItems = await LunchRecurringRegistration.ToListAsync();

        foreach (var lunchRecurringRegistrationItem in lunchRecurringRegistrationItems)
        {
            lunchRecurringRegistrationItem.IsRegistered = isRegistered;
            Entry(lunchRecurringRegistrationItem).State = EntityState.Modified;
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

    private bool LunchRecurringRegistrationItemExists(string microsoftId)
    {
        return (LunchRecurringRegistration?.Any(e => e.MicrosoftId == microsoftId)).GetValueOrDefault();
    }
}