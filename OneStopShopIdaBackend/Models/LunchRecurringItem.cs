using System.ComponentModel.DataAnnotations;

namespace OneStopShopIdaBackend.Models
{
    public class LunchRecurringItem
    {
        [Key]
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
