using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneStopShopIdaBackend.Models
{
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
        public string  MicrosoftId { get; set; }
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
    }
}
