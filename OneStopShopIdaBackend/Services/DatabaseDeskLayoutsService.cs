using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService
{
    public async Task<List<OfficeLayoutsItem>> GetOfficeLayoutForOffice(string office)
    {
        IsDbSetNull("DeskReservation");

        var officeLayoutsItem = await OfficeLayouts.Where(e=> e.Office == office).ToListAsync();

        if (officeLayoutsItem == null)
        {
            throw new KeyNotFoundException();
        }

        return officeLayoutsItem;
    }

    private bool OfficeLayoutsItemExists(string microsoftId)
    {
        return (DeskReservation?.Any(e => e.MicrosoftId == microsoftId)).GetValueOrDefault();
    }
}