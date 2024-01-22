using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OneStopShopIdaBackend.Models;

[PrimaryKey(nameof(ClusterId), nameof(DeskId), nameof(IsAvailable))]
public class OfficeDeskLayoutsItem
{
    public OfficeDeskLayoutsItem()
    {
    } 

    public OfficeDeskLayoutsItem(string office, int clusterid, int deskid, int isAvailable)
    {
        Office = office;
        ClusterId = clusterid;
        DeskId = deskid;
        IsAvailable = isAvailable;
    }

    [StringLength(45)] [Required] public string Office { get; set; }
    [Required] public int ClusterId { get; set; }
    [Required] public int DeskId { get; set; }
    [Required] public int IsAvailable { get; set; }
    [Required] public int AmountOfTimeSlots { get; set; }
}