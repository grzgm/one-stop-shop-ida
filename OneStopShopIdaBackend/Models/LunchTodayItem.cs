using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneStopShopIdaBackend.Models
{
    public class LunchTodayItem
    {
        [Key]
        [ForeignKey(nameof(UserItem))]
        [StringLength(255)]
        [Required]
        public string MicrosoftId { get; set; }

        [Required]
        public bool IsRegistered { get; set; }
        
        [StringLength(45)]
        [Required]
        public string? Office { get; set; }
    }
    public class LunchTodayItemFrontend
    {
        public bool IsRegistered { get; set; }
        public string? Office { get; set; }

        public LunchTodayItemFrontend(bool isRegistered, string office)
        {
            IsRegistered = isRegistered;
            Office = office;
        }

        public LunchTodayItemFrontend(LunchTodayItem lunchTodayItem)
        {
            IsRegistered = lunchTodayItem.IsRegistered;
            Office = lunchTodayItem.Office;
        }
    }
}