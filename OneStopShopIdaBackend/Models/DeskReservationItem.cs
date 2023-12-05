using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OneStopShopIdaBackend.Models;

[PrimaryKey(nameof(Office), nameof(Date), nameof(ClusterId), nameof(DeskId), nameof(TimeSlot))]
public class DeskReservationItem
{
    public DeskReservationItem() { }

    public DeskReservationItem(string microsoftid, string office, DateTime date, int clusterid, int deskid, int timeslot)
    {
        MicrosoftId = microsoftid;
        Office = office;
        Date = date;
        ClusterId = clusterid;
        DeskId = deskid;
        TimeSlot = timeslot;
    }

    [Key]
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
