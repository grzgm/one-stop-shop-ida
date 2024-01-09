using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneStopShopIdaBackend.Models;

public class LunchDaysItem
{
    public LunchDaysItem()
    {
    }

    public LunchDaysItem(string microsoftId, LunchDaysItemFrontend lunchDaysItemFrontend)
    {
        this.MicrosoftId = microsoftId;
        this.Monday = lunchDaysItemFrontend.Monday;
        this.Tuesday = lunchDaysItemFrontend.Tuesday;
        this.Wednesday = lunchDaysItemFrontend.Wednesday;
        this.Thursday = lunchDaysItemFrontend.Thursday;
        this.Friday = lunchDaysItemFrontend.Friday;
    }

    [Key]
    [ForeignKey(nameof(UserItem))]
    [StringLength(255)]
    [Required]
    public string MicrosoftId { get; set; }

    [Required] public bool Monday { get; set; }
    [Required] public bool Tuesday { get; set; }
    [Required] public bool Wednesday { get; set; }
    [Required] public bool Thursday { get; set; }
    [Required] public bool Friday { get; set; }

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

    public bool IsRegisteredOnDate(DateTime date)
    {
        DayOfWeek currentDay = date.DayOfWeek;

        switch (currentDay)
        {
            case DayOfWeek.Monday:
                return Monday;

            case DayOfWeek.Tuesday:
                return Tuesday;

            case DayOfWeek.Wednesday:
                return Wednesday;

            case DayOfWeek.Thursday:
                return Thursday;

            case DayOfWeek.Friday:
                return Friday;

            default:
                return false; // Handle other days as needed
        }
    }
}

public class LunchDaysItemFrontend
{
    public LunchDaysItemFrontend()
    {
    }

    public LunchDaysItemFrontend(LunchDaysItem lunchDaysItemFrontend)
    {
        this.Monday = lunchDaysItemFrontend.Monday;
        this.Tuesday = lunchDaysItemFrontend.Tuesday;
        this.Wednesday = lunchDaysItemFrontend.Wednesday;
        this.Thursday = lunchDaysItemFrontend.Thursday;
        this.Friday = lunchDaysItemFrontend.Friday;
    }

    public bool Monday { get; set; }
    public bool Tuesday { get; set; }
    public bool Wednesday { get; set; }
    public bool Thursday { get; set; }
    public bool Friday { get; set; }
}