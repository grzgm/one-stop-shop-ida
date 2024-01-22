using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneStopShopIdaBackend.Models;
public class OfficeFeaturesItem
{
    [Key]
    [StringLength(255)]
    [Required]
    public string OfficeName { get; set; }

    [Required]
    public bool CanReserveDesk { get; set; }

    [Required]
    public bool CanRegisterLunch { get; set; }

    [Required]
    public bool CanRegisterPresence { get; set; }
    [NotMapped]
    public OfficeInformationItem? OfficeInformation { get; set; }
}

public class OfficeInformationItem
{
    [Key]
    [ForeignKey(nameof(OfficeFeaturesItem))]
    [StringLength(255)]
    [Required]
    public string OfficeName { get; set; }

    [StringLength(255)]
    public string Address { get; set; }

    [StringLength(255)]
    public string OpeningHours { get; set; }

    [Column(TypeName = "TEXT")]
    public string AccessInformation { get; set; }

    [Column(TypeName = "TEXT")]
    public string ParkingInformation { get; set; }

    [Column(TypeName = "TEXT")]
    public string LunchInformation { get; set; }
    [NotMapped]

    public OfficeCoordinatesItem? OfficeCoordinates { get; set; }
}

public class OfficeCoordinatesItem
{
    [Key]
    [ForeignKey(nameof(OfficeFeaturesItem))]
    [StringLength(255)]
    [Required]
    public string OfficeName { get; set; }

    public double Lat { get; set; }

    public double Lng { get; set; }
}
