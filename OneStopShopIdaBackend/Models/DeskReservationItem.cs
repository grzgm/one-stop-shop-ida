using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OneStopShopIdaBackend.Models;

[PrimaryKey(nameof(Office), nameof(Date), nameof(DeskClusterId), nameof(DeskId), nameof(TimeSlot))]
public class DeskReservationItem
{
    public DeskReservationItem() { }

    public DeskReservationItem(string microsoftid, string office, DateTime date, int deskclusterid, int deskid, int timeslot)
    {
        MicrosoftId = microsoftid;
        Office = office;
        Date = date;
        DeskClusterId = deskclusterid;
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
    public int DeskClusterId { get; set; }
    [Required]
    public int DeskId { get; set; }
    [Required]
    public int TimeSlot { get; set; }
}
