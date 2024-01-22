using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneStopShopIdaBackend.Models;

public class LunchRegistrationsItem
{
    [Key]
    [ForeignKey(nameof(UserItem))]
    [StringLength(255)]
    [Required]
    public string MicrosoftId { get; set; }

    public DateTime? RegistrationDate { get; set; }
    [StringLength(45)] public string? Office { get; set; }
}

public class LunchRegistrationsItemFrontend
{
    public DateTime? RegistrationDate { get; set; }
    public string? Office { get; set; }

    public LunchRegistrationsItemFrontend(DateTime? registrationDate, string office)
    {
        RegistrationDate = registrationDate;
        Office = office;
    }

    public LunchRegistrationsItemFrontend(LunchRegistrationsItem lunchRegistrationsItem)
    {
        RegistrationDate = lunchRegistrationsItem.RegistrationDate;
        Office = lunchRegistrationsItem.Office;
    }
}