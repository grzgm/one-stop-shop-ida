using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService
{
    public async Task<OfficeFeaturesItem> GetOfficeFeaturesItem(string officeName)
    {
        IsDbSetNull("OfficeFeatures");
        IsDbSetNull("OfficeInformation");
        IsDbSetNull("OfficeCoordinates");

        var officeFeaturesItem = await OfficeFeatures.FindAsync(officeName);
        var officeInformationItem = await OfficeInformation.FindAsync(officeName);
        var officeCoordinatesItem = await OfficeCoordinates.FindAsync(officeName);

        if (officeFeaturesItem == null)
        {
            throw new KeyNotFoundException();
        }

        if (officeInformationItem != null)
        {
            officeInformationItem.OfficeCoordinates = officeCoordinatesItem;
            officeFeaturesItem.OfficeInformation = officeInformationItem;
        }

        return officeFeaturesItem;
    }

    public async Task<List<OfficeFeaturesItem>> GetAllOfficeFeaturesItem()
    {
        IsDbSetNull("OfficeFeatures");
        IsDbSetNull("OfficeInformation");
        IsDbSetNull("OfficeCoordinates");

        var officeFeaturesItems = await OfficeFeatures.ToListAsync();
        foreach(var item in officeFeaturesItems)
        {
            var officeInformationItem = await OfficeInformation.FindAsync(item.OfficeName);
            var officeCoordinatesItem = await OfficeCoordinates.FindAsync(item.OfficeName);

            if (officeInformationItem != null)
            {
                officeInformationItem.OfficeCoordinates = officeCoordinatesItem;
                item.OfficeInformation = officeInformationItem;
            }
        }

        return officeFeaturesItems;
    }
}