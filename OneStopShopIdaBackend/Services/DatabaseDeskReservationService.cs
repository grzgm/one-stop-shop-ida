using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService
{
    public async Task<List<DeskReservationItem>> GetDeskReservationForOfficeDate(string office, DateTime date)
    {
        IsDbSetNull("DeskReservation");

        var deskReservationItem = await DeskReservation.Where(e=> e.Office == office && e.Date == date).ToListAsync();

        if (deskReservationItem == null)
        {
            throw new KeyNotFoundException();
        }

        return deskReservationItem;
    }

    public async Task<List<DeskReservationItem>> GetDeskReservationForUser(string microsoftId)
    {
        IsDbSetNull("DeskReservation");

        var deskReservationItem = await DeskReservation.Where(e => e.MicrosoftId == microsoftId).ToListAsync();

        return deskReservationItem;
    }

    public async Task PutDeskReservation(DeskReservationItem deskReservationItem)
    {
        IsDbSetNull("DeskReservation");

        if (!DeskReservationItemExists(deskReservationItem.MicrosoftId))
        {
            throw new KeyNotFoundException();
        }

        Entry(deskReservationItem).State = EntityState.Modified;

        await SaveChangesAsync();
    }

    public async Task PostDeskReservation(DeskReservationItem deskReservationItem)
    {
        IsDbSetNull("DeskReservation");

        DeskReservation.Add(deskReservationItem);

        await SaveChangesAsync();
    }

    public async Task DeleteDeskReservation(DeskReservationItem deskReservationItem)
    {
        IsDbSetNull("DeskReservation");

        DeskReservation.Remove(deskReservationItem);

        await SaveChangesAsync();
    }

    private bool DeskReservationItemExists(string microsoftId)
    {
        return (DeskReservation?.Any(e => e.MicrosoftId == microsoftId)).GetValueOrDefault();
    }
}