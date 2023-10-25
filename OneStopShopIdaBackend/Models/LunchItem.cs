using System.ComponentModel.DataAnnotations;

namespace OneStopShopIdaBackend.Models
{
    public class LunchItem
    {
        [Key]
        [StringLength(255)]
        public string  MicrosoftId { get; set; }
        [Key]
        [StringLength(20)]
        public string DayName { get; set; }
        [Required]
        public string Registeres { get; set; }
    }
}
