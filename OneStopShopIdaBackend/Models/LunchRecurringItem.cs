using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneStopShopIdaBackend.Models;
public class LunchRecurringItem
{
    public LunchRecurringItem()
    {
    }
    public LunchRecurringItem(string microsoftId, LunchRecurringItemFrontend lunchRecurringItemFrontend)
    {
        this.MicrosoftId = microsoftId;
        this.Monday = lunchRecurringItemFrontend.Monday;
        this.Tuesday = lunchRecurringItemFrontend.Tuesday;
        this.Wednesday = lunchRecurringItemFrontend.Wednesday;
        this.Thursday = lunchRecurringItemFrontend.Thursday;
        this.Friday = lunchRecurringItemFrontend.Friday;
    }
    [Key]
    [ForeignKey(nameof(UserItem))]
    [StringLength(255)]
    [Required]
    public string MicrosoftId { get; set; }
    [Required]
    public bool Monday { get; set; }
    [Required]
    public bool Tuesday { get; set; }
    [Required]
    public bool Wednesday { get; set; }
    [Required]
    public bool Thursday { get; set; }
    [Required]
    public bool Friday { get; set; }

    public bool IsRegistered()
    {
        if (Monday) return true;
        if (Tuesday) return true;
        if (Wednesday) return true;
        if (Thursday) return true;
        if (Friday) return true;

        return false;
    }

    public override string ToString()
    {
        var selectedDays = new List<string>();

        if (Monday) selectedDays.Add("Monday");
        if (Tuesday) selectedDays.Add("Tuesday");
        if (Wednesday) selectedDays.Add("Wednesday");
        if (Thursday) selectedDays.Add("Thursday");
        if (Friday) selectedDays.Add("Friday");

        return string.Join(", ", selectedDays);
    }
}

public class LunchRecurringItemFrontend
{
    public LunchRecurringItemFrontend()
    {
    }
    public LunchRecurringItemFrontend(LunchRecurringItem lunchRecurringItemFrontend)
    {
        this.Monday = lunchRecurringItemFrontend.Monday;
        this.Tuesday = lunchRecurringItemFrontend.Tuesday;
        this.Wednesday = lunchRecurringItemFrontend.Wednesday;
        this.Thursday = lunchRecurringItemFrontend.Thursday;
        this.Friday = lunchRecurringItemFrontend.Friday;
    }
    public bool Monday { get; set; }
    public bool Tuesday { get; set; }
    public bool Wednesday { get; set; }
    public bool Thursday { get; set; }
    public bool Friday { get; set; }
}