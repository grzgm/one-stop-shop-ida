using System.ComponentModel.DataAnnotations;

namespace OneStopShopIdaBackend.Models
{
    public class LunchTodayItem
    {
        [Key]
        [StringLength(255)]
        [Required]
        public string MicrosoftId { get; set; }
        [Required]
        public bool IsRegistered { get; set; }
    }
}
