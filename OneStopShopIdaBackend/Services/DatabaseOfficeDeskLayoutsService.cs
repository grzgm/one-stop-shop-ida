using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService
{
    public async Task<List<OfficeDeskLayoutsItem>> GetOfficeDeskLayoutForOffice(string office)
    {
        IsDbSetNull("DeskReservation");

        var officeDeskLayoutsItem = await OfficeDeskLayouts.Where(e=> e.Office == office).ToListAsync();

        if (officeDeskLayoutsItem == null)
        {
            throw new KeyNotFoundException();
        }

        return officeDeskLayoutsItem;
    }

    private bool OfficeDeskLayoutsItemExists(string microsoftId)
    {
        return (DeskReservation?.Any(e => e.MicrosoftId == microsoftId)).GetValueOrDefault();
    }
}