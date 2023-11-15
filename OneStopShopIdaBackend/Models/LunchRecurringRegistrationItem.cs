using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OneStopShopIdaBackend.Models
{
    public class LunchRecurringRegistrationItem
    {
        [Key]
        [ForeignKey(nameof(UserItem))]
        [StringLength(255)]
        [Required]
        public string MicrosoftId { get; set; }
        [Required]
        public bool IsRegistered { get; set; }
    }
}
