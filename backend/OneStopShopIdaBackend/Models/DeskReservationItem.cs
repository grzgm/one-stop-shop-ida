using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OneStopShopIdaBackend.Models;

[PrimaryKey(nameof(Office), nameof(Date), nameof(ClusterId), nameof(DeskId), nameof(TimeSlot))]
public class DeskReservationItem
{
    public DeskReservationItem() { }

    public DeskReservationItem(string microsoftId, string office, DateTime date, int clusterId, int deskId, int timeslot)
    {
        MicrosoftId = microsoftId;
        Office = office;
        Date = date;
        ClusterId = clusterId;
        DeskId = deskId;
        TimeSlot = timeslot;
    }

    [ForeignKey(nameof(UserItem))]
    [StringLength(255)]
    [Required]
    public string MicrosoftId { get; set; }

    [StringLength(45)]
    [Required]
    public string Office { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public int ClusterId { get; set; }
    [Required]
    public int DeskId { get; set; }
    [Required]
    public int TimeSlot { get; set; }
}

public class DeskFrontend
{
    public int? ClusterId { get; set; }
    public int? DeskId { get; set; }
    public List<bool>? Occupied { get; set; }
    public List<bool>? UserReservations { get; set; }
}

public class DeskClusterFrontend
{
    public int ClusterId { get; set; }

    public Dictionary<int, DeskFrontend> Desks { get; set; } = new Dictionary<int, DeskFrontend>();
}
public class DeskReservationItemFrontend
{
    public bool IsUser { get; set; }
    public DateTime Date { get; set; }
    public int ClusterId { get; set; }
    public int DeskId { get; set; }
    public int TimeSlot { get; set; }
}
public class DeskReservationsDayFrontend
{
    public List<DeskReservationItemFrontend> Occupied { get; set; } = new List<DeskReservationItemFrontend>();
    public List<DeskReservationItemFrontend> UserReservations { get; set; } = new List<DeskReservationItemFrontend>();
}
